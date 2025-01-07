using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

namespace VFi.Api.CMS.ViewModels;

public class Sort
{
    public Guid Id { get; set; }
    public int SortOrder { get; set; }
}
public class SortRequest
{
    public List<Sort> SortList { get; set; } = null!;
}
