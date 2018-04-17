using System.Collections.Generic;
using NetworkService;
using UnityEngine;

public class PubSubService {
    private List<ChannelType> clientChannels;
    private Dictionary<string, Subscriber> subscribers;
    private Dictionary<ChannelType, List<Subscriber>> channelLists;
    private ComServiceServer css;

    public PubSubService() {
        clientChannels = new List<ChannelType>();
        channelLists = new Dictionary<ChannelType, List<Subscriber>>();
        css = new ComServiceServer();
        css.StartService();
        CreateChannel(ChannelType.Clientstate);
    }

    ~PubSubService() {
        css.StopService();
    }

    private bool AddSubscriberToChannel(string subscriberName, string URI, ChannelType channelName) {
        // First check to see if the channel even exists
        if (clientChannels.Contains(channelName) == false) {
            Debug.LogError("Error trying to subscribe to non-existent channel.");
            return false;
        }

        // Now check to see if this is a new subscriber
        Subscriber sub;
        if (subscribers.TryGetValue(subscriberName, out sub) == false) {
            sub = new Subscriber(URI, subscriberName);
            subscribers.Add(subscriberName, sub);
        }

        if (channelLists.ContainsKey(channelName)) {
            // Subscriber list for channel exists
            if (channelLists[channelName].Contains(sub) == false) {
                // So just add the new subscriber to the channel
                channelLists[channelName].Add(new Subscriber(URI, subscriberName));
            } else {
                Debug.LogError("Trying to re-add a subscriber to a channel.");
                return false;
            }
        } else {
            // Subscriber list for channel does not exist yet
            // So create one and add the new subscriber to teh channel
            List<Subscriber> tempL = new List<Subscriber> {
                    new Subscriber(URI, subscriberName)
                };
            channelLists.Add(channelName, tempL);
        }
        return true;
    }

    public void CreateChannel(ChannelType name) {
        clientChannels.Add(name);
    }

    public bool Subscribe(string subscriberName, string URI, ChannelType channelName) {
        return AddSubscriberToChannel(subscriberName, URI, channelName);
    }

    public bool Unsubscribe() {
        return true;
    }

    public bool PublishGameStateUpdate() {
        return true;
    }

    public bool ProcessClientStateUpdate() {
        return true;
    }

}
