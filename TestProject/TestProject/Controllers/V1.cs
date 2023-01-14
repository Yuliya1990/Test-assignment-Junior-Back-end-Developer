using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TestProject.Models;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Linq;

namespace TestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class V1 : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public V1(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/v1/first-character
        [HttpGet("first-character")]
        public async Task<Character> GetFirstCharacter()
        {
            var httpClient = _httpClientFactory.CreateClient("RickAndMorty");
            try
            {
                return await httpClient.GetFromJsonAsync<Character>("/api/character/1");
            }
            catch (HttpRequestException) // Non success
            {
                Console.WriteLine("An error occurred.");
            }
            catch (NotSupportedException) // When content type is not valid
            {
                Console.WriteLine("The content type is not supported.");
            }
            catch (System.Text.Json.JsonException) // Invalid JSON
            {
                Console.WriteLine("Invalid JSON.");
            }

            return null;
        }
        // POST: api/v1/check-person
        [HttpPost("check-person")]
        public async Task<IActionResult> CheckPerson(string personName, string episodeName)
        {
            //does the person exist in this episode?
            //можна знайти цього персонажа по імені.
            //Далі зібрати всі URL епізодів, в яких цей персонаж зустрічається і запхати ці url в масив.
            //пройтися по массиву і зробити get запит до кожного епізоду. Як тільки знайдемо з потрібним іменем - повертаємо true
            return Ok();
        }

        //GET: api/v1/person?name=person
        [HttpGet("person")]
        public async Task<IActionResult> GetPerson(string name)
        {
            var httpClient = _httpClientFactory.CreateClient("RickAndMorty");
            try
            {
                var response = await httpClient.GetFromJsonAsync<MultipleResponseCharacter>($"api/character/?name={name}");
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


/*var response = await httpClient.GetAsync($"/api/character/?name={name}");
var json = await response.Content.ReadAsStringAsync();
var jsonParsed = JsonDocument.Parse(json);
var results = jsonParsed.RootElement.GetProperty("results");
var resultsInJson = JsonSerializer.Serialize(results);
var characters = JsonSerializer.Deserialize<List<Character>>(resultsInJson.ToString());*/