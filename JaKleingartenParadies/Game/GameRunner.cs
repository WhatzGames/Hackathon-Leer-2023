using JaKleingartenParadies.Data;
using JaKleingartenParadies.Dto;
using JaKleingartenParadies.Heatmap;

namespace JaKleingartenParadies.Game;

public class GameRunner
{
    private readonly string _spielerId;
    private readonly GetHeatmapFromFlo _heatmap;
    private readonly List<List<string>> _grid; 
    
    public GameRunner(string spielrId)
    {
        _spielerId = spielrId;
        _heatmap = new GetHeatmapFromFlo();
        _grid = new Grid().GetStartGrid();
    }

    public string SpielerId => _spielerId;

    public async Task<int[]> Round()
    {
        //logic
        //todo: returnwert muss noch eingebaut werden als parameter von GetHeatmap
        var probabilityMap = await _heatmap.GetHeatmap(_grid);
        
        return GetHighestProbability(probabilityMap);
    }

    public async Task<InitialStartShips[]> Set()
    {
        //logic
        BoardInitializer boardInitializer = new BoardInitializer();
        return await boardInitializer.GetRandomStartBoardAsync();
    }

    private int[] GetHighestProbability(List<List<decimal>> probabilitys)
    {
        int xCor = 0;
        int yCor = 0;
        decimal currentHigh = 0;
        
        
        for (int y = 0; y < probabilitys.Count; y++)
        {
            for (int x = 0; x < probabilitys[y].Count; x++)
            {
                if (currentHigh < probabilitys[y][x])
                {
                    currentHigh = probabilitys[y][x];
                    xCor = x;
                    yCor = y;
                }
            }
        }
        
        return new int[] {xCor ,yCor };
    }
    



}