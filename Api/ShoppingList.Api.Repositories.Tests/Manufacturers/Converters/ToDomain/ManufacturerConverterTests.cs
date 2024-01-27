using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;
using Manufacturer = ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Entities.Manufacturer;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Manufacturers.Converters.ToDomain;

public class ManufacturerConverterTests : ToDomainConverterTestBase<Manufacturer, IManufacturer, ManufacturerConverter>
{
    public override ManufacturerConverter CreateSut()
    {
        return new(new ManufacturerFactory());
    }

    protected override void AddMapping(IMappingExpression<Manufacturer, IManufacturer> mapping)
    {
        mapping.As<Domain.Manufacturers.Models.Manufacturer>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Manufacturer, Domain.Manufacturers.Models.Manufacturer>()
            .ForCtorParam(nameof(IManufacturer.Id).LowerFirstChar(), opt => opt.MapFrom(src => new ManufacturerId(src.Id)))
            .ForCtorParam(nameof(IManufacturer.Name).LowerFirstChar(), opt => opt.MapFrom(src => new ManufacturerName(src.Name)))
            .ForCtorParam(nameof(IManufacturer.IsDeleted).LowerFirstChar(), opt => opt.MapFrom(src => src.Deleted))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => src.RowVersion))
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());
    }
}