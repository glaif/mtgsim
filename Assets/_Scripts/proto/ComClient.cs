using UnityEngine;
using Grpc.Core;
using NetworkService;

public class ComClient {
	public void ClientConnect() {
        Channel channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);

        var client = new NetworkService.NetworkService.NetworkServiceClient(channel);
        string user = "you";

        var reply = client.SayHello(new HelloRequest { Name = user });
        Debug.Log("Greeting: " + reply.Message);

        channel.ShutdownAsync().Wait();
    }
}
