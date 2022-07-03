﻿using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ItemUpdateWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems;

public class UpdateItemWithTypesCommandConverter
    : IToDomainConverter<(Guid id, UpdateItemWithTypesContract contract), UpdateItemWithTypesCommand>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, IItemAvailability> _availabilityConverter;

    public UpdateItemWithTypesCommandConverter(
        IToDomainConverter<ItemAvailabilityContract, IItemAvailability> availabilityConverter)
    {
        _availabilityConverter = availabilityConverter;
    }

    public UpdateItemWithTypesCommand ToDomain((Guid id, UpdateItemWithTypesContract contract) source)
    {
        var (id, contract) = source;
        ArgumentNullException.ThrowIfNull(contract);

        var itemTypeUpdates = contract.ItemTypes.Select(t => new ItemTypeUpdate(
            new ItemTypeId(t.OldId),
            new ItemTypeName(t.Name),
            _availabilityConverter.ToDomain(t.Availabilities)));

        ItemQuantityInPacket? itemQuantityInPacket = null;
        //todo improve this check
        if (contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is not null)
        {
            itemQuantityInPacket = new ItemQuantityInPacket(
                new Quantity(contract.QuantityInPacket.Value),
                contract.QuantityTypeInPacket.Value.ToEnum<QuantityTypeInPacket>());
        }

        var itemUpdate = new ItemWithTypesUpdate(
            new ItemId(id),
            new ItemName(contract.Name),
            new Comment(contract.Comment),
            new ItemQuantity(
                contract.QuantityType.ToEnum<QuantityType>(),
                itemQuantityInPacket),
            new ItemCategoryId(contract.ItemCategoryId),
            contract.ManufacturerId.HasValue ? new ManufacturerId(contract.ManufacturerId.Value) : null,
            itemTypeUpdates);

        return new UpdateItemWithTypesCommand(itemUpdate);
    }
}