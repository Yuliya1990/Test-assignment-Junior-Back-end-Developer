using TestProject.Models;

namespace TestProject
{
    public static class DataHandler
    {
        public static List<Character> UnionCharactersAndLocations(List<Character> characters, List<Location> origins)
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

        public static bool IsCharacterInTheEpisode(List<Character> characters, List<Episode> episodes)
        {
            //надо пройтись циклом по каждому персонажу. Пройтись циклом по каждому эпизоду.
            //И проверить, есть ли у персонажа хотя бы одна ссылка на эпизод, что совпадает хотя бы с одним url из массива епизодов
            //выбрать такого персонажа, у которого будет нужная нам ссылка
            var a = (from c in characters
                     where episodes.Any(e =>
                     {
                         foreach (string ep in c.Episodes)
                         {
                             if (ep == e.Url)
                                 return true;
                         }
                         return false;
                     })
                     select c).ToList();
            if (a.Count != 0) return true;
            return false;
        }
    }
}
