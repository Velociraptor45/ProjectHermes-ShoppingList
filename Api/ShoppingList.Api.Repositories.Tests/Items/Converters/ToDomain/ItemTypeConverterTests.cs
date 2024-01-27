using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.Items.Entities;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

using DomainModels = ProjectHermes.ShoppingList.Api.Domain.Items.Models;

using ItemType = ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.ItemType;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Items.Converters.ToDomain;

public class ItemTypeConverterTests : ToDomainConverterTestBase<ItemType, IItemType, ItemTypeConverter>
{
    public override ItemTypeConverter CreateSut()
    {
        return new ItemTypeConverter(
            new ItemTypeFactory(),
            new ItemTypeAvailabilityConverterTests().CreateSut());
    }

    protected override ItemType CreateSource()
    {
        return new ItemTypeEntityBuilder().Create();
    }

    protected override void AddMapping(IMappingExpression<ItemType, IItemType> mapping)
    {
        mapping.As<DomainModels.ItemType>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<ItemType, DomainModels.ItemType>()
            .ForCtorParam(nameof(DomainModels.ItemType.Id).LowerFirstChar(), opt => opt.MapFrom(src => new ItemTypeId(src.Id)))
            .ForCtorParam(nameof(DomainModels.ItemType.Name).LowerFirstChar(), opt => opt.MapFrom(src => new ItemTypeName(src.Name)))
            .ForCtorParam(nameof(DomainModels.ItemType.PredecessorId).LowerFirstChar(), opt => opt.MapFrom(src => new ItemTypeId(src.PredecessorId!.Value)))
            .ForCtorParam(nameof(DomainModels.ItemType.Availabilities).LowerFirstChar(), opt => opt.MapFrom(src => src.AvailableAt))
            .ForCtorParam(nameof(DomainModels.ItemType.IsDeleted).LowerFirstChar(), opt => opt.MapFrom(src => src.IsDeleted));

        new ItemTypeAvailabilityConverterTests().AddMapping(cfg);
    }
}