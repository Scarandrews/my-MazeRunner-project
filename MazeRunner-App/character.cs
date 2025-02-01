public class Character
{
    public string Name{ get; set; }
    public char Token { get; set; }
    public string Skill { get; set; }
    public string Description { get; set; }

    public Character(string name, char token, string skill,string description)
    {
        Name = name;
        Token = token;
        Skill = skill;
        Description = description;
    }
}