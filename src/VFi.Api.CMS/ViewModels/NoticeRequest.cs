using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using VFi.Application.CMS.DTOs;

namespace VFi.Api.CMS.ViewModels;

public class AddNoticeRequest
{

    public string? Code { get; set; }
    public string Name { get; set; }
    public string Title { get; set; }
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    // public string? IdCategories { get; set; }
    public string? Image { get; set; }
    public List<string> ListIdCategories { get; set; }
    public int Status { get; set; }
    public string? Tags { get; set; }
    public bool? IsAuto { get; set; }
    public string? Slug { get; set; }
}
public class EditNoticeRequest : AddNoticeRequest
{
    public Guid Id { get; set; }
}
public class DuplicateNoticeRequest
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; }
    public int Status { get; set; }
    public bool? IsAuto { get; set; }

}
