using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TestProject.Models;


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
        
        // GET: api/v1/characters
        [HttpGet("first-character")]
        public async Task<IActionResult> GetFirstCharacter()
        {
            var httpClient = _httpClientFactory.CreateClient("RickAndMorty");
            var httpResponseMessage = httpClient.GetAsync("/api/character/1").Result;
            var json = httpResponseMessage.Content.ReadAsStringAsync().Result;
            //using Newtonsoft.Json
            var character = JsonConvert.DeserializeObject<Character>(json);
            return Ok(character);
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
            return Ok();
        }
    }

}


/*public class Rootobject
{
    public Info info { get; set; }
    public Result[] results { get; set; }
}

public class Info
{
    public int count { get; set; }
    public int pages { get; set; }
    public string next { get; set; }
    public object prev { get; set; }
}

public class Result
{
    public int id { get; set; }
    public string name { get; set; }
    public string status { get; set; }
    public string species { get; set; }
    public string type { get; set; }
    public string gender { get; set; }
    public Origin origin { get; set; }
    public Location location { get; set; }
    public string image { get; set; }
    public string[] episode { get; set; }
    public string url { get; set; }
    public DateTime created { get; set; }
}

public class Origin
{
    public string name { get; set; }
    public string url { get; set; }
}

public class Location
{
    public string name { get; set; }
    public string url { get; set; }
}

public class Rootobject
{
    public int id { get; set; }
    public string name { get; set; }
    public string air_date { get; set; }
    public string episode { get; set; }
    public string[] characters { get; set; }
    public string url { get; set; }
    public DateTime created { get; set; }
}*/

