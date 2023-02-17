namespace JaKleingartenParadies.Heatmap;

public class Grid
{
    public Grid()
    {
        
    }

    /// <summary>
    /// U = unknown
    /// H = hit
    /// M = miss
    /// W = besigtes schiff
    /// </summary>
    /// <returns></returns>
    public List<List<string>> GetStartGrid()
    {
        List<List<string>> grid = new List<List<string>>()
        {
            new List<string>(){"U","U", "U", "U", "U", "U", "U", "U", "U", "U"},
            new List<string>(){"U","U", "U", "U", "U", "U", "U", "U", "U", "U"},
            new List<string>(){"U","U", "U", "U", "U", "U", "U", "U", "U", "U"},
            new List<string>(){"U","U", "U", "U", "U", "U", "U", "U", "U", "U"},
            new List<string>(){"U","U", "U", "U", "U", "U", "U", "U", "U", "U"},
            new List<string>(){"U","U", "U", "U", "U", "U", "U", "U", "U", "U"},
            new List<string>(){"U","U", "U", "U", "U", "U", "U", "U", "U", "U"},
            new List<string>(){"U","U", "U", "U", "U", "U", "U", "U", "U", "U"},
            new List<string>(){"U","U", "U", "U", "U", "U", "U", "U", "U", "U"},
            new List<string>(){"U","U", "U", "U", "U", "U", "U", "U", "U", "U"},
        };
        return grid;
    }
}