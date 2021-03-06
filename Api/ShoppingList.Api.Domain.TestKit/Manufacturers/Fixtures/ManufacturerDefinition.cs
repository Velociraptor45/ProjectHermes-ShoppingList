﻿using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ShoppingList.Api.Domain.TestKit.Manufacturers.Fixtures
{
    public class ManufacturerDefinition
    {
        public ManufacturerId Id { get; set; }

        public static ManufacturerDefinition FromId(ManufacturerId id)
        {
            return new ManufacturerDefinition
            {
                Id = id
            };
        }
    }
}