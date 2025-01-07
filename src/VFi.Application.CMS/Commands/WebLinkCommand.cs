using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using VFi.Application.CMS.Commands.Validations;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands;

public class WebLinkCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string Image { get; set; }
    public string Image2 { get; set; }
    public string Image3 { get; set; }
    public string Url { get; set; }
    public Guid? ParentWebLinkId { get; set; }
    public Guid? GroupWebLinkId { get; set; }
    public int Status { get; set; }
    public int DisplayOrder { get; set; }
    public string? Keywords { get; set; }
}

public class WebLinkAddCommand : WebLinkCommand
{
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public WebLinkAddCommand(
        Guid id,
        string code,
        string name,
        string? description,
        Guid? parentWebLinkId,
        Guid? groupWebLinkId,
        int status,
        int displayOrder,
        string? keywords,
        Guid createdBy,
        DateTime createdDate,
        string? createdName)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        ParentWebLinkId = parentWebLinkId;
        GroupWebLinkId = groupWebLinkId;
        Status = status;
        DisplayOrder = displayOrder;
        Keywords = keywords;
        CreatedBy = createdBy;
        CreatedDate = createdDate;
        CreatedByName = createdName;
    }
    public bool IsValid(IWebLinkRepository _context)
    {
        ValidationResult = new WebLinkAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class WebLinkEditCommand : WebLinkCommand
{
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedByName { get; set; }
    public WebLinkEditCommand(
       Guid id,
        string code,
        string name,
        string? description,
        Guid? parentWebLinkId,
        Guid? groupWebLinkId,
        int status,
        int displayOrder,
        string? keywords,
        Guid? updatedBy,
        DateTime? updatedDate,
        string? updatedName)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        DisplayOrder = displayOrder;
        Keywords = keywords;
        ParentWebLinkId = parentWebLinkId;
        GroupWebLinkId = groupWebLinkId;
        UpdatedBy = updatedBy;
        UpdatedDate = updatedDate;
        UpdatedByName = updatedName;
    }
    public bool IsValid(IWebLinkRepository _context)
    {
        ValidationResult = new WebLinkEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class WebLinkSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public WebLinkSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}

public class WebLinkDeleteCommand : WebLinkCommand
{
    public WebLinkDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IWebLinkRepository _context)
    {
        ValidationResult = new WebLinkDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
