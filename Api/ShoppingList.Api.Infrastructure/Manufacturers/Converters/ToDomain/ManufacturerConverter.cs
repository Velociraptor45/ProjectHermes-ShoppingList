﻿using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Converters.ToDomain;

public class ManufacturerConverter : IToDomainConverter<Entities.Manufacturer, IManufacturer>
{
    private readonly IManufacturerFactory _manufacturerFactory;

    public ManufacturerConverter(IManufacturerFactory manufacturerFactory)
    {
        _manufacturerFactory = manufacturerFactory;
    }

    public IManufacturer ToDomain(Entities.Manufacturer source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return _manufacturerFactory.Create(
            new ManufacturerId(source.Id),
            source.Name,
            source.Deleted);
    }
}