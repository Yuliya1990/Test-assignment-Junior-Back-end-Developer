using System.Text.RegularExpressions;
using TestProject.Models;

namespace TestProject
{
    public class ParseLocationURL
    {
        private string _baseUrl = "api/location/";
        public List<string> GetOriginUrlListFromCharacters(List<Character> characters)
        {
            List<string> originsUrl = new List<string>();
            foreach (Character character in characters)
            {
                originsUrl.Add(character.Origin.Url);
            }
            return originsUrl;
        }

        public string GetNewLocationsUrl(List<string> OriginURLs)
        {
            List<string> LocationIDs = GetLocationIDFromURL(OriginURLs);
            return GenereateLocationsUrl(LocationIDs);
        }

        private List<string> GetLocationIDFromURL(List<string> OriginURLs)
        {
            List<string> LocationIDs = new List<string>();
            foreach (string url in OriginURLs)
            {
                if (url != "")
                {
                    string locationId = Regex.Match(url, @"\d+").Value;
                    if (!LocationIDs.Contains(locationId))
                    {
                        LocationIDs.Add(locationId);
                    }
                }
            }
            return LocationIDs;
        }

        private string GenereateLocationsUrl(List<string> LocationIDs)
        {
            string filter = _baseUrl;
            if (LocationIDs.Count != 0)
            {
                filter += String.Join(',', LocationIDs);
                return filter;
            }
            else return ("");
        }
    }
}
