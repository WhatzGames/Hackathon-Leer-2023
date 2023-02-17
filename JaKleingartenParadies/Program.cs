// See https://aka.ms/new-console-template for more information

using System.Runtime.Loader;
using SocketIOClient;
using SocketIOClient.Transport;

const string Secret = "2d376eb7-ead4-4b7c-99c0-3a21515e8cd5";

var client = new SocketIO("https://games.uhno.de", new SocketIOOptions() {Transport = TransportProtocol.WebSocket});

client.OnConnected += async (sender, e) =>
{
    await client.EmitAsync("authenticate", (success) =>
    {

    }, Secret);
};

client.On("disconnect", (e) => { });




Console.WriteLine("Hello, World!");

