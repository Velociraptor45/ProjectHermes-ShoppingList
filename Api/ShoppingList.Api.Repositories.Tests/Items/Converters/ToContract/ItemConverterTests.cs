using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Converters.ToContract;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;
using Item = ProjectHermes.ShoppingList.Api.Domain.Items.Models.Item;
using ItemType = ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.ItemType;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Items.Converters.ToContract;

public class ItemConverterTests : ToContractConverterTestBase<Item, Repositories.Items.Entities.Item, ItemConverter>
{
    protected override ItemConverter CreateSut()
    {
        return new ItemConverter();
    }

    protected override void AddMapping(IMappingExpression<Item, Repositories.Items.Entities.Item> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment.Value))
            .ForMember(dest => dest.IsTemporary, opt => opt.MapFrom(src => src.IsTemporary))
            .ForMember(dest => dest.QuantityType, opt => opt.MapFrom(src => src.ItemQuantity.Type.ToInt()))
            .ForMember(dest => dest.QuantityInPacket, opt => opt.MapFrom(src => src.ItemQuantity.InPacket!.Quantity.Value))
            .ForMember(dest => dest.QuantityTypeInPacket, opt => opt.MapFrom(src => src.ItemQuantity.InPacket!.Type.ToInt()))
            .ForMember(dest => dest.ItemCategoryId, opt => opt.MapFrom(src => src.ItemCategoryId!.Value))
            .ForMember(dest => dest.ManufacturerId, opt => opt.MapFrom(src => src.ManufacturerId!.Value))
            .ForMember(dest => dest.CreatedFrom, opt => opt.MapFrom(src => src.TemporaryId))
            .ForMember(dest => dest.AvailableAt, opt => opt.MapFrom(src => CreateItemAvailabilityTuple(src)))
            .ForMember(dest => dest.ItemTypes, opt => opt.MapFrom(src => CreateItemTypeTuple(src)))
            .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(src => src.UpdatedOn))
            .ForMember(dest => dest.PredecessorId, opt => opt.MapFrom(src => src.PredecessorId!.Value))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => src.RowVersion))
            .ForMember(dest => dest.Predecessor, opt => opt.Ignore());
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<(Item, ItemAvailability), AvailableAt>(MemberList.Destination)
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Item1.Id.Value))
            .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.Item2.StoreId.Value))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Item2.Price.Value))
            .ForMember(dest => dest.DefaultSectionId, opt => opt.MapFrom(src => src.Item2.DefaultSectionId.Value))
            .ForMember(dest => dest.Item, opt => opt.Ignore());

        cfg.CreateMap<(Item, IItemType), ItemType>(MemberList.Destination)
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Item2.Id.Value))
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Item1.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item2.Name.Value))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.Item2.IsDeleted))
            .ForMember(dest => dest.PredecessorId, opt => opt.MapFrom(src => src.Item2.PredecessorId!.Value))
            .ForMember(dest => dest.AvailableAt, opt => opt.MapFrom(src => CreateItemTypeAvailabilityTuple(src.Item2)))
            .ForMember(dest => dest.Predecessor, opt => opt.Ignore())
            .ForMember(dest => dest.Item, opt => opt.Ignore());

        cfg.CreateMap<(IItemType, ItemAvailability), ItemTypeAvailableAt>(MemberList.Destination)
           .ForMember(dest => dest.ItemTypeId, opt => opt.MapFrom(src => src.Item1.Id.Value))
           .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.Item2.StoreId.Value))
           .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Item2.Price.Value))
           .ForMember(dest => dest.DefaultSectionId, opt => opt.MapFrom(src => src.Item2.DefaultSectionId.Value))
           .ForMember(dest => dest.ItemType, opt => opt.Ignore());
    }

    private static IEnumerable<(Item, IItemType)> CreateItemTypeTuple(Item item)
    {
        return item.ItemTypes.Select(it => (item, it));
    }

    private static IEnumerable<(Item, ItemAvailability)> CreateItemAvailabilityTuple(Item item)
    {
        return item.Availabilities.Select(av => (item, av));
    }

    private static IEnumerable<(IItemType, ItemAvailability)> CreateItemTypeAvailabilityTuple(IItemType itemType)
    {
        return itemType.Availabilities.Select(av => (itemType, av));
    }
}