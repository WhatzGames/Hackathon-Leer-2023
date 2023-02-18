// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using JaKleingartenParadies.Dto;
using JaKleingartenParadies.Game;
using SocketIOClient;
using SocketIOClient.Transport;

Dictionary<string, GameRunner> games = new Dictionary<string, GameRunner>();

const string Secret = "2d376eb7-ead4-4b7c-99c0-3a21515e8cd5";
bool gotDisconnected = false;

var client = new SocketIO("https://games.uhno.de", new SocketIOOptions() {Transport = TransportProtocol.WebSocket});

client.On("connect",
    async (e) =>
    {
        Console.WriteLine("trying to Authenticate");
        await client.EmitAsync("authenticate",
            (success) => { Console.WriteLine($"Authentication successful: {success}"); }, Secret);
    });

client.On("disconnect", (e) =>
{
    Console.WriteLine("Disconnected");
    gotDisconnected = true;
});

client.OnConnected += (sender, e) =>
{
    Console.WriteLine("Connected");
};

await client.ConnectAsync();
Console.WriteLine("trying to Authenticate");
await client.EmitAsync("authenticate",
    (success) => { Console.WriteLine($"Authentication successful: {success}"); }, Secret);

client.On("data", async (response) =>
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


while (!gotDisconnected)
{
    await Task.Delay(10000);
    Console.WriteLine("Still alive!");
}


void Init(InitDto botDto)
{
    if (!games.ContainsKey(botDto.id))
    {
        games[botDto.id] = new GameRunner( botDto.self );
    }
}


void Result(InitDto botDto)
{
    foreach (var player in botDto.players)
    {
        if (player.id.Equals(games[botDto.id].SpielerId))
        {
            if (player.score == 0)
            {
                Console.WriteLine("we lost :-(");
                File.AppendAllText("results.txt", $"{DateTime.Now:O} - won game {botDto.id}");
            }
            else
            {
                Console.WriteLine("We won! ;-)");
                File.AppendAllText("results.txt", $"{DateTime.Now:O} - lost game {botDto.id}");
            }

            break;
        }
    }
    
    games.Remove(botDto.id);
    
}

async Task Set(InitDto botDto, SocketIOResponse socketIoResponse)
{
    var resultShips = await games[botDto.id].Set();
    var resultShipsString = JsonSerializer.Serialize(resultShips);
    Console.WriteLine("set: {0}", resultShipsString);
    await socketIoResponse.CallbackAsync(resultShips.ToList());
}


async Task Round(BotDto botDto, SocketIOResponse socketIoResponse)
{
    var playerIndex = Array.FindIndex(botDto.players, players => players.id != botDto.self);
    //TODO: Write index into class in init?
    var shoot = await games[botDto.id].Round(botDto.boards[playerIndex]);
    //todo: check ob response string oder array sein muss
    var shootString = JsonSerializer.Serialize(shoot);
    Console.WriteLine("round: id {0} {1}", botDto.self[^3..^1], shootString);
    await socketIoResponse.CallbackAsync(shoot.ToList());
}
