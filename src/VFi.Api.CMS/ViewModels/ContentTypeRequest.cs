using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Models;

namespace VFi.Api.CMS.ViewModels;

public class AddContentTypeRequest
{
    public string? Code { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Status { get; set; }
    public int DisplayOrder { get; set; }
}

public class EditContentTypeRequest : AddContentTypeRequest
{
    public string Id { get; set; }
}
public class ListBoxContentTypeRequest : ListBoxRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
}
public class PagingContentTypeRequest : PagingRequest
{
    [FromQuery(Name = "$status")]
    public int? Status { get; set; }
}
