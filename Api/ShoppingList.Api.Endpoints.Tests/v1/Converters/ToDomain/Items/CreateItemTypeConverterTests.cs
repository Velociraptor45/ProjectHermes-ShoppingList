using AutoMapper;
using FluentAssertions.Equivalency;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Items;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Converters.ToDomain.Items;

public class CreateItemTypeConverterTests
    : ToDomainConverterTestBase<CreateItemTypeContract, IItemType, CreateItemTypeConverter>
{
    public override CreateItemTypeConverter CreateSut()
    {
        return new(new ItemTypeFactory(), new ItemAvailabilityConverter());
    }

    protected override void AddMapping(IMappingExpression<CreateItemTypeContract, IItemType> mapping)
    {
        mapping.As<ItemType>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<CreateItemTypeContract, ItemType>()
            .ForCtorParam(nameof(ItemType.Id).LowerFirstChar(), opt => opt.MapFrom(_ => ItemTypeId.New))
            .ForCtorParam(nameof(ItemType.IsDeleted).LowerFirstChar(), opt => opt.MapFrom(_ => false))
            .ForCtorParam(nameof(ItemType.PredecessorId).LowerFirstChar(), opt => opt.MapFrom(_ => (ItemTypeId?)null))
            .ForCtorParam(nameof(ItemType.Name).LowerFirstChar(), opt => opt.MapFrom(src => new ItemTypeName(src.Name)))
            .ForCtorParam(nameof(ItemType.Availabilities).LowerFirstChar(), opt => opt.MapFrom((src, ctx) =>
                src.Availabilities.Select(ctx.Mapper.Map<ItemAvailabilityContract, ItemAvailability>)));

        new ItemAvailabilityConverterTests().AddMapping(cfg);
    }

    protected override void CustomizeAssertionOptions(EquivalencyAssertionOptions<IItemType> opt)
    {
        opt.Excluding(info => info.Path == "Id");
    }
}