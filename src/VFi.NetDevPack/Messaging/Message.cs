using System;

namespace VFi.NetDevPack.Messaging;

public class Message
{
    public string MessageType { get; set; }
    public Guid AggregateId { get; set; }

    public Message()
    {
        MessageType = GetType().Name;
    }
}
