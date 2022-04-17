﻿using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain
{
    public class QuantityTypeInPacketConverter :
        IToDomainConverter<QuantityTypeInPacketContract, QuantityTypeInPacket>
    {
        public QuantityTypeInPacket ToDomain(QuantityTypeInPacketContract source)
        {
            return new QuantityTypeInPacket(source.Id, source.Name, source.QuantityLabel);
        }
    }
}