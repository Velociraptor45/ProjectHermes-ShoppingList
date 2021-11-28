﻿using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToDomain.StoreItems
{
    public class CreateItemTypeConverter : IToDomainConverter<CreateItemTypeContract, IItemType>
    {
        private readonly IItemTypeFactory _itemTypeFactory;
        private readonly IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> _itemAvailabilityConverter;

        public CreateItemTypeConverter(IItemTypeFactory itemTypeFactory,
            IToDomainConverter<ItemAvailabilityContract, IStoreItemAvailability> itemAvailabilityConverter)
        {
            _itemTypeFactory = itemTypeFactory;
            _itemAvailabilityConverter = itemAvailabilityConverter;
        }

        public IItemType ToDomain(CreateItemTypeContract source)
        {
            return _itemTypeFactory.Create(
                new ItemTypeId(0),
                source.Name,
                _itemAvailabilityConverter.ToDomain(source.Availabilities));
        }
    }
}