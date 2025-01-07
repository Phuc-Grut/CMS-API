using System;

namespace VFi.NetDevPack.Messaging;

public class QueueEvent
{
    public DateTime Timestamp { get; private set; }
    public string MessageType { get; protected set; }
    public Guid AggregateId { get; set; }
    public string AggregateName { get; set; }
    public string Tenant { get; set; }
    public string Data_Zone { get; set; }
    public string Data { get; set; }
    //public string Token { get; set; }
    public QueueEvent()
    {
        Timestamp = DateTime.Now;
    }
}
