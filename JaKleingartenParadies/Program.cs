// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using JaKleingartenParadies.Dto;
using SocketIOClient;
using SocketIOClient.Transport;

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
    BotDto data = response.GetValue<BotDto>();

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
            await Round(data, response);
            Console.WriteLine("Round");
            break;
    }
});


while (!gotDisconnected)
{
    await Task.Delay(10000);
    Console.WriteLine("Still alive!");
}


void Init(BotDto botDto)
{
    
}


void Result(BotDto botDto)
{
}

Task Set(BotDto botDto, SocketIOResponse socketIoResponse)
{
    throw new NotImplementedException();
}


Task Round(BotDto botDto, SocketIOResponse socketIoResponse)
{
    throw new NotImplementedException();
}
