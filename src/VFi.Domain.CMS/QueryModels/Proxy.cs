using System;
using System.Collections.Generic;
using VFi.NetDevPack.Domain;

namespace VFi.Domain.CMS.Models;

public partial class Proxy : Entity, IAggregateRoot
{
    public Proxy()
    {
    }
    public string Code { get; set; }
    public string Host { get; set; }
    public int? Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool IsSecureConnection { get; set; }
    public bool? Status { get; set; }
    public string GroupName { get; set; }
    public string GroupId { get; set; }

}
