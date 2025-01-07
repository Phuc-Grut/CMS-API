using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using MediatR;
using VFi.Domain.CMS.Interfaces;
using VFi.Domain.CMS.Models;
using VFi.NetDevPack.Messaging;

namespace VFi.Application.CMS.Commands.Handler;

internal class ItemCommandHandler : CommandHandler, IRequestHandler<ItemAddCommand, ValidationResult>,
                                                    IRequestHandler<FolderAddCommand, ValidationResult>,
                                                    IRequestHandler<FolderEditCommand, ValidationResult>,
                                                    IRequestHandler<ItemEditCommand, ValidationResult>,
                                                    IRequestHandler<ItemDeleteCommand, ValidationResult>
{
    private readonly IItemRepository _itemRepository;
    public ItemCommandHandler(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }
    public void Dispose()
    {
        _itemRepository.Dispose();
    }

    public async Task<ValidationResult> Handle(ItemDeleteCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_itemRepository))
            return request.ValidationResult;
        var item = new Item
        {
            Id = request.Id
        };
        _itemRepository.Remove(item);
        return await Commit(_itemRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ItemEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_itemRepository))
            return request.ValidationResult;
        var item = await _itemRepository.GetById(request.Id);
        item.Name = request.Name;
        item.Title = request.Title;
        item.Description = request.Description;
        item.UpdatedBy = request.UpdatedBy;
        item.UpdatedDate = request.UpdatedDate;
        _itemRepository.Update(item);
        return await Commit(_itemRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(ItemAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_itemRepository))
            return request.ValidationResult;
        var item = new Item
        {
            Id = request.Id,
            Name = request.Name,
            Title = request.Title,
            Description = request.Description,
            Size = request.Size,
            IsFile = request.IsFile,
            MimeType = request.MimeType,
            HasChild = request.HasChild,
            VirtualPath = request.VirtualPath,
            LocalPath = request.LocalPath,
            Cdn = request.Cdn,
            ParentId = request.ParentId,
            Status = request.Status,
            Product = request.Product,
            Workspace = request.Workspace,
            Content = request.Content,
            Tenant = request.Tenant,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
        };

        _itemRepository.Add(item);
        return await Commit(_itemRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(FolderAddCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_itemRepository))
            return request.ValidationResult;
        var item = new Item
        {
            Id = request.Id,
            Name = request.Name,
            Title = request.Title,
            Description = request.Description,
            Size = request.Size,
            IsFile = request.IsFile,
            MimeType = request.MimeType,
            HasChild = request.HasChild,
            VirtualPath = request.VirtualPath,
            LocalPath = request.LocalPath,
            Cdn = request.Cdn,
            ParentId = request.ParentId,
            Status = request.Status,
            Product = request.Product,
            Workspace = request.Workspace,
            Content = request.Content,
            Tenant = request.Tenant,
            CreatedBy = request.CreatedBy,
            CreatedDate = request.CreatedDate,
        };

        _itemRepository.Add(item);
        return await Commit(_itemRepository.UnitOfWork);
    }

    public async Task<ValidationResult> Handle(FolderEditCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid(_itemRepository))
            return request.ValidationResult;
        var item = await _itemRepository.GetById(request.Id);
        item.Id = request.Id;
        item.Title = request.Title;
        item.UpdatedBy = request.UpdatedBy;
        item.UpdatedDate = request.UpdatedDate;

        _itemRepository.Update(item);
        return await Commit(_itemRepository.UnitOfWork);
    }
}
