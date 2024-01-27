using AutoMapper;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.TemporaryItems;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Items;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Converters.ToDomain.Items;

public class MakeTemporaryItemPermanentCommandConverterTests
{
    public class WithQuantityWeight : MakeTemporaryItemPermanentCommandConverterTestsBase
    {
        protected override (Guid id, MakeTemporaryItemPermanentContract contract) CreateSource()
        {
            var contract = new TestBuilder<MakeTemporaryItemPermanentContract>()
                .FillPropertyWith(x => x.QuantityInPacket, null)
                .FillPropertyWith(x => x.QuantityTypeInPacket, null)
                .FillPropertyWith(x => x.QuantityType, QuantityType.Weight.ToInt())
                .Create();
            return (Guid.NewGuid(), contract);
        }

        protected override void AddMapping(IMappingExpression<(Guid id, MakeTemporaryItemPermanentContract contract),
            MakeTemporaryItemPermanentCommand> mapping)
        {
            mapping
                .ForCtorParam(nameof(MakeTemporaryItemPermanentCommand.PermanentItem).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) => new PermanentItem(
                        new ItemId(src.id),
                        new ItemName(src.contract.Name),
                        new Comment(src.contract.Comment),
                        new ItemQuantity(QuantityType.Weight, null),
                        new ItemCategoryId(src.contract.ItemCategoryId),
                        new ManufacturerId(src.contract.ManufacturerId!.Value),
                        src.contract.Availabilities.Select(
                            ctx.Mapper.Map<ItemAvailabilityContract, ItemAvailability>))));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public class WithQuantityUnit : MakeTemporaryItemPermanentCommandConverterTestsBase
    {
        protected override (Guid id, MakeTemporaryItemPermanentContract contract) CreateSource()
        {
            var contract = new TestBuilder<MakeTemporaryItemPermanentContract>()
                .FillPropertyWith(x => x.QuantityTypeInPacket, new DomainTestBuilder<QuantityTypeInPacket>().Create().ToInt())
                .FillPropertyWith(x => x.QuantityType, QuantityType.Unit.ToInt())
                .Create();
            return (Guid.NewGuid(), contract);
        }

        protected override void AddMapping(IMappingExpression<(Guid id, MakeTemporaryItemPermanentContract contract),
            MakeTemporaryItemPermanentCommand> mapping)
        {
            mapping
                .ForCtorParam(nameof(MakeTemporaryItemPermanentCommand.PermanentItem).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) => new PermanentItem(
                        new ItemId(src.id),
                        new ItemName(src.contract.Name),
                        new Comment(src.contract.Comment),
                        new ItemQuantity(QuantityType.Unit, new ItemQuantityInPacket(
                            new Quantity(src.contract.QuantityInPacket!.Value),
                            src.contract.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>())),
                        new ItemCategoryId(src.contract.ItemCategoryId),
                        new ManufacturerId(src.contract.ManufacturerId!.Value),
                        src.contract.Availabilities.Select(
                            ctx.Mapper.Map<ItemAvailabilityContract, ItemAvailability>))));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public class WithManufacturerNull : MakeTemporaryItemPermanentCommandConverterTestsBase
    {
        protected override (Guid id, MakeTemporaryItemPermanentContract contract) CreateSource()
        {
            var contract = new TestBuilder<MakeTemporaryItemPermanentContract>()
                .FillPropertyWith(x => x.QuantityTypeInPacket, new DomainTestBuilder<QuantityTypeInPacket>().Create().ToInt())
                .FillPropertyWith(x => x.QuantityType, QuantityType.Unit.ToInt())
                .FillPropertyWith(x => x.ManufacturerId, null)
                .Create();
            return (Guid.NewGuid(), contract);
        }

        protected override void AddMapping(IMappingExpression<(Guid id, MakeTemporaryItemPermanentContract contract),
            MakeTemporaryItemPermanentCommand> mapping)
        {
            mapping
                .ForCtorParam(nameof(MakeTemporaryItemPermanentCommand.PermanentItem).LowerFirstChar(),
                    opt => opt.MapFrom((src, ctx) => new PermanentItem(
                        new ItemId(src.id),
                        new ItemName(src.contract.Name),
                        new Comment(src.contract.Comment),
                        new ItemQuantity(QuantityType.Unit, new ItemQuantityInPacket(
                            new Quantity(src.contract.QuantityInPacket!.Value),
                            src.contract.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>())),
                        new ItemCategoryId(src.contract.ItemCategoryId),
                        null,
                        src.contract.Availabilities.Select(
                            ctx.Mapper.Map<ItemAvailabilityContract, ItemAvailability>))));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public abstract class MakeTemporaryItemPermanentCommandConverterTestsBase :
        ToDomainConverterTestBase<(Guid id, MakeTemporaryItemPermanentContract contract),
            MakeTemporaryItemPermanentCommand, MakeTemporaryItemPermanentCommandConverter>
    {
        public override MakeTemporaryItemPermanentCommandConverter CreateSut()
        {
            return new(new ItemAvailabilityConverter());
        }
    }
}