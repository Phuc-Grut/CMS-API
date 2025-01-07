using VFi.Application.CMS.Commands.Validations;
using VFi.Domain.CMS.Interfaces;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands;

public class ItemCommand : Command
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? Size { get; set; }
    public bool IsFile { get; set; }
    public Guid? ParentId { get; set; }
    public string? MimeType { get; set; }
    public bool? HasChild { get; set; }
    public string? VirtualPath { get; set; }
    public string? LocalPath { get; set; }
    public string? Cdn { get; set; }
    public string? Product { get; set; }
    public int? Status { get; set; }
    public string? Workspace { get; set; }
    public string? Content { get; set; }
    public string? Tenant { get; set; }

}
public class ItemAddCommand : ItemCommand
{
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public ItemAddCommand(Guid id,
                          string name,
                          string? title,
                          string? description,
                          int? size,
                          bool isFile,
                          Guid? parentId,
                          string? mimeType,
                          bool? hasChild,
                           string? virtualPath,
                          string? localPath,
                          string? cdn,
                          string? product,
                          int? status,
                          string? workspace,
                          string? content,
                          string? tenant,
                          Guid? createdBy,
                          DateTime? createdDate)
    {
        Id = id;
        Name = name;
        Title = title;
        Description = description;
        Size = size;
        IsFile = isFile;
        ParentId = parentId;
        MimeType = mimeType;
        HasChild = hasChild;
        VirtualPath = virtualPath;
        LocalPath = localPath;
        Cdn = cdn;
        Product = product;
        Status = status;
        Workspace = workspace;
        Content = content;
        Tenant = tenant;
        Status = status;
        CreatedBy = createdBy;
        CreatedDate = createdDate;
    }
    public bool IsValid(IItemRepository _context)
    {
        ValidationResult = new ItemAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ItemEditCommand : ItemCommand
{
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public ItemEditCommand(Guid id,
                          string name,
                          string? title,
                          string? description,
                          int? size,
                          bool isFile,
                          Guid? parentId,
                          string? mimeType,
                          bool? hasChild,
                          string? localPath,
                          string? cdn,
                          string? product,
                          int? status,
                          string? workspace,
                          string? content,
                          string? tenant,
                          Guid? updatedBy,
                          DateTime? updatedDate)
    {
        Id = id;
        Name = name;
        Title = title;
        Description = description;
        Size = size;
        IsFile = isFile;
        ParentId = parentId;
        MimeType = mimeType;
        HasChild = hasChild;
        LocalPath = localPath;
        Cdn = cdn;
        Product = product;
        Status = status;
        Workspace = workspace;
        Content = content;
        Tenant = tenant;
        Status = status;
        UpdatedBy = updatedBy;
        UpdatedDate = updatedDate;
    }
    public bool IsValid(IItemRepository _context)
    {
        ValidationResult = new ItemEditCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class ItemDeleteCommand : ItemCommand
{
    public ItemDeleteCommand(Guid id)
    {
        Id = id;
    }
    public bool IsValid(IItemRepository _context)
    {
        ValidationResult = new ItemDeleteCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class FolderAddCommand : ItemCommand
{
    public Guid? CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public FolderAddCommand(Guid id,
                          string name,
                          string? title,
                          string? description,
                          int? size,
                          bool isFile,
                          Guid? parentId,
                          string? mimeType,
                          bool? hasChild,
                          string? localPath,
                          string? cdn,
                          string? product,
                          int? status,
                          string? workspace,
                          string? content,
                          string? tenant,
                          Guid? createdBy,
                          DateTime? createdDate)
    {
        Id = id;
        Name = name;
        Title = title;
        Description = description;
        Size = size;
        IsFile = isFile;
        ParentId = parentId;
        MimeType = mimeType;
        HasChild = hasChild;
        LocalPath = localPath;
        Cdn = cdn;
        Product = product;
        Status = status;
        Workspace = workspace;
        Content = content;
        Tenant = tenant;
        Status = status;
        CreatedBy = createdBy;
        CreatedDate = createdDate;
    }
    public bool IsValid(IItemRepository _context)
    {
        ValidationResult = new FolderAddCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
public class FolderEditCommand : ItemCommand
{
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public FolderEditCommand(Guid id,
                          string name,
                          string? title,
                          string? description,
                          int? size,
                          bool isFile,
                          Guid? parentId,
                          string? mimeType,
                          bool? hasChild,
                          string? localPath,
                          string? cdn,
                          string? product,
                          int? status,
                          string? workspace,
                          string? content,
                          string? tenant,
                          Guid? updatedBy,
                          DateTime? updatedDate)
    {
        Id = id;
        Name = name;
        Title = title;
        Description = description;
        Size = size;
        IsFile = isFile;
        ParentId = parentId;
        MimeType = mimeType;
        HasChild = hasChild;
        LocalPath = localPath;
        Cdn = cdn;
        Product = product;
        Status = status;
        Workspace = workspace;
        Content = content;
        Tenant = tenant;
        Status = status;
        UpdatedBy = updatedBy;
        UpdatedDate = updatedDate;
    }
    public bool IsValid(IItemRepository _context)
    {
        ValidationResult = new FolderEditCommandValidation(_context).Validate(this);
        return ValidationResult.IsValid;
    }
}
