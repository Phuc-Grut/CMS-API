using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;

namespace VFi.Api.CMS.ViewModels;

public class PagingRequest
{
    [FromQuery(Name = "$skip")]
    public int Skip { get; set; }
    [FromQuery(Name = "$top")]
    public int Top { get; set; }
    [FromQuery(Name = "$keyword")]
    public string? Keyword { get; set; }
}
public class FilterInfor
{
    public string? Key { get; set; }
    public string? Value { get; set; }
    public string? Ope { get; set; }
}
public class FilterQuery
{
    public string? Filter { get; set; }

    public string? Order { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }
    public string? Keyword { get; set; }
}
