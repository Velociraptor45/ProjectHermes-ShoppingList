﻿using ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems;

public class UpdateItemCommandConverter : IToDomainConverter<(Guid id, UpdateItemContract contract), UpdateItemCommand>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> _storeItemAvailabilityConverter;

    public UpdateItemCommandConverter(
        IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> storeItemAvailabilityConverter)
    {
        _storeItemAvailabilityConverter = storeItemAvailabilityConverter;
    }

    public UpdateItemCommand ToDomain((Guid id, UpdateItemContract contract) source)
    {
        var (id, contract) = source;
        ArgumentNullException.ThrowIfNull(contract);

        ItemQuantityInPacket? itemQuantityInPacket = null;
        //todo improve this check
        if (contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is not null)
        {
            itemQuantityInPacket = new ItemQuantityInPacket(
                new Quantity(contract.QuantityInPacket.Value),
                contract.QuantityTypeInPacket.Value.ToEnum<QuantityTypeInPacket>());
        }

        var update = new ItemUpdate(
            new ItemId(id),
            new ItemName(contract.Name),
            new Comment(contract.Comment),
            new ItemQuantity(
                contract.QuantityType.ToEnum<QuantityType>(),
                itemQuantityInPacket),
            new ItemCategoryId(contract.ItemCategoryId),
            contract.ManufacturerId.HasValue ?
                new ManufacturerId(contract.ManufacturerId.Value) :
                null,
            _storeItemAvailabilityConverter.ToDomain(contract.Availabilities));

        return new UpdateItemCommand(update);
    }
}