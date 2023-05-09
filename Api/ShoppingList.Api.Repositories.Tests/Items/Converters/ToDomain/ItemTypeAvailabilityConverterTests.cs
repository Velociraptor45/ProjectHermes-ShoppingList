﻿using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Items.Converters.ToDomain;

public class ItemTypeAvailabilityConverterTests
    : ToDomainConverterBase<ItemTypeAvailableAt, IItemAvailability, ItemTypeAvailabilityConverter>
{
    public override ItemTypeAvailabilityConverter CreateSut()
    {
        return new ItemTypeAvailabilityConverter(new ItemAvailabilityFactory());
    }

    protected override ItemTypeAvailableAt CreateSource()
    {
        return new ItemTypeAvailableAtEntityBuilder().Create();
    }

    protected override void AddMapping(IMappingExpression<ItemTypeAvailableAt, IItemAvailability> mapping)
    {
        mapping.As<ItemAvailability>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<ItemTypeAvailableAt, ItemAvailability>()
            .ForCtorParam(nameof(AvailableAt.StoreId), opt => opt.MapFrom(src => new StoreId(src.StoreId)))
            .ForCtorParam(nameof(AvailableAt.Price), opt => opt.MapFrom(src => new Price(src.Price)))
            .ForCtorParam(nameof(AvailableAt.DefaultSectionId),
                opt => opt.MapFrom(src => new SectionId(src.DefaultSectionId)));
    }
}