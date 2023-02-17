namespace JaKleingartenParadies.Dto;

public class BotDto
{
    public string id { get; set; }
    public Players[] players { get; set; }
    public char[][] boards { get; set; }
    public Log[] log { get; set; }
    public string type { get; set; }
    public string self { get; set; }
}

public class Players
{
    public string id { get; set; }
    public string symbol { get; set; }
    public int score { get; set; }
}

public class Log
{
    public string player { get; set; }
    public int[] move { get; set; }
}

