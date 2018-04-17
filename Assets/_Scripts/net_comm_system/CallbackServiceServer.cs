using UnityEngine;
using System.Threading.Tasks;
using Grpc.Core;
using static NetworkService.NetworkService;
using NetworkService;
using System;

public class CallbackServiceServer {

    class CallbackService : ComServiceParent {

        public override Task<GameStateUpdateReply> GameStateUpdate(GameStateUpdateRequest request, ServerCallContext context) {
            GameStateUpdateReply rep = new GameStateUpdateReply();
            return Task.FromResult(rep);
        }

    }

    private int servicePort;
    private Server serviceServer = null;
    private CallbackService service = null;
    private bool running = false;

    public void StartService(string port = "50052") {
        servicePort = Int32.Parse(port);
        service = new CallbackService();
        serviceServer = new Server {
            Services = { BindService(new CallbackService()) },
            Ports = { new ServerPort("localhost", servicePort, ServerCredentials.Insecure) }
        };
        serviceServer.Start();
        running = true;
        Debug.Log("CallbackService server listening on port " + servicePort);
    }

    public void StopService() {
        if (service != null) {
            Debug.Log("Stopping the CallbackService...");
            serviceServer.ShutdownAsync().Wait();
            serviceServer = null;
        }
        Debug.Log("CallbackService stopped.");
    }

    public bool IsRunning() {
        return running;
    }
}
