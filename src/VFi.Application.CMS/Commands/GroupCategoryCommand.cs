using VFi.Application.CMS.Commands.Validations;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands;

public class GroupCategoryCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public string Logo { get; set; }
    public string Logo2 { get; set; }
    public string Favicon { get; set; }
    public string Url { get; set; }
    public string Tags { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Facebook { get; set; }
    public string Youtube { get; set; }
    public string Zalo { get; set; }
    public int Status { get; set; }
    public int DisplayOrder { get; set; }
}

public class GroupCategoryAddCommand : GroupCategoryCommand
{
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public GroupCategoryAddCommand(
        Guid id,
        string code,
        string? name,
        string? description,
        int status,
        int displayOrder,
        Guid createdBy,
        DateTime createdDate,
        string? createdByName)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        DisplayOrder = displayOrder;
        CreatedBy = createdBy;
        CreatedDate = createdDate;
        CreatedByName = createdByName;
    }

    public GroupCategoryAddCommand()
    {
    }

    public bool IsValid(IGroupCategoryRepository _context)
    {
        ValidationResult = new GroupCategoryAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class GroupCategoryEditCommand : GroupCategoryCommand
{
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedByName { get; set; }
    public GroupCategoryEditCommand(
       Guid id,
        string code,
        string? name,
        string? description,
        int status,
        int displayOrder,
        Guid? updatedBy,
        DateTime? updatedDate,
        string? updatedByName)
    {
        Id = id;
        Code = code;
        Name = name;
        Description = description;
        Status = status;
        DisplayOrder = displayOrder;
        UpdatedBy = updatedBy;
        UpdatedDate = updatedDate;
        UpdatedByName = updatedByName;
    }

    public GroupCategoryEditCommand()
    {
    }

    public bool IsValid(IGroupCategoryRepository _context)
    {
        ValidationResult = new GroupCategoryEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class EditGroupCategorySortCommand : GroupCategoryCommand
{
    public IEnumerable<GroupCategorySortDto> List { get; set; }

    public EditGroupCategorySortCommand(IEnumerable<GroupCategorySortDto> list)
    {
        List = list;
    }
}
public class GroupCategoryDeleteCommand : GroupCategoryCommand
{
    public GroupCategoryDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IGroupCategoryRepository _context)
    {
        ValidationResult = new GroupCategoryDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
