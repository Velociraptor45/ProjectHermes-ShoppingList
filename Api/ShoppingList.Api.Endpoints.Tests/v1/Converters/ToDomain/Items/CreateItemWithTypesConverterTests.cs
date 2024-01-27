using AutoMapper;
using FluentAssertions.Equivalency;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Items;
using System.Text.RegularExpressions;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Converters.ToDomain.Items;

public class CreateItemWithTypesConverterTests
{
    public class WithQuantityTypeUnit : CreateItemWithTypesConverterTestsBase
    {
        protected override CreateItemWithTypesContract CreateSource()
        {
            return new TestBuilder<CreateItemWithTypesContract>()
                .FillPropertyWith(x => x.QuantityTypeInPacket, new DomainTestBuilder<QuantityTypeInPacket>().Create().ToInt())
                .FillPropertyWith(x => x.QuantityType, QuantityType.Unit.ToInt())
                .Create();
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CreateItemWithTypesContract, Item>()
                .ConstructUsing((src, ctx) => new Item(
                    ItemId.New,
                    new ItemName(src.Name),
                    false,
                    new Comment(src.Comment),
                    new ItemQuantity(QuantityType.Unit, new ItemQuantityInPacket(
                        new Quantity(src.QuantityInPacket!.Value),
                        src.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>())),
                    new ItemCategoryId(src.ItemCategoryId),
                    new ManufacturerId(src.ManufacturerId!.Value),
                    new ItemTypes(src.ItemTypes.Select(ctx.Mapper.Map<CreateItemTypeContract, IItemType>), new ItemTypeFactory()),
                    null,
                    null))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(_ => (DateTimeOffset?)null))
                .ForMember(dest => dest.IsTemporary, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.ItemQuantity, opt => opt.Ignore())
                .ForMember(dest => dest.Availabilities, opt => opt.Ignore())
                .ForMember(dest => dest.ItemTypes, opt => opt.Ignore())
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
                .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

            new CreateItemTypeConverterTests().AddMapping(cfg);
        }
    }

    public class WithQuantityTypeWeight : CreateItemWithTypesConverterTestsBase
    {
        protected override CreateItemWithTypesContract CreateSource()
        {
            return new TestBuilder<CreateItemWithTypesContract>()
                .FillPropertyWith(c => c.QuantityTypeInPacket, null)
                .FillPropertyWith(c => c.QuantityInPacket, null)
                .FillPropertyWith(c => c.QuantityType, QuantityTypeInPacket.Weight.ToInt())
                .Create();
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CreateItemWithTypesContract, Item>()
                .ConstructUsing((src, ctx) => new Item(
                    ItemId.New,
                    new ItemName(src.Name),
                    false,
                    new Comment(src.Comment),
                    new ItemQuantity(QuantityType.Weight, null),
                    new ItemCategoryId(src.ItemCategoryId),
                    new ManufacturerId(src.ManufacturerId!.Value),
                    new ItemTypes(src.ItemTypes.Select(ctx.Mapper.Map<CreateItemTypeContract, IItemType>), new ItemTypeFactory()),
                    null,
                    null))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(_ => (DateTimeOffset?)null))
                .ForMember(dest => dest.IsTemporary, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.ItemQuantity, opt => opt.Ignore())
                .ForMember(dest => dest.Availabilities, opt => opt.Ignore())
                .ForMember(dest => dest.ItemTypes, opt => opt.Ignore())
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
                .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

            new CreateItemTypeConverterTests().AddMapping(cfg);
        }
    }

    public class WithManufacturerIdNull : CreateItemWithTypesConverterTestsBase
    {
        protected override CreateItemWithTypesContract CreateSource()
        {
            return new TestBuilder<CreateItemWithTypesContract>()
                .FillPropertyWith(c => c.QuantityTypeInPacket, null)
                .FillPropertyWith(c => c.QuantityInPacket, null)
                .FillPropertyWith(c => c.QuantityType, QuantityTypeInPacket.Weight.ToInt())
                .Create();
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CreateItemWithTypesContract, Item>()
                .ConstructUsing((src, ctx) => new Item(
                    ItemId.New,
                    new ItemName(src.Name),
                    false,
                    new Comment(src.Comment),
                    new ItemQuantity(QuantityType.Weight, null),
                    new ItemCategoryId(src.ItemCategoryId),
                    null,
                    new ItemTypes(src.ItemTypes.Select(ctx.Mapper.Map<CreateItemTypeContract, IItemType>), new ItemTypeFactory()),
                    null,
                    null))
                .ForMember(dest => dest.UpdatedOn, opt => opt.MapFrom(_ => (DateTimeOffset?)null))
                .ForMember(dest => dest.IsTemporary, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.ItemQuantity, opt => opt.Ignore())
                .ForMember(dest => dest.Availabilities, opt => opt.Ignore())
                .ForMember(dest => dest.ItemTypes, opt => opt.Ignore())
                .ForMember(dest => dest.RowVersion, opt => opt.Ignore())
                .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

            new CreateItemTypeConverterTests().AddMapping(cfg);
        }
    }

    public abstract class CreateItemWithTypesConverterTestsBase
        : ToDomainConverterTestBase<CreateItemWithTypesContract, IItem, CreateItemWithTypesConverter>
    {
        public override CreateItemWithTypesConverter CreateSut()
        {
            return new(new ItemFactory(new ItemTypeFactory()),
                new CreateItemTypeConverter(new ItemTypeFactory(), new ItemAvailabilityConverter()));
        }

        protected override void AddMapping(IMappingExpression<CreateItemWithTypesContract, IItem> mapping)
        {
            mapping.As<Item>();
        }

        protected override void CustomizeAssertionOptions(EquivalencyAssertionOptions<IItem> opt)
        {
            opt
                .Excluding(x => x.Id)
                .Excluding(info => new Regex(@"^ItemTypes\[\d+\]\.Id$").IsMatch(info.Path));
        }
    }
}