using VFi.Application.CMS.DTOs;
using VFi.Domain.CMS.Interfaces;
using VFi.NetDevPack.Queries;

namespace VFi.Application.CMS.Queries;

public class ItemProductQueryAll : IQuery<IEnumerable<ItemProductDto>>
{
    public ItemProductQueryAll()
    {
    }
}
public class ItemProductQueryHandler : IQueryHandler<ItemProductQueryAll, IEnumerable<ItemProductDto>>
{

    private readonly IItemProductRepository _itemProductRepository;
    public ItemProductQueryHandler(IItemProductRepository itemProductRepository)
    {
        _itemProductRepository = itemProductRepository;
    }
    public async Task<IEnumerable<ItemProductDto>> Handle(ItemProductQueryAll request, CancellationToken cancellationToken)
    {
        var items = await _itemProductRepository.GetAll();
        var result = items.Select(item => new ItemProductDto()
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Icon = item.Icon,
            Color = item.Color,
        });
        return result;
    }
}
