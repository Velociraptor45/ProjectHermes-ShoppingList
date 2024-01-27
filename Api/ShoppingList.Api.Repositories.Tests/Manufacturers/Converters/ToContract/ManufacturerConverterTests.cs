using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Converters.ToContract;
using Manufacturer = ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Entities.Manufacturer;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Manufacturers.Converters.ToContract;

public class ManufacturerConverterTests : ToContractConverterTestBase<IManufacturer, Manufacturer, ManufacturerConverter>
{
    protected override ManufacturerConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<IManufacturer, Manufacturer> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
            .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => ((AggregateRoot)src).RowVersion));
    }
}