using System.Text.Json;
using JaKleingartenParadies.Dto;
using SocketIOClient;
using SocketIOClient.Transport;

namespace JaKleingartenParadies.Game;

public class Bot
{
    private readonly string _secret;
    Dictionary<string, GameRunner> _games = new ();
    private SocketIO _client;
    
    int wins = 0;
    int losses = 0;

    public Dictionary<string, GameRunner > CurrentGames
    {
        get => _games;
    }

    public Bot(string secret)
    {
        _secret = secret;
        _client = new SocketIO("https://games.uhno.de", new SocketIOOptions() {Transport = TransportProtocol.WebSocket});

    }

    public async Task Start()
    {
        _client.On("disconnect", (e) =>
        {
            Console.WriteLine("Disconnected");
        });
        
        _client.OnConnected += (sender, e) =>
        {
            Console.WriteLine("Connected");
        };
        
        await _client.ConnectAsync();
        Console.WriteLine("trying to Authenticate");
        await _client.EmitAsync("authenticate",
            (success) => { Console.WriteLine($"Authentication successful: {success}"); }, _secret);
        
        _client.On("data", async (response) =>
        {
            InitDto data = response.GetValue<InitDto>();
        
            switch (data.type)
            {
                case "INIT":
                    Console.WriteLine("Init");
                    Init(data);
        
                    break;
                case "RESULT":
                    Result(data);
                    Console.WriteLine("Result");
                    break;
        
                case "SET":
                    await Set(data, response);
                    Console.WriteLine("Set");
                    break;
                case "ROUND":
                    BotDto botData = response.GetValue<BotDto>();
                    await Round(botData, response);
                    Console.WriteLine("Round");
                    break;
            }
        });
        
        // Send stats to teams channel
        /*_ = Task.Run(async () =>
        {
            // var timer = new PeriodicTimer(TimeSpan.FromMinutes(30));
            // while (true)
            // {
            //     Console.WriteLine("Uploading stats");
            //     try
            //     {
            //         int winsCopy = wins;
            //         int lossesCopy = losses;
            //         wins = 0;
            //         losses = 0;
            //         await new TellFloWinOrLoose().Send(winsCopy, lossesCopy);
            //         Console.WriteLine("Uploaded stats");
            //     }
            //     catch (Exception ex)
            //     {
            //         Console.WriteLine("Uploading stats failed: {0}", ex);
            //     }
            //     await timer.WaitForNextTickAsync();
            // }
        });*/
        
    }
    
    protected virtual void Init(InitDto botDto)
    {
        if (!_games.ContainsKey(botDto.id))
        {
            _games[botDto.id] = new GameRunner( botDto.self );
        }
    }

    protected virtual async Task Result(InitDto botDto)
    {
        try
        {
            foreach (var player in botDto.players)
            {
                if (player.id.Equals(_games[botDto.id].SpielerId))
                {
                    if (player.score == 0)
                    {
                        Console.WriteLine("we lost :-(");
                        File.AppendAllText("results.txt", $"{DateTime.Now:O} - lost game, game={botDto.id}, self={botDto.self}\n");
                        losses++;
                    }
                    else
                    {
                        Console.WriteLine("We won! ;-)");
                        File.AppendAllText("results.txt", $"{DateTime.Now:O} - won game, game={botDto.id}, self={botDto.self}\n");
                        wins++;
                    }
                    break;
                }
            }
            
            _games.Remove(botDto.id);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception in Result: {0}", ex);
        }
    }

    protected virtual async Task Set(InitDto botDto, SocketIOResponse socketIoResponse)
    {
        var resultShips = await _games[botDto.id].Set();
        var resultShipsString = JsonSerializer.Serialize(resultShips);
        Console.WriteLine("set: {0}", resultShipsString);
        await socketIoResponse.CallbackAsync(resultShips.ToList());
    }


    protected virtual async Task Round(BotDto botDto, SocketIOResponse socketIoResponse)
    {
        var playerIndex = Array.FindIndex(botDto.players, players => players.id != botDto.self);
        //TODO: Write index into class in init?
        var shoot = await _games[botDto.id].Round(botDto.boards[playerIndex]);
        //todo: check ob response string oder array sein muss
        var shootString = JsonSerializer.Serialize(shoot);
        Console.WriteLine("round: id {0} {1}", botDto.self[^3..^1], shootString);
        await socketIoResponse.CallbackAsync(shoot.ToList());
    }
}