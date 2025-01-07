using System;
using MediatR;

namespace VFi.NetDevPack.Messaging;


public class Event : Message, INotification
{
    public DateTime Timestamp { get; private set; }
    public string Tenant { get; set; }
    public string Data { get; set; }
    public string Data_Zone { get; set; }
    public Event()
    {
        Timestamp = DateTime.Now;
    }
}
