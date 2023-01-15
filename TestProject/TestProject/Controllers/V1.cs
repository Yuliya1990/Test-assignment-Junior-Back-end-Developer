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
            List<Character> characters;
            List<Episode> episodes;
            try
            {
                if (!_cache.TryGetValue(personName, out characters))
                {
                    characters = new List<Character>();
                    var responseCharacter = await httpClient.GetFromJsonAsync<MultipleResponseCharacter>($"api/character/?name={personName}");
                    characters.AddRange(responseCharacter.Characters);
                    while (responseCharacter.Info.Next != null)
                    {
                        responseCharacter = await httpClient.GetFromJsonAsync<MultipleResponseCharacter>(responseCharacter.Info.Next);
                        characters.AddRange(responseCharacter.Characters);
                    }
                    _cache.Set(personName, characters,
                   new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2)));
                }

                if (!_cache.TryGetValue(episodeName, out episodes))
                {
                    var responseEpisode = await httpClient.GetFromJsonAsync<MultipleResonseEpisode>($"api/episode/?name={episodeName}");
                    episodes = new List<Episode>();
                    episodes.AddRange(responseEpisode.Episodes);
                    while (responseEpisode.Info.Next != null)
                    {
                        responseEpisode = await httpClient.GetFromJsonAsync<MultipleResonseEpisode>(responseEpisode.Info.Next);
                        episodes.AddRange(responseEpisode.Episodes);
                    }
                    _cache.Set(episodeName, episodes,
                  new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2)));
                }
                bool isCharacterInEpisode = DataHandler.IsCharacterInTheEpisode(characters, episodes);
                return Ok(isCharacterInEpisode);
            }
            catch (HttpRequestException)
            {
                return StatusCode(StatusCodes.Status404NotFound);
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
            List<Character> characters;
            try
            {
                if (!_cache.TryGetValue(name, out characters))
                {
                    characters = new List<Character>();
                    var response = await httpClient.GetFromJsonAsync<MultipleResponseCharacter>($"api/character/?name={name}");
                    characters.AddRange(response.Characters);
                    while (response.Info.Next != null)
                    {
                        response = await httpClient.GetFromJsonAsync<MultipleResponseCharacter>(response.Info.Next);
                        characters.AddRange(response.Characters);
                    }
                    _cache.Set(name, characters,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(2)));
                }
                //якщо у нас один персонаж
                if (characters.Count == 1)
                {
                    Character character = characters.First();
                    string locationUrl = character.Origin.Url;
                    if (locationUrl == "")
                    {
                        return Ok(new CharacterOriginResult(character));
                    }
                    else
                    {
                        Location origin = await httpClient.GetFromJsonAsync<Location>(locationUrl);
                        character.Origin = origin;
                        return Ok(new CharacterOriginResult(character));
                    }
                }
                //якщо в нас багато персонажей
                ParseLocationURL Parser = new ParseLocationURL();
                List<string> OriginURLs = Parser.GetOriginUrlListFromCharacters(characters);
                string locationsUrl = Parser.GetNewLocationsUrl(OriginURLs);

                    if (locationsUrl == "")
                    {
                        var result = (from c in characters
                                      select new CharacterOriginResult(c)).ToList();
                        return Ok(result);
                    }
                    else
                    {
                        List<Location> origins = await httpClient.GetFromJsonAsync<List<Location>>(locationsUrl);
                        var union = DataHandler.UnionCharactersAndLocations(characters, origins);
                        var result = (from c in union
                                      select new CharacterOriginResult(c)).ToList();
                        return Ok(result);
                    }
            }
            catch (HttpRequestException)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (NotSupportedException) // When content type is not valid
            {
                return BadRequest("The content type is not supported.");
            }
            catch (System.Text.Json.JsonException) // Invalid JSON
            {
                return BadRequest("Invalid JSON.");
            }
            return Ok();
        }

        
    }
}


