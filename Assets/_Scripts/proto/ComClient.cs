using UnityEngine;
using Grpc.Core;
using static NetworkService.NetworkService;

public class ComClient {
	public void ClientConnect(string ipaddr, string port, string userName) {
        Channel channel = new Channel(ipaddr + ":" + port, ChannelCredentials.Insecure);

        var client = new NetworkServiceClient(channel);

        var reply = client.SayHello(new HelloRequest { Name = userName });
        Debug.Log("Greeting: " + reply.Message);

        channel.ShutdownAsync().Wait();
    }
}
