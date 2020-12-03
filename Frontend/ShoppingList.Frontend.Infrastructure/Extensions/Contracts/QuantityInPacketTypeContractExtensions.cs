﻿using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ShoppingList.Frontend.Models;

namespace ShoppingList.Frontend.Infrastructure.Extensions.Contracts
{
    public static class QuantityInPacketTypeContractExtensions
    {
        public static QuantityInPacketType ToModel(this QuantityInPacketTypeContract contract)
        {
            return new QuantityInPacketType(contract.Id, contract.Name, contract.QuantityLabel);
        }
    }
}