#nullable enable
namespace JaKleingartenParadies.Dto;

public class InitialStartShips
{
    public int[] Start { get; set; } = Array.Empty<int>();
    public string Direction { get; set; } = string.Empty;
    public int Size { get; set; }
}