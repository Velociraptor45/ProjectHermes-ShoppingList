using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Items.Converters.ToDomain;

public class ItemAvailabilityConverterTests : ToDomainConverterTestBase<AvailableAt, ItemAvailability, ItemAvailabilityConverter>
{
    public override ItemAvailabilityConverter CreateSut()
    {
        return new ItemAvailabilityConverter();
    }

    protected override AvailableAt CreateSource()
    {
        return new AvailableAtEntityBuilder().Create();
    }

    protected override void AddMapping(IMappingExpression<AvailableAt, ItemAvailability> mapping)
    {
        mapping
            .ForCtorParam(nameof(AvailableAt.StoreId), opt => opt.MapFrom(src => new StoreId(src.StoreId)))
            .ForCtorParam(nameof(AvailableAt.Price), opt => opt.MapFrom(src => new Price(src.Price)))
            .ForCtorParam(nameof(AvailableAt.DefaultSectionId),
                opt => opt.MapFrom(src => new SectionId(src.DefaultSectionId)));
    }
}