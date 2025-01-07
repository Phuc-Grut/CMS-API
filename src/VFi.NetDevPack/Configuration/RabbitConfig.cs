namespace VFi.NetDevPack.Configuration;

public partial class RabbitConfig
{
    public bool RabbitEnabled { get; set; }
    public bool RabbitCachePubSubEnabled { get; set; }
    public string RabbitHostName { get; set; }
    public string RabbitVirtualHost { get; set; }
    public string RabbitUsername { get; set; }
    public string RabbitPassword { get; set; }
    public string RabbitCacheReceiveEndpoint { get; set; }
    public bool ConsumerEnabled { get; set; }
    public bool PublisherEnabled { get; set; }

    public string Url
    {
        get
        {
            return string.Format("rabbitmq://{0}/{1}", RabbitHostName, RabbitVirtualHost);
        }
    }
    public Uri BuildEndPoint(string queueName)
    {
        return new Uri(Url + "/" + queueName);
    }
}
