﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem
{
    public class CreateItemCommandHandler : ICommandHandler<CreateItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IStoreItemFactory storeItemFactory;
        private readonly IStoreItemAvailabilityFactory storeItemAvailabilityFactory;
        private readonly IStoreRepository storeRepository;
        private readonly IStoreItemSectionReadRepository storeItemSectionReadRepository;

        public CreateItemCommandHandler(IItemRepository itemRepository, IManufacturerRepository manufacturerRepository,
            IItemCategoryRepository itemCategoryRepository, IStoreItemFactory storeItemFactory,
            IStoreItemAvailabilityFactory storeItemAvailabilityFactory, IStoreRepository storeRepository,
            IStoreItemSectionReadRepository storeItemSectionReadRepository)
        {
            this.itemRepository = itemRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.storeItemFactory = storeItemFactory;
            this.storeItemAvailabilityFactory = storeItemAvailabilityFactory;
            this.storeRepository = storeRepository;
            this.storeItemSectionReadRepository = storeItemSectionReadRepository;
        }

        public async Task<bool> HandleAsync(CreateItemCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            IItemCategory itemCategory = await itemCategoryRepository
                .FindByAsync(command.ItemCreation.ItemCategoryId, cancellationToken);
            if (itemCategory == null)
                throw new DomainException(new ItemCategoryNotFoundReason(command.ItemCreation.ItemCategoryId));

            IManufacturer manufacturer = null;
            if (command.ItemCreation.ManufacturerId != null)
            {
                manufacturer = await manufacturerRepository
                    .FindByAsync(command.ItemCreation.ManufacturerId, cancellationToken);
                if (manufacturer == null)
                    throw new DomainException(new ManufacturerNotFoundReason(command.ItemCreation.ManufacturerId));
            }

            cancellationToken.ThrowIfCancellationRequested();

            IEnumerable<IStoreItemAvailability> availabilities =
                await GetStoreItemAvailabilities(command.ItemCreation.Availabilities, cancellationToken);
            var storeItem = storeItemFactory.Create(command.ItemCreation, itemCategory, manufacturer, availabilities);

            await itemRepository.StoreAsync(storeItem, cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return true;
        }

        private async Task<IEnumerable<IStoreItemAvailability>> GetStoreItemAvailabilities(
            IEnumerable<ShortAvailability> shortAvailabilities, CancellationToken cancellationToken)
        {
            var sectionIds = shortAvailabilities.Select(av => av.StoreItemSectionId);
            var sections = (await storeItemSectionReadRepository.FindByAsync(sectionIds, cancellationToken))
                .ToLookup(s => s.Id);

            var availabilities = new List<IStoreItemAvailability>();
            foreach (var shortAvailability in shortAvailabilities)
            {
                if (!sections.Contains(shortAvailability.StoreItemSectionId))
                    throw new DomainException(new StoreItemSectionNotFoundReason(shortAvailability.StoreItemSectionId));

                StoreId storeId = shortAvailability.StoreId.AsStoreId();
                var store = await storeRepository.FindActiveByAsync(storeId, cancellationToken);
                if (store == null)
                    throw new DomainException(new StoreNotFoundReason(storeId));

                var availability = storeItemAvailabilityFactory
                    .Create(store, shortAvailability.Price, shortAvailability.StoreItemSectionId);
                availabilities.Add(availability);
            }

            return availabilities;
        }
    }
}