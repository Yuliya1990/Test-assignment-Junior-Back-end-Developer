using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TestProject.Models;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace TestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class V1 : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private IMemoryCache _cache;

        public V1(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _cache = memoryCache;
        }

        // POST: api/v1/check-person
        [HttpPost("check-person")]
        public async Task<IActionResult> CheckPerson(string personName, string episodeName)
        {
            var httpClient = _httpClientFactory.CreateClient("RickAndMorty");
            try
            {
                MultipleResponseCharacter responseCharacter = new MultipleResponseCharacter();
                MultipleResonseEpisode responseEpisode = new MultipleResonseEpisode();
                if (!_cache.TryGetValue(personName, out responseCharacter))
                {
                    responseCharacter = await httpClient.GetFromJsonAsync<MultipleResponseCharacter>($"api/character/?name={personName}");
                    _cache.Set(personName, responseCharacter,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2)));
                }
                if (!_cache.TryGetValue(episodeName, out responseEpisode))
                {
                    responseEpisode = await httpClient.GetFromJsonAsync<MultipleResonseEpisode>($"api/episode/?name={episodeName}");
                    _cache.Set(episodeName, responseEpisode,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2)));
                }
                //надо пройтись циклом по каждому персонажу. Пройтись циклом по каждому эпизоду.
                //И проверить, есть ли у персонажа хотя бы одна ссылка на эпизод, что совпадает хотя бы с одним url из массива епизодов

                //выбрать такого персонажа, у которого будет нужная нам ссылка

                   var a = (from c in responseCharacter.Characters
                            where responseEpisode.Episodes.Any(e =>
                            {
                                foreach (string ep in c.Episodes)
                                {
                                    if (ep == e.Url)
                                        return true;
                                }
                                return false;
                            })
                            select c).ToList();

                   if (a.Count != 0)
                       return Ok(true);
                return Ok(false);
                        
            }
            catch (HttpRequestException) // Non success
            {
                return BadRequest("An error occurred.");
            }
            catch (NotSupportedException) // When content type is not valid
            {
                return BadRequest("The content type is not supported.");
            }
            catch (System.Text.Json.JsonException) // Invalid JSON
            {
                return BadRequest("Invalid JSON.");
            }
            return null;
        }

        //GET: api/v1/person?name=person
        [HttpGet("person")]
        public async Task<IActionResult> GetPerson(string name)
        {
            var httpClient = _httpClientFactory.CreateClient("RickAndMorty");
            try
            {
                MultipleResponseCharacter response = new MultipleResponseCharacter();
                if (!_cache.TryGetValue(name, out response))
                {
                    response = await httpClient.GetFromJsonAsync<MultipleResponseCharacter>($"api/character/?name={name}");
                    _cache.Set(name, response,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2)));
                }
                string locationsUrl = GetUrlLocations(response.Characters);
                if (locationsUrl != "")
                {
                    var origins = await httpClient.GetFromJsonAsync<List<Location>>(locationsUrl);
                    var union = UnionCharactersAndLocations(response.Characters, origins);
                    var result = (from c in union
                                  select new
                                  {
                                      Name = c.Name,
                                      Status = c.Status,
                                      Species = c.Species,
                                      Type = c.Type,
                                      Gender = c.Gender,
                                      Origin = new
                                      {
                                          Name = c.Origin.Name,
                                          Type = c.Origin.Type,
                                          Dimension = c.Origin.Dimension
                                      }
                                  }).ToList();
                    return Ok(result);
                  
                }
                else { 
                        var result = (from c in response.Characters
                        select new
                        {
                            Name = c.Name,
                            Status = c.Status,
                            Species = c.Species,
                            Type = c.Type,
                            Gender = c.Gender,
                            Origin = new
                            {
                                Name = c.Origin.Name,
                                Type = c.Origin.Type,
                                Dimension = c.Origin.Dimension
                            }
                        }).ToList();
                    return Ok(result);
                };
            }
            catch (HttpRequestException) // Non success
            {
                return BadRequest("An error occurred.");
            }
            catch (NotSupportedException) // When content type is not valid
            {
                return BadRequest("The content type is not supported.");
            }
            catch (System.Text.Json.JsonException) // Invalid JSON
            {
                return BadRequest("Invalid JSON.");
            }
             return null;
        }

        private string GetUrlLocations(List<Character> characters)
        {
            string filter = "api/location/";
            List<string> LocationIDs = new List<string>();
            foreach (Character character in characters)
            {
                if (character.Origin.Url != "")
                {
                    string locationId = Regex.Match(character.Origin.Url, @"\d+").Value;
                    LocationIDs.Add(locationId);
                }
                
            }
            if (LocationIDs.Count != 0)
            {
                filter += String.Join(',', LocationIDs);
                return filter;
            }
            else return ("");
        }

        private List<Character> UnionCharactersAndLocations(List<Character> characters, List<Location> origins)
        {
            var joinedCharacterswithOrigins = (from c in characters
                                               join o in origins on c.Origin.Url equals o.Url
                                               select new Character
                                               {
                                                   Id = c.Id,
                                                   Name = c.Name,
                                                   Status = c.Status,
                                                   Species = c.Species,
                                                   Type = c.Type,
                                                   Gender = c.Gender,
                                                   Origin = new Location
                                                   {
                                                       Id = o.Id,
                                                       Name = o.Name,
                                                       Type = o.Type,
                                                       Dimension = o.Dimension
                                                   }
                                               }).ToList();

            var except = from c in characters
                         where !joinedCharacterswithOrigins.Any(j => j.Id == c.Id)
                         select c;

            joinedCharacterswithOrigins.AddRange(except);
            return joinedCharacterswithOrigins;
        }
    }
}


