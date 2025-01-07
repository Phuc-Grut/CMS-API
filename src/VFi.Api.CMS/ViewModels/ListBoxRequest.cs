using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Api.CMS.ViewModels;

namespace VFi.Api.CMS.ViewModels;

public class ListBoxRequest
{
    [FromQuery(Name = "$keyword")]
    public string? Keyword { get; set; }
}
