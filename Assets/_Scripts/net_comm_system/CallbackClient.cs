using UnityEngine;
using Grpc.Core;
using static NetworkService.NetworkService;
using NetworkService;

public class CallbackClient : ComParent {

    // TODO: Bypass network stack for local client


    public CallbackClient(string URI, string playerName) {

    }

    public bool PingService() {
        return true;
    }

}
