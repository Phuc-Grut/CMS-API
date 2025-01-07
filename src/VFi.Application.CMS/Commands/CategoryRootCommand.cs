using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VFi.Application.CMS.Commands.Validations;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands;

public class CategoryRootCommand : Command
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string Title { get; set; }
    public string Image2 { get; set; }
    public string Image1 { get; set; }
    public string? Url { get; set; }
    public string? Slug { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public int Status { get; set; }
    public int DisplayOrder { get; set; }
    public int IdNumber { get; set; }
    public string? Keywords { get; set; }
    public string? JsonData { get; set; }
}
public class CategoryRootAddCommand : CategoryRootCommand
{
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? CreatedByName { get; set; }

    public CategoryRootAddCommand(
        Guid id,
        string code,
        string name,
        string? description,
        Guid? parentCategoryId,
        int status,
        int displayOrder,
        int idNumber,
        string? keywords,
        string? jsonData,
        Guid createdBy,
        DateTime createdDate,
        string? createdName)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        ParentCategoryId = parentCategoryId;
        Status = status;
        DisplayOrder = displayOrder;
        IdNumber = idNumber;
        Keywords = keywords;
        JsonData = jsonData;
        CreatedBy = createdBy;
        CreatedDate = createdDate;
        CreatedByName = createdName;
    }
    public bool IsValid(ICategoryRootRepository _context)
    {
        ValidationResult = new CategoryRootAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class CategoryRootEditCommand : CategoryRootCommand
{
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedByName { get; set; }
    public CategoryRootEditCommand(
        Guid id,
        string code,
        string name,
        string? description,
        Guid? parentCategoryId,
        int status,
        int displayOrder,
        int idNumber,
        string? keywords,
        string? jsonData,
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
        IdNumber = idNumber;
        Keywords = keywords;
        JsonData = jsonData;
        ParentCategoryId = parentCategoryId;
        UpdatedBy = updatedBy;
        UpdatedDate = updatedDate;
        UpdatedByName = updatedName;
    }
    public bool IsValid(ICategoryRootRepository _context)
    {
        ValidationResult = new CategoryRootEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class CategoryRootSortCommand : Command
{
    public List<SortItemDto> SortList { get; set; }
    public CategoryRootSortCommand(List<SortItemDto> sortList)
    {
        SortList = sortList;
    }
}

public class CategoryRootDeleteCommand : CategoryRootCommand
{
    public CategoryRootDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(ICategoryRootRepository _context)
    {
        ValidationResult = new CategoryRootDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
