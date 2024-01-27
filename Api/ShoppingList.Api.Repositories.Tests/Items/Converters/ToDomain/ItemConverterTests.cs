using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.Items.Entities;
using System;

using DomainModels = ProjectHermes.ShoppingList.Api.Domain.Items.Models;

using Item = ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.Item;
using ItemType = ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.ItemType;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Items.Converters.ToDomain;

public class ItemConverterTests
{
    public class WithItemTypes : ItemConverterTestsBase
    {
        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyAvailableAt().Create();
        }

        protected override void AddMapping(IMappingExpression<Item, IItem> mapping)
        {
            mapping
                .ConvertUsing((src, _, ctx) =>
                    new DomainModels.Item(
                        new ItemId(src.Id),
                        new ItemName(src.Name),
                        src.Deleted,
                        new Comment(src.Comment),
                        new ItemQuantity(src.QuantityType.ToEnum<QuantityType>(),
                            new ItemQuantityInPacket(
                                new Quantity(src.QuantityInPacket!.Value),
                                src.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>())),
                        new ItemCategoryId(src.ItemCategoryId!.Value),
                        new ManufacturerId(src.ManufacturerId!.Value),
                        new ItemTypes(src.ItemTypes.Select(ctx.Mapper.Map<ItemType, IItemType>), new ItemTypeFactory()),
                        src.UpdatedOn,
                        new ItemId(src.PredecessorId!.Value)));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemTypeConverterTests().AddMapping(cfg);
        }
    }

    public class WithoutItemCategoryId : ItemConverterTestsBase
    {
        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyItemTypes().WithoutItemCategoryId().Create();
        }

        protected override void AddMapping(IMappingExpression<Item, IItem> mapping)
        {
            mapping
                .ConvertUsing((src, _, ctx) =>
                    new DomainModels.Item(
                        new ItemId(src.Id),
                        new ItemName(src.Name),
                        src.Deleted,
                        new Comment(src.Comment),
                        src.IsTemporary,
                        new ItemQuantity(src.QuantityType.ToEnum<QuantityType>(),
                            new ItemQuantityInPacket(
                                new Quantity(src.QuantityInPacket!.Value),
                                src.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>())),
                        null,
                        new ManufacturerId(src.ManufacturerId!.Value),
                        src.AvailableAt.Select(ctx.Mapper.Map<AvailableAt, ItemAvailability>),
                        new TemporaryItemId(src.CreatedFrom!.Value),
                        src.UpdatedOn,
                        new ItemId(src.PredecessorId!.Value)));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public class WithoutManufacturerId : ItemConverterTestsBase
    {
        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyItemTypes().WithoutManufacturerId().Create();
        }

        protected override void AddMapping(IMappingExpression<Item, IItem> mapping)
        {
            mapping
                .ConvertUsing((src, _, ctx) =>
                    new DomainModels.Item(
                        new ItemId(src.Id),
                        new ItemName(src.Name),
                        src.Deleted,
                        new Comment(src.Comment),
                        src.IsTemporary,
                        new ItemQuantity(src.QuantityType.ToEnum<QuantityType>(),
                            new ItemQuantityInPacket(
                                new Quantity(src.QuantityInPacket!.Value),
                                src.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>())),
                        new ItemCategoryId(src.ItemCategoryId!.Value),
                        null,
                        src.AvailableAt.Select(ctx.Mapper.Map<AvailableAt, ItemAvailability>),
                        new TemporaryItemId(src.CreatedFrom!.Value),
                        src.UpdatedOn,
                        new ItemId(src.PredecessorId!.Value)));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public class WithoutItemTypes : ItemConverterTestsBase
    {
        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyItemTypes().Create();
        }

        protected override void AddMapping(IMappingExpression<Item, IItem> mapping)
        {
            mapping
                .ConvertUsing((src, _, ctx) =>
                    new DomainModels.Item(
                        new ItemId(src.Id),
                        new ItemName(src.Name),
                        src.Deleted,
                        new Comment(src.Comment),
                        src.IsTemporary,
                        new ItemQuantity(src.QuantityType.ToEnum<QuantityType>(),
                            new ItemQuantityInPacket(
                                new Quantity(src.QuantityInPacket!.Value),
                                src.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>())),
                        new ItemCategoryId(src.ItemCategoryId!.Value),
                        new ManufacturerId(src.ManufacturerId!.Value),
                        src.AvailableAt.Select(ctx.Mapper.Map<AvailableAt, ItemAvailability>),
                        new TemporaryItemId(src.CreatedFrom!.Value),
                        src.UpdatedOn,
                        new ItemId(src.PredecessorId!.Value)));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public class WithoutTemporaryItemId : ItemConverterTestsBase
    {
        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyItemTypes().WithoutCreatedFrom().Create();
        }

        protected override void AddMapping(IMappingExpression<Item, IItem> mapping)
        {
            mapping
                .ConvertUsing((src, _, ctx) =>
                    new DomainModels.Item(
                        new ItemId(src.Id),
                        new ItemName(src.Name),
                        src.Deleted,
                        new Comment(src.Comment),
                        src.IsTemporary,
                        new ItemQuantity(src.QuantityType.ToEnum<QuantityType>(),
                            new ItemQuantityInPacket(
                                new Quantity(src.QuantityInPacket!.Value),
                                src.QuantityTypeInPacket!.Value.ToEnum<QuantityTypeInPacket>())),
                        new ItemCategoryId(src.ItemCategoryId!.Value),
                        new ManufacturerId(src.ManufacturerId!.Value),
                        src.AvailableAt.Select(ctx.Mapper.Map<AvailableAt, ItemAvailability>),
                        null,
                        src.UpdatedOn,
                        new ItemId(src.PredecessorId!.Value)));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public class WithoutQuantityInPacket : ItemConverterTestsBase
    {
        protected override Item CreateSource()
        {
            return new ItemEntityBuilder()
                .WithEmptyItemTypes()
                .WithoutQuantityInPacket()
                .WithoutQuantityTypeInPacket()
                .WithQuantityType(QuantityType.Weight.ToInt())
                .Create();
        }

        protected override void AddMapping(IMappingExpression<Item, IItem> mapping)
        {
            mapping
                .ConvertUsing((src, _, ctx) =>
                    new DomainModels.Item(
                        new ItemId(src.Id),
                        new ItemName(src.Name),
                        src.Deleted,
                        new Comment(src.Comment),
                        src.IsTemporary,
                        new ItemQuantity(src.QuantityType.ToEnum<QuantityType>(), null),
                        new ItemCategoryId(src.ItemCategoryId!.Value),
                        new ManufacturerId(src.ManufacturerId!.Value),
                        src.AvailableAt.Select(ctx.Mapper.Map<AvailableAt, ItemAvailability>),
                        new TemporaryItemId(src.CreatedFrom!.Value),
                        src.UpdatedOn,
                        new ItemId(src.PredecessorId!.Value)));
        }

        protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
        {
            new ItemAvailabilityConverterTests().AddMapping(cfg);
        }
    }

    public class ItemTypesWithoutItemCategoryId : ItemFailedConverterTestsBase<InvalidOperationException>
    {
        protected override string ExpectedMessage => "ItemCategoryId mustn't be null for an item with types";

        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyAvailableAt().WithoutItemCategoryId().Create();
        }
    }

    public class QuantityInPacketButNoQuantityInPacketType : ItemFailedConverterTestsBase<InvalidOperationException>
    {
        protected override string ExpectedMessage => "Invalid data state for item *: QuantityInPacket isn't null but QuantityTypeInPacket is";

        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyAvailableAt().WithoutQuantityTypeInPacket().Create();
        }
    }

    public class QuantityInPacketTypeButNoQuantityInPacket : ItemFailedConverterTestsBase<InvalidOperationException>
    {
        protected override string ExpectedMessage => "Invalid data state for item *: QuantityInPacket is null but QuantityTypeInPacket isn't";

        protected override Item CreateSource()
        {
            return new ItemEntityBuilder().WithEmptyAvailableAt().WithoutQuantityInPacket().Create();
        }
    }

    public abstract class ItemConverterTestsBase : ToDomainConverterTestBase<Item, IItem, ItemConverter>
    {
        public override ItemConverter CreateSut()
        {
            return new ItemConverter(
                new ItemFactory(new ItemTypeFactory()),
                new ItemTypeConverterTests().CreateSut(),
                new ItemAvailabilityConverterTests().CreateSut());
        }
    }

    public abstract class ItemFailedConverterTestsBase<TException> : ToDomainFailedConverterTestBase<Item, IItem, ItemConverter, TException>
        where TException : Exception
    {
        public override ItemConverter CreateSut()
        {
            return new ItemConverter(
                new ItemFactory(new ItemTypeFactory()),
                new ItemTypeConverterTests().CreateSut(),
                new ItemAvailabilityConverterTests().CreateSut());
        }
    }
}