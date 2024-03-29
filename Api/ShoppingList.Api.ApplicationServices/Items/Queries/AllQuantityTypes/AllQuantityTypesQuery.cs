﻿using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries.Quantities;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.AllQuantityTypes;

public class AllQuantityTypesQuery : IQuery<IEnumerable<QuantityTypeReadModel>>
{
}