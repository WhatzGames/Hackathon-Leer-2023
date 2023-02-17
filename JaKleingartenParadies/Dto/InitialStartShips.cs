#nullable enable
namespace JaKleingartenParadies.Dto;

public class InitialStartShips
{
    public int[] start { get; set; } = Array.Empty<int>();
    public string direction { get; set; } = string.Empty;
    public int size { get; set; }
}