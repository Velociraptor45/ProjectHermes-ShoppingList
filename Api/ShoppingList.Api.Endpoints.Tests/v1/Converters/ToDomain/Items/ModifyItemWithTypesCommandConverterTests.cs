using AutoMapper;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Items;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Converters.ToDomain.Items;

public class ModifyItemWithTypesCommandConverterTests
{
    public class WithQuantityWeight : ModifyItemWithTypesCommandConverterTestsBase
    {
        protected override (Guid id, ModifyItemWithTypesContract contract) CreateSource()
        {
            var contract = new TestBuilder<ModifyItemWithTypesContract>()
                .FillPropertyWith(x => x.QuantityInPacket, null)
                .FillPropertyWith(x => x.QuantityTypeInPacket, null)
                .FillPropertyWith(x => x.QuantityType, QuantityType.Weight.ToInt())
                .Create();
            return (Guid.NewGuid(), contract);
        }

        protected override void AddMapping(IMappingExpression<(Guid id, ModifyItemWithTypesContract contract),
            ModifyItemWithTypesCommand> mapping)
        {
            mapping
                .ForCtorParam(nameof(ModifyItemWithTypesCommand.ItemWithTypesModification).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) => new ItemWithTypesModification(
                        new ItemId(src.id),
                        new ItemName(src.contract.Name),
                        new Comment(src.contract.Comment),
                        new ItemQuantity(QuantityType.Weight, null),
                        new ItemCategoryId(src.contract.ItemCategoryId),
                        new ManufacturerId(src.contract.ManufacturerId!.Value),
                        src.contract.ItemTypes.Select(
                            ctx.Mapper.Map<ModifyItemTypeContract, ItemTypeModification>))));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ModifyItemTypeContract, ItemTypeModification>()
                .ForCtorParam(nameof(ItemTypeModification.Id).LowerFirstChar(),
                    opt => opt.MapFrom(src => new ItemTypeId(src.Id!.Value)))
                .ForCtorParam(nameof(ItemTypeModification.Name).LowerFirstChar(),
                    opt => opt.MapFrom(src => new ItemTypeName(src.Name)))
                .ForCtorParam(nameof(ItemTypeModification.Availabilities).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) =>
                        src.Availabilities.Select(ctx.Mapper.Map<ItemAvailabilityContract, ItemAvailability>)));

            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public class WithQuantityUnit : ModifyItemWithTypesCommandConverterTestsBase
    {
        protected override (Guid id, ModifyItemWithTypesContract contract) CreateSource()
        {
            var contract = new TestBuilder<ModifyItemWithTypesContract>()
                .FillPropertyWith(x => x.QuantityTypeInPacket, new DomainTestBuilder<QuantityTypeInPacket>().Create().ToInt())
                .FillPropertyWith(x => x.QuantityType, QuantityType.Unit.ToInt())
                .Create();
            return (Guid.NewGuid(), contract);
        }

        protected override void AddMapping(IMappingExpression<(Guid id, ModifyItemWithTypesContract contract),
            ModifyItemWithTypesCommand> mapping)
        {
            mapping
                .ForCtorParam(nameof(ModifyItemWithTypesCommand.ItemWithTypesModification).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) => new ItemWithTypesModification(
                        new ItemId(src.id),
                        new ItemName(src.contract.Name),
                        new Comment(src.contract.Comment),
                        new ItemQuantity(QuantityType.Unit, new ItemQuantityInPacket(
                            new Quantity(src.contract.QuantityInPacket!.Value),
                            src.contract.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>())),
                        new ItemCategoryId(src.contract.ItemCategoryId),
                        new ManufacturerId(src.contract.ManufacturerId!.Value),
                        src.contract.ItemTypes.Select(
                            ctx.Mapper.Map<ModifyItemTypeContract, ItemTypeModification>))));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ModifyItemTypeContract, ItemTypeModification>()
                .ForCtorParam(nameof(ItemTypeModification.Id).LowerFirstChar(),
                    opt => opt.MapFrom(src => new ItemTypeId(src.Id!.Value)))
                .ForCtorParam(nameof(ItemTypeModification.Name).LowerFirstChar(),
                    opt => opt.MapFrom(src => new ItemTypeName(src.Name)))
                .ForCtorParam(nameof(ItemTypeModification.Availabilities).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) =>
                        src.Availabilities.Select(ctx.Mapper.Map<ItemAvailabilityContract, ItemAvailability>)));

            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public class WithItemTypeIdNull : ModifyItemWithTypesCommandConverterTestsBase
    {
        protected override (Guid id, ModifyItemWithTypesContract contract) CreateSource()
        {
            var types = new DomainTestBuilder<ModifyItemTypeContract>()
                .FillPropertyWith(x => x.Id, null)
                .CreateMany(1)
                .ToList();

            var contract = new TestBuilder<ModifyItemWithTypesContract>()
                .FillPropertyWith(x => x.QuantityTypeInPacket, new DomainTestBuilder<QuantityTypeInPacket>().Create().ToInt())
                .FillPropertyWith(x => x.QuantityType, QuantityType.Unit.ToInt())
                .FillPropertyWith(x => x.ItemTypes, types)
                .Create();
            return (Guid.NewGuid(), contract);
        }

        protected override void AddMapping(IMappingExpression<(Guid id, ModifyItemWithTypesContract contract),
            ModifyItemWithTypesCommand> mapping)
        {
            mapping
                .ForCtorParam(nameof(ModifyItemWithTypesCommand.ItemWithTypesModification).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) => new ItemWithTypesModification(
                        new ItemId(src.id),
                        new ItemName(src.contract.Name),
                        new Comment(src.contract.Comment),
                        new ItemQuantity(QuantityType.Unit, new ItemQuantityInPacket(
                            new Quantity(src.contract.QuantityInPacket!.Value),
                            src.contract.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>())),
                        new ItemCategoryId(src.contract.ItemCategoryId),
                        new ManufacturerId(src.contract.ManufacturerId!.Value),
                        src.contract.ItemTypes.Select(
                            ctx.Mapper.Map<ModifyItemTypeContract, ItemTypeModification>))));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ModifyItemTypeContract, ItemTypeModification>()
                .ForCtorParam(nameof(ItemTypeModification.Id).LowerFirstChar(),
                    opt => opt.MapFrom(_ => (ItemTypeId?)null))
                .ForCtorParam(nameof(ItemTypeModification.Name).LowerFirstChar(),
                    opt => opt.MapFrom(src => new ItemTypeName(src.Name)))
                .ForCtorParam(nameof(ItemTypeModification.Availabilities).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) =>
                        src.Availabilities.Select(ctx.Mapper.Map<ItemAvailabilityContract, ItemAvailability>)));

            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public class WithManufacturerNull : ModifyItemWithTypesCommandConverterTestsBase
    {
        protected override (Guid id, ModifyItemWithTypesContract contract) CreateSource()
        {
            var contract = new TestBuilder<ModifyItemWithTypesContract>()
                .FillPropertyWith(x => x.QuantityTypeInPacket, new DomainTestBuilder<QuantityTypeInPacket>().Create().ToInt())
                .FillPropertyWith(x => x.QuantityType, QuantityType.Unit.ToInt())
                .FillPropertyWith(x => x.ManufacturerId, null)
                .Create();
            return (Guid.NewGuid(), contract);
        }

        protected override void AddMapping(IMappingExpression<(Guid id, ModifyItemWithTypesContract contract),
            ModifyItemWithTypesCommand> mapping)
        {
            mapping
                .ForCtorParam(nameof(ModifyItemWithTypesCommand.ItemWithTypesModification).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) => new ItemWithTypesModification(
                        new ItemId(src.id),
                        new ItemName(src.contract.Name),
                        new Comment(src.contract.Comment),
                        new ItemQuantity(QuantityType.Unit, new ItemQuantityInPacket(
                            new Quantity(src.contract.QuantityInPacket!.Value),
                            src.contract.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>())),
                        new ItemCategoryId(src.contract.ItemCategoryId),
                        null,
                        src.contract.ItemTypes.Select(
                            ctx.Mapper.Map<ModifyItemTypeContract, ItemTypeModification>))));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<ModifyItemTypeContract, ItemTypeModification>()
                .ForCtorParam(nameof(ItemTypeModification.Id).LowerFirstChar(),
                    opt => opt.MapFrom(src => new ItemTypeId(src.Id!.Value)))
                .ForCtorParam(nameof(ItemTypeModification.Name).LowerFirstChar(),
                    opt => opt.MapFrom(src => new ItemTypeName(src.Name)))
                .ForCtorParam(nameof(ItemTypeModification.Availabilities).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) =>
                        src.Availabilities.Select(ctx.Mapper.Map<ItemAvailabilityContract, ItemAvailability>)));

            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public abstract class ModifyItemWithTypesCommandConverterTestsBase :
        ToDomainConverterTestBase<(Guid id, ModifyItemWithTypesContract contract), ModifyItemWithTypesCommand, ModifyItemWithTypesCommandConverter>
    {
        public override ModifyItemWithTypesCommandConverter CreateSut()
        {
            return new(new ItemAvailabilityConverter());
        }
    }
}