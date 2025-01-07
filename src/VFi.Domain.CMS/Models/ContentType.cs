using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.NetDevPack.Domain;

namespace VFi.Domain.CMS.Models;

public partial class ContentType : Entity, IAggregateRoot
{
    public ContentType()
    {
        Content = new HashSet<Content>();
    }
    public string? Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }
    public virtual ICollection<Content> Content { get; set; }
}
