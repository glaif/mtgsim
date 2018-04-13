using UnityEngine;
using System.Threading.Tasks;
using Grpc.Core;
using static NetworkService.NetworkService;
using System;

public class ComService {

    class ServiceServer : NetworkServiceBase {

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
            return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
        }

    }

    private const int Port = 50051;
    private Server service = null;
    private bool running = false;

    public void StartService(bool isMaster, string port) {
        int Port = Int32.Parse(port);

        service = new Server {
            Services = { BindService(new ServiceServer()) },
            Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
        };
        service.Start();
        running = true;
        Debug.Log("ComService server listening on port " + Port);
    }

    public void StopService() {
        if (service != null) {
            Debug.Log("Stopping the ComService...");
            service.ShutdownAsync().Wait();
            service = null;
        }
        Debug.Log("ComService stopped.");
    }

    public bool IsRunning() {
        return running;
    }
}
