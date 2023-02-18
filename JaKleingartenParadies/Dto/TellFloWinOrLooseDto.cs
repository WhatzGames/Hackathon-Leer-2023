namespace JaKleingartenParadies.Dto;

public class TellFloWinOrLooseDto
{
    public string status => "success";
    public string title => $"Stats {DateTime.Now:O}";
    public string message { get; set; }
}