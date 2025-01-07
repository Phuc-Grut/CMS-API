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

public class NewsCommand : Command
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
public class NewsAddCommand : NewsCommand
{
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? CreatedByName { get; set; }

    public NewsAddCommand
        (
        )
    {

    }
    public bool IsValid(IContentRepository _context)
    {
        ValidationResult = new NewsAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class NewsEditCommand : NewsCommand
{
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? UpdatedByName { get; set; }
    public NewsEditCommand
        ()
    {

    }
    public bool IsValid(IContentRepository _context)
    {
        ValidationResult = new NewsEditCommandValidation(_context, Id).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class NewsDeleteCommand : NewsCommand
{
    public NewsDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IContentRepository _context)
    {
        ValidationResult = new NewsDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class NewsDuplicateCommand : NewsCommand
{
    public Guid? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public NewsDuplicateCommand(
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
        ValidationResult = new NewsDuplicateCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
