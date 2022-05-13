﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Deletions;

public class ManufacturerDeletionService : IManufacturerDeletionService
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IItemRepository _itemRepository;
    private readonly CancellationToken _cancellationToken;

    public ManufacturerDeletionService(
        IManufacturerRepository manufacturerRepository,
        IItemRepository itemRepository,
        CancellationToken cancellationToken)
    {
        _manufacturerRepository = manufacturerRepository;
        _itemRepository = itemRepository;
        _cancellationToken = cancellationToken;
    }

    public async Task DeleteAsync(ManufacturerId manufacturerId)
    {
        var manufacturer = await _manufacturerRepository.FindByAsync(manufacturerId, _cancellationToken);
        if (manufacturer == null)
            throw new DomainException(new ManufacturerNotFoundReason(manufacturerId));

        var items = await _itemRepository.FindByAsync(manufacturerId, _cancellationToken);

        foreach (var item in items)
        {
            item.RemoveManufacturer();
            await _itemRepository.StoreAsync(item, _cancellationToken);
        }

        manufacturer.Delete();

        await _manufacturerRepository.StoreAsync(manufacturer, _cancellationToken);
    }
}