﻿using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Items;

public class CreateItemWithTypesConverter : IToDomainConverter<CreateItemWithTypesContract, IItem>
{
    private readonly IItemFactory _itemFactory;
    private readonly IToDomainConverter<CreateItemTypeContract, IItemType> _itemTypeConverter;

    public CreateItemWithTypesConverter(IItemFactory itemFactory,
        IToDomainConverter<CreateItemTypeContract, IItemType> itemTypeConverter)
    {
        _itemFactory = itemFactory;
        _itemTypeConverter = itemTypeConverter;
    }

    public IItem ToDomain(CreateItemWithTypesContract source)
    {
        ItemQuantityInPacket? itemQuantityInPacket = null;
        if (source.QuantityInPacket is not null && source.QuantityTypeInPacket is not null)
        {
            itemQuantityInPacket = new ItemQuantityInPacket(
                new Quantity(source.QuantityInPacket.Value),
                source.QuantityTypeInPacket.Value.ToEnum<QuantityTypeInPacket>());
        }

        return _itemFactory.CreateNew(
            new ItemName(source.Name),
            new Comment(source.Comment),
            new ItemQuantity(
                source.QuantityType.ToEnum<QuantityType>(),
                itemQuantityInPacket),
            new ItemCategoryId(source.ItemCategoryId),
            source.ManufacturerId.HasValue ? new ManufacturerId(source.ManufacturerId.Value) : null,
            null,
            _itemTypeConverter.ToDomain(source.ItemTypes));
    }
}