using JaKleingartenParadies.Heatmap;

namespace JaKleingartenParadies.Game;

public class GameRunner
{
    public GameRunner()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    public async Task Game()
    {
        var heatmap = new GetHeatmapFromFlo();
        var grid = GetGrid();

        do
        {
            //returnwert muss noch eingebaut werden
            await heatmap.GetHeatmap(grid);
            // wähle schussfeld
        } while (true);
    }
    
    private List<List<string>> GetGrid()
    {
        var grid = new Grid();
        return grid.GetStartGrid();
    }

}