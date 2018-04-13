using UnityEngine;
using System.Threading.Tasks;
using Grpc.Core;
using NetworkService;

public class ComService {

    class ServiceServer : NetworkService.NetworkService.NetworkServiceBase {

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
            return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
        }

    }

    private const int Port = 50051;
    private Server service = null;

    public void StartService() {
        service = new Server {
            Services = { NetworkService.NetworkService.BindService(new ServiceServer()) },
            Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
        };
        service.Start();
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
}
