﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.AllQuantityTypesInPacket;

public class AllQuantityTypesInPacketQuery : IQuery<IEnumerable<QuantityTypeInPacketReadModel>>
{
}