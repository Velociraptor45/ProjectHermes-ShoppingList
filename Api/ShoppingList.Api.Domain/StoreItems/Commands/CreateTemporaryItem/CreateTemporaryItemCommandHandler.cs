﻿using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;

public class CreateTemporaryItemCommandHandler : ICommandHandler<CreateTemporaryItemCommand, bool>
{
    private readonly IItemRepository _itemRepository;
    private readonly IStoreItemFactory _storeItemFactory;
    private readonly IAvailabilityValidationService _availabilityValidationService;

    public CreateTemporaryItemCommandHandler(IItemRepository itemRepository, IStoreItemFactory storeItemFactory,
        IAvailabilityValidationService availabilityValidationService)
    {
        _itemRepository = itemRepository;
        _storeItemFactory = storeItemFactory;
        _availabilityValidationService = availabilityValidationService;
    }

    public async Task<bool> HandleAsync(CreateTemporaryItemCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        var availability = command.TemporaryItemCreation.Availability;
        await _availabilityValidationService.ValidateAsync(availability.ToMonoList(), cancellationToken);

        var storeItem = _storeItemFactory.Create(command.TemporaryItemCreation);

        await _itemRepository.StoreAsync(storeItem, cancellationToken);
        return true;
    }
}