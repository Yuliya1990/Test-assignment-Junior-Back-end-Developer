using System.Text.RegularExpressions;
using TestProject.Models;

namespace TestProject
{
    public class ParseLocationURL
    {
        private string _baseUrl = "api/location/";

        /*      public string GetLocationsUrl(List<Character> characters)
              {
                  List<string> LocationIDs = GetLocationIDFromURL(characters);
                  return GenereateLocationsUrl(LocationIDs);
              }

              private List<string> GetLocationIDFromURL(List<Character> characters)
              {
                  List<string> LocationIDs = new List<string>();
                  foreach (Character character in characters)
                  {
                      if (character.Origin.Url != "")
                      {
                          string locationId = Regex.Match(character.Origin.Url, @"\d+").Value;
                          LocationIDs.Add(locationId);
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
        */
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
                    LocationIDs.Add(locationId);
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
