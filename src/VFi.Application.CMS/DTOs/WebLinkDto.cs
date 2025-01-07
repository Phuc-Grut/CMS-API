

using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Domain.CMS.Models;

namespace VFi.Application.CMS.DTOs;

public class WebLinkDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string FullName { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public string Image2 { get; set; }
    public string Image3 { get; set; }
    public string Url { get; set; }
    public string ParentWebLinkName { get; set; }
    public Guid? ParentWebLinkId { get; set; }
    public string Keywords { get; set; }
    public int Status { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public Guid? GroupWebLinkId { get; set; }

    public string ParentIds { get; set; }
    public int? Level { get; set; }
    public string GroupWebLinkCode { get; set; }
    public string GroupWebLinkName { get; set; }
    public string? CreatedByName { get; set; }
    public string? UpdatedByName { get; set; }

    public List<WebLinkDto> Children { get; set; }
}
public class WebLinkListBoxDto
{
    public Guid Value { get; set; }
    public string Label { get; set; }
    public string? Key { get; set; }
    public bool Expanded { get; set; }
    public int? DisplayOrder { get; set; }
    public Guid? ParentWebLinkId { get; set; }
    public List<WebLinkListBoxDto> Children { get; set; }
}
public class WebLinkComboboxDto
{
    public Guid Value { get; set; }
    public string Label { get; set; }
    public string? Key { get; set; }
    public int? DisplayOrder { get; set; }
    public Guid? ParentWebLinkId { get; set; }
}
public class WebLinkQueryParams
{
    public int? Status { get; set; }
    public string? GroupWebLinkId { get; set; }
    public string? ParentWebLinkId { get; set; }
}
public class WebLinkParentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
public class WebLinkListViewDto
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? FullName { get; set; }
    public string? Description { get; set; }
    public string Image { get; set; }
    public string Image2 { get; set; }
    public string Image3 { get; set; }
    public string Url { get; set; }
    public Guid? ParentWebLinkId { get; set; }
    public string? ParentIds { get; set; }
    public Guid? GroupWebLinkId { get; set; }
    public string? GroupWebLinkCode { get; set; }
    public string? GroupWebLinkName { get; set; }
    public int? Level { get; set; }
    public int DisplayOrder { get; set; }
    public int? Status { get; set; }

}
