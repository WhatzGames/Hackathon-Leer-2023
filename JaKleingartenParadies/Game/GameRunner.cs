using System.Text;
using System.Text.Json;
using JaKleingartenParadies.Data;
using JaKleingartenParadies.Dto;
using JaKleingartenParadies.Heatmap;

namespace JaKleingartenParadies.Game;

public class GameRunner
{
    private readonly string _spielerId;
    private readonly GetHeatmapFromFlo _heatmap;
    private readonly List<List<string>> _grid;
    private readonly HashSet<(int i, int j)> _foundShipLocations;
    private readonly List<int> _existingShips;

    public GameRunner(string spielrId)
    {
        _spielerId = spielrId;
        _heatmap = new GetHeatmapFromFlo();
        _grid = new Grid().GetStartGrid();
        _foundShipLocations = new HashSet<(int i, int j)>();
        _existingShips = new List<int>
        {
            2,
            3,
            3,
            4,
            5
        };
    }

    public string SpielerId => _spielerId;

    public async Task<int[]> Round(string[][] board)
    {
        //logic
        UpdateRemainingShips(board);
        var probabilityMap = await _heatmap.GetHeatmap(_spielerId, TranslateBoard(board), _existingShips);
        var probability = GetHighestProbability(probabilityMap);
        
        // var value = JsonSerializer.Serialize(new
        // {
        //     board,
        //     probabilityMap,
        //     probability
        // });
        // File.AppendAllText($"ROUND_{_spielerId}.json", value);

        // var sb = new StringBuilder();
        // sb.AppendLine("board:");
        // for (var i = 0; i < board.Length; i++)
        // {
        //     for (var j = 0; j < board.Length; j++)
        //     {
        //         if (board[i][j] == "")
        //         {
        //             sb.Append(' ');
        //             continue;
        //         }
        //         sb.Append(board[i][j]);
        //     }
        //     sb.AppendLine();
        // }
        //
        // sb.AppendLine($"ships remaining: ({string.Join(',', _existingShips)})");
        // sb.Append($"next shot: ({probability[0]},{probability[1]})\n\n");
        // File.AppendAllText($"ROUND_{_spielerId}.json", sb.ToString());

        return probability;
    }

    private List<List<string>> TranslateBoard(string[][] board)
    {
        List<List<string>> floRow = new();
        foreach (string[] chars in board)
        {
            List<string> floColumn = new();
            foreach (string c in chars)
            {
                switch (c)
                {
                    case "X":
                        floColumn.Add("W");
                        break;
                    case "":
                        floColumn.Add("U");
                        break;
                    case "x":
                        floColumn.Add("H");
                        break;
                    case ".":
                        floColumn.Add("M");
                        break;
                    default:
                        throw new InvalidOperationException("Invalid Character");
                }
            }
            floRow.Add(floColumn);
        }

        return floRow;
    }

    public async Task<InitialStartShips[]> Set()
    {
        //logic
        await using BoardInitializer boardInitializer = new BoardInitializer();
        return await boardInitializer.GetRandomStartBoardAsync();
    }

    private int[] GetHighestProbability(List<List<double>> probabilitys)
    {
        int xCor = 0;
        int yCor = 0;
        double currentHigh = 0;
        
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
        
        return new int[] {yCor ,xCor };
    }

    public void UpdateRemainingShips(string[][] board)
    {
        for (var i = 0; i < board.Length; i++)
        {
            for (var j = 0; j < board[i].Length; j++)
            {
                if (board[i][j] == "X")
                {
                    FindHitShip(board, i, j);
                }
            }
        }
    }

    private void FindHitShip(string[][] board, int i, int j)
    {
        if (_foundShipLocations.Contains((i, j)))
        {
            return;
        }
        _foundShipLocations.Add((i, j));
        
        int length = 0;
        for (var boardI = i + 1; boardI < board.Length; boardI++)
        {
            if (board[boardI][j] != "X")
                break;

            length = (boardI - i) + 1;
            _foundShipLocations.Add((boardI, j));
        }

        if (length > 0)
        {
            _existingShips.Remove(length);
            return;
        }
        
        for (var boardJ = j + 1; boardJ < board[i].Length; boardJ++)
        {
            if (board[i][boardJ] != "X")
                break;

            length = (boardJ - j) + 1;
            _foundShipLocations.Add((i, boardJ));
        }

        if (length > 0)
        {
            _existingShips.Remove(length);
        }
    }
    



}