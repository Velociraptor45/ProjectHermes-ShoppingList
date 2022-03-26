﻿using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Queries.AllActiveStores;

public class AllActiveStoresQuery : IQuery<IEnumerable<StoreReadModel>>
{
}