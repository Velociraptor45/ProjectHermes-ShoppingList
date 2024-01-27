using AutoMapper;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.SpecimenBuilders;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Items;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Converters.ToDomain.Items;

public class ItemCreationConverterTests
{
    public class WithQuantityWeight : ItemCreationConverterTestsBase
    {
        protected override CreateItemContract CreateSource()
        {
            return new TestBuilder<CreateItemContract>()
                .FillPropertyWith(x => x.QuantityInPacket, null)
                .FillPropertyWith(x => x.QuantityTypeInPacket, null)
                .FillPropertyWith(x => x.QuantityType, QuantityType.Weight.ToInt())
                .Create();
        }

        protected override void AddMapping(IMappingExpression<CreateItemContract, ItemCreation> mapping)
        {
            mapping
                .ForCtorParam(nameof(ItemCreation.Name).LowerFirstChar(), opt => opt.MapFrom(src => new ItemName(src.Name)))
                .ForCtorParam(nameof(ItemCreation.Comment).LowerFirstChar(), opt => opt.MapFrom(src => new Comment(src.Comment)))
                .ForCtorParam(nameof(ItemCreation.ItemQuantity).LowerFirstChar(), opt => opt
                    .MapFrom(src => new ItemQuantity(QuantityType.Weight, null)))
                .ForCtorParam(nameof(ItemCreation.ItemCategoryId).LowerFirstChar(),
                    opt => opt.MapFrom(src => new ItemCategoryId(src.ItemCategoryId)))
                .ForCtorParam(nameof(ItemCreation.ManufacturerId).LowerFirstChar(),
                    opt => opt.MapFrom(src => new ManufacturerId(src.ManufacturerId!.Value)))
                .ForCtorParam(nameof(ItemCreation.Availabilities).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) => src.Availabilities.Select(
                        ctx.Mapper.Map<ItemAvailabilityContract, ItemAvailability>)));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public class WithQuantityUnit : ItemCreationConverterTestsBase
    {
        protected override CreateItemContract CreateSource()
        {
            var fixture = new TestBuilder<CreateItemContract>();
            fixture.Customizations.Add(new EnumSpecimenBuilder<QuantityTypeInPacket>());

            fixture
                .FillPropertyWith(x => x.QuantityTypeInPacket, new DomainTestBuilder<QuantityTypeInPacket>().Create().ToInt())
                .FillPropertyWith(x => x.QuantityType, QuantityType.Unit.ToInt())
                .Create();
            return fixture.Create();
        }

        protected override void AddMapping(IMappingExpression<CreateItemContract, ItemCreation> mapping)
        {
            mapping
                .ForCtorParam(nameof(ItemCreation.Name).LowerFirstChar(), opt => opt.MapFrom(src => new ItemName(src.Name)))
                .ForCtorParam(nameof(ItemCreation.Comment).LowerFirstChar(), opt => opt.MapFrom(src => new Comment(src.Comment)))
                .ForCtorParam(nameof(ItemCreation.ItemQuantity).LowerFirstChar(), opt => opt
                    .MapFrom(src => new ItemQuantity(QuantityType.Unit,
                        new ItemQuantityInPacket(
                            new Quantity(src.QuantityInPacket!.Value),
                            src.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>()))))
                .ForCtorParam(nameof(ItemCreation.ItemCategoryId).LowerFirstChar(),
                    opt => opt.MapFrom(src => new ItemCategoryId(src.ItemCategoryId)))
                .ForCtorParam(nameof(ItemCreation.ManufacturerId).LowerFirstChar(),
                    opt => opt.MapFrom(src => new ManufacturerId(src.ManufacturerId!.Value)))
                .ForCtorParam(nameof(ItemCreation.Availabilities).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) => src.Availabilities.Select(
                        ctx.Mapper.Map<ItemAvailabilityContract, ItemAvailability>)));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public class WithManufacturerNull : ItemCreationConverterTestsBase
    {
        protected override CreateItemContract CreateSource()
        {
            var fixture = new TestBuilder<CreateItemContract>();
            fixture.Customizations.Add(new EnumSpecimenBuilder<QuantityTypeInPacket>());

            fixture
                .FillPropertyWith(x => x.ManufacturerId, null)
                .FillPropertyWith(x => x.QuantityInPacket, null)
                .FillPropertyWith(x => x.QuantityTypeInPacket, null)
                .FillPropertyWith(x => x.QuantityType, QuantityType.Weight.ToInt())
                .Create();
            return fixture.Create();
        }

        protected override void AddMapping(IMappingExpression<CreateItemContract, ItemCreation> mapping)
        {
            mapping
                .ForCtorParam(nameof(ItemCreation.Name).LowerFirstChar(), opt => opt.MapFrom(src => new ItemName(src.Name)))
                .ForCtorParam(nameof(ItemCreation.Comment).LowerFirstChar(), opt => opt.MapFrom(src => new Comment(src.Comment)))
                .ForCtorParam(nameof(ItemCreation.ItemQuantity).LowerFirstChar(), opt => opt
                    .MapFrom(src => new ItemQuantity(QuantityType.Weight, null)))
                .ForCtorParam(nameof(ItemCreation.ItemCategoryId).LowerFirstChar(),
                    opt => opt.MapFrom(src => new ItemCategoryId(src.ItemCategoryId)))
                .ForCtorParam(nameof(ItemCreation.ManufacturerId).LowerFirstChar(),
                    opt => opt.MapFrom(src => (ManufacturerId?)null))
                .ForCtorParam(nameof(ItemCreation.Availabilities).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) => src.Availabilities.Select(
                        ctx.Mapper.Map<ItemAvailabilityContract, ItemAvailability>)));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public abstract class ItemCreationConverterTestsBase :
        ToDomainConverterTestBase<CreateItemContract, ItemCreation, ItemCreationConverter>
    {
        public override ItemCreationConverter CreateSut()
        {
            return new(new ItemAvailabilityConverter());
        }
    }
}