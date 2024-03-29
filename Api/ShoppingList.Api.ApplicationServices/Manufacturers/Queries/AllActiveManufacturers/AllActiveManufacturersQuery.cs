﻿using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Queries.AllActiveManufacturers;

public class AllActiveManufacturersQuery : IQuery<IEnumerable<ManufacturerReadModel>>
{
}