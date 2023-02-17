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

    public async Task<int[]> Round(char[][] board)
    {
        //logic
        //todo: returnwert muss noch eingebaut werden als parameter von GetHeatmap
        UpdateRemainingShips(board);
        var probabilityMap = await _heatmap.GetHeatmap(TranslateBoard(board), _existingShips);
        
        return GetHighestProbability(probabilityMap);
    }

    private List<List<string>> TranslateBoard(char[][] board)
    {
        List<List<string>> floRow = new();
        foreach (char[] chars in board)
        {
            List<string> floColumn = new();
            foreach (char c in chars)
            {
                switch (c)
                {
                    case 'X':
                        floColumn.Add("W");
                        break;
                    case ' ':
                        floColumn.Add("U");
                        break;
                    case 'x':
                        floColumn.Add("H");
                        break;
                    case '.':
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

    public void UpdateRemainingShips(char[][] board)
    {
        for (var i = 0; i < board.Length; i++)
        {
            for (var j = 0; j < board[i].Length; j++)
            {
                if (board[i][j] == 'X')
                {
                    FindHitShip(board, i, j);
                }
            }
        }
    }

    private void FindHitShip(char[][] board, int i, int j)
    {
        if (_foundShipLocations.Contains((i, j)))
        {
            return;
        }
        _foundShipLocations.Add((i, j));
        
        int length = 0;

        for (var boardI = i+1; boardI<board.Length;boardI++)
        {
            if (board[boardI][j] != 'X')
                break;

            length = boardI - i;
            _foundShipLocations.Add((boardI, j));
        }

        if (length > 0)
        {
            _existingShips.Remove(length);
            return;
        }
        
        for (var boardJ = j+1; boardJ< board[i].Length; boardJ++)
        {
            if (board[i][boardJ] != 'X')
                break;

            length = boardJ - j;
            _foundShipLocations.Add((i, boardJ));
        }

        if (length > 0)
        {
            _existingShips.Remove(length);
        }
    }
    



}