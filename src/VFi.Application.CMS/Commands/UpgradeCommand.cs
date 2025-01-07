using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Consul;
using VFi.Application.CMS.Commands.Validations;
using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands;

public class UpgradeCommand : Command
{
    public Guid Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? ShortDescription { get; set; }
    public string? FullDescription { get; set; }
    public List<string>? ListIdCategories { get; set; }
    public string? Image { get; set; }
    public int Status { get; set; }
    public string? Tags { get; set; }
    public string? Slug { get; set; }

}
public class UpgradeAddCommand : UpgradeCommand
{
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? CreatedByName { get; set; }

    public UpgradeAddCommand
        (
        )
    {

    }
    public bool IsValid(IContentRepository _context)
    {
        ValidationResult = new UpgradeAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class UpgradeEditCommand : UpgradeCommand
{
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedByName { get; set; }
    public UpgradeEditCommand
        ()
    {

    }
    public bool IsValid(IContentRepository _context)
    {
        ValidationResult = new UpgradeEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class UpgradeDeleteCommand : UpgradeCommand
{
    public UpgradeDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IContentRepository _context)
    {
        ValidationResult = new UpgradeDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class UpgradeDuplicateCommand : UpgradeCommand
{
    public Guid? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public UpgradeDuplicateCommand(
        Guid id,
        string? code,
        string name,
        int status,
        Guid? createdBy,
        string? createdByName
    )
    {
        Id = id;
        Code = code;
        Name = name;
        Status = status;
        CreatedBy = createdBy;
        CreatedByName = createdByName;

    }
    public bool IsValid(IContentRepository _context)

    {
        ValidationResult = new UpgradeDuplicateCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
