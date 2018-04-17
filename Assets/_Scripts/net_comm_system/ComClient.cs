using UnityEngine;
using Grpc.Core;
using static NetworkService.NetworkService;
using NetworkService;
using System.Collections.Generic;

public class ComClient : ComParent {

    // TODO: Bypass network stack for local client

    public ComClient(string ipaddr, string port, string userName) {
        SetNetData(ipaddr, port, userName);
    }

    public bool Subscribe() {
        GetClient();
        var reply = Client.Subscribe(new SubscribeRequest { Channel = ChannelType.Clientstate, PlayerName = UserName });
        Debug.Log("JoinGame REPLY: " + reply.ResCode);
        if (reply.ResCode == SubscribeReplyStatus.Success)
            return true;
        return false;
    }

    private ClientStateUpdateType MatchUpdateType(string updateType) {
        ClientStateUpdateType res = ClientStateUpdateType.Null;
        
        switch (MainGameScript.Str2CST(updateType)) {
            case (MainGameScript.ClientStateTypes.CST_COUNT):
                res = ClientStateUpdateType.Cardcount;
                break;
            case (MainGameScript.ClientStateTypes.CST_MULLIGAN):
                res = ClientStateUpdateType.Mulligan;
                break;
            default:
                Debug.LogError("Error matching ClientStateType to a corresponding ClientStateUpdateType");
                break;
        }
        return res;
    }

    public bool ClientStateUpdate(string updateType, Dictionary<string, object> updateParams) {
        GetClient();

        ClientStateUpdateRequest req = new ClientStateUpdateRequest();
        req.Type = MatchUpdateType(updateType);
        req.Params. = updateParams


        var reply = Client.ClientStateUpdate(req);
        Debug.Log("JoinGame REPLY: " + reply.ResCode);
        if (reply.ResCode == ClientStateUpdateReplyStatus.Success)
            return true;
        return false;
    }
}
