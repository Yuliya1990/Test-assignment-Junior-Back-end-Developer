namespace TestProject.Models
{
    public class CharacterOriginResult
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Species { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public OriginResult Origin { get; set; }
        public CharacterOriginResult(Character character)
        {
            Name = character.Name;
            Status = character.Status;
            Species = character.Species;
            Type = character.Type;
            Gender = character.Gender;
            Origin = new OriginResult(character.Origin);
        }
    }

    public class OriginResult
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Dimension { get; set; }
        public OriginResult(Location origin)
        {
            Name=origin.Name;
            Type = origin.Type;
            Dimension = origin.Dimension;
        }
    }
}
