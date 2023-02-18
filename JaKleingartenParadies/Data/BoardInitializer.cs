#nullable enable
using System.Text;
using System.Text.Json;
using JaKleingartenParadies.Dto;

namespace JaKleingartenParadies.Data;

public class BoardInitializer : IAsyncDisposable
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web); 
    private readonly Stream _stream;
    public BoardInitializer()
    {
        try
        {
            var files = Directory.EnumerateFiles("Data/").Where(x => !x.StartsWith("init")).ToArray();
            var index = Random.Shared.Next(files.Length);
            _stream = File.OpenRead(files[index]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            _stream = new MemoryStream(Encoding.UTF8.GetBytes("[]"));
        }
    }

    public async Task<InitialStartShips[]> GetRandomStartBoardAsync()
    {
        try
        {
            return await JsonSerializer.DeserializeAsync<InitialStartShips[]>(_stream, Options);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        

        return Array.Empty<InitialStartShips>();
    }
    
    

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return _stream.DisposeAsync();
    }

    public async Task GenerateAlternativeBoards()
    {
        var boards = JsonSerializer.DeserializeAsyncEnumerable<InitialStartShips[]>(_stream, Options);
        var counter = 0;
        await foreach (var shipsArray in boards)
        {
            await using (var file = File.OpenWrite($"Data/{counter}.json"))
            {
                await JsonSerializer.SerializeAsync(file, shipsArray);
            };
            
            await using (var file = File.OpenWrite($"Data/{counter}-1.json"))
            {
                var flippedX = FlipBoard(shipsArray!, "h");
                await JsonSerializer.SerializeAsync(file, flippedX);
            };
            await using var fileFlippedY = File.OpenWrite($"Data/{counter}-2.json");
            var flippedY = FlipBoard(shipsArray!, "v").ToArray();
            await JsonSerializer.SerializeAsync(fileFlippedY, flippedY);
            
            await using var fileFlippedXY = File.OpenWrite($"Data/{counter}-3.json");
            var flippedXY = FlipBoard(flippedY!, "h");
            await JsonSerializer.SerializeAsync(fileFlippedXY, flippedXY);
            counter++;
        }
    }

    private IEnumerable<InitialStartShips> FlipBoard(IEnumerable<InitialStartShips> ships, string direction)
    {
        foreach (var initialStartShips in ships)
        {
            yield return Flip(initialStartShips, direction);
        }
    }

    public InitialStartShips Flip(InitialStartShips ship, string direction)
    {
        const float CenterAxis = 4.5f;

        int firstIndex = 0;
        int lastIndex = 1;
        int subtraction = ship.direction != direction ? 0 : ship.size - 1;

        if (direction is "v")
        {
            firstIndex = 1;
            lastIndex = 0;
        }
        
        var newFirstValue = (int) (CenterAxis + (CenterAxis - ship.start[firstIndex]) -  subtraction);
        var newLastValue = ship.start[lastIndex];

        var position = new int[2];
        position[firstIndex] = newFirstValue;
        position[lastIndex] = newLastValue;

        return new InitialStartShips
        {
            start = position,
            direction = ship.direction,
            size = ship.size
        };
    }
}