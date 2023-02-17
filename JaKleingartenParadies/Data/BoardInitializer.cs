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
            _stream = File.OpenRead("Data/test-init.json");
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
            JsonSerializer.Serialize(true, Options);
            var boardInits = JsonSerializer.DeserializeAsyncEnumerable<InitialStartShips[]>(_stream, Options);
            int randomIndex = Random.Shared.Next(6);
            var counter = 0;
            await foreach (var boardInitDtos in boardInits)
            {
                if (counter < randomIndex)
                {
                    counter++;
                    continue;
                }
                return boardInitDtos!;
            }
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
}