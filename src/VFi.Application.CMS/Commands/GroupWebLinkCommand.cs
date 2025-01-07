using VFi.Application.CMS.Commands.Validations;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands;

public class GroupWebLinkCommand : Command
{

    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public string Image1 { get; set; }
    public string Image2 { get; set; }
    public string Url { get; set; }

    public int Status { get; set; }
    public int DisplayOrder { get; set; }
}

public class GroupWebLinkAddCommand : GroupWebLinkCommand
{
    public Guid? CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? CreatedByName { get; set; }
    public GroupWebLinkAddCommand(
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

    public GroupWebLinkAddCommand()
    {
    }

    public bool IsValid(IGroupWebLinkRepository _context)
    {
        ValidationResult = new GroupWebLinkAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}

public class GroupWebLinkEditCommand : GroupWebLinkCommand
{
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedByName { get; set; }
    public GroupWebLinkEditCommand(
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

    public GroupWebLinkEditCommand()
    {
    }

    public bool IsValid(IGroupWebLinkRepository _context)
    {
        ValidationResult = new GroupWebLinkEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class EditGroupWebLinkSortCommand : GroupWebLinkCommand
{
    public IEnumerable<GroupWebLinkSortDto> List { get; set; }

    public EditGroupWebLinkSortCommand(IEnumerable<GroupWebLinkSortDto> list)
    {
        List = list;
    }
}
public class GroupWebLinkDeleteCommand : GroupWebLinkCommand
{
    public GroupWebLinkDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IGroupWebLinkRepository _context)
    {
        ValidationResult = new GroupWebLinkDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
