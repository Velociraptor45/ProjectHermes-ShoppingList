﻿using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.ModifyItem;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.ModifyItem;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.Items;

public class ModifyItemCommandConverter : IToDomainConverter<(Guid id, ModifyItemContract contract), ModifyItemCommand>
{
    private readonly IToDomainConverter<ItemAvailabilityContract, IItemAvailability> _itemAvailabilityConverter;

    public ModifyItemCommandConverter(
        IToDomainConverter<ItemAvailabilityContract, IItemAvailability> itemAvailabilityConverter)
    {
        _itemAvailabilityConverter = itemAvailabilityConverter;
    }

    public ModifyItemCommand ToDomain((Guid id, ModifyItemContract contract) source)
    {
        var (id, contract) = source;
        ArgumentNullException.ThrowIfNull(contract);

        ItemQuantityInPacket? itemQuantityInPacket = null;
        if (contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is not null)
        {
            itemQuantityInPacket = new ItemQuantityInPacket(
                new Quantity(contract.QuantityInPacket.Value),
                contract.QuantityTypeInPacket.Value.ToEnum<QuantityTypeInPacket>());
        }

        var modification = new ItemModification(
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
            _itemAvailabilityConverter.ToDomain(contract.Availabilities));

        return new ModifyItemCommand(modification);
    }
}