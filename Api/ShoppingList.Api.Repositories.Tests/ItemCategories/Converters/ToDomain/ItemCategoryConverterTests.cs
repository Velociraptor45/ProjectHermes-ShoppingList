using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models.Factories;
using ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;
using ItemCategory = ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Entities.ItemCategory;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.ItemCategories.Converters.ToDomain;

public class ItemCategoryConverterTests : ToDomainConverterTestBase<ItemCategory, IItemCategory, ItemCategoryConverter>
{
    public override ItemCategoryConverter CreateSut()
    {
        return new ItemCategoryConverter(new ItemCategoryFactory());
    }

    protected override void AddMapping(IMappingExpression<ItemCategory, IItemCategory> mapping)
    {
        mapping.As<Domain.ItemCategories.Models.ItemCategory>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<ItemCategory, Domain.ItemCategories.Models.ItemCategory>()
            .ForCtorParam(nameof(IItemCategory.Id).LowerFirstChar(), opt => opt.MapFrom(src => new ItemCategoryId(src.Id)))
            .ForCtorParam(nameof(IItemCategory.Name).LowerFirstChar(), opt => opt.MapFrom(src => new ItemCategoryName(src.Name)))
            .ForCtorParam(nameof(IItemCategory.IsDeleted).LowerFirstChar(), opt => opt.MapFrom(src => src.Deleted))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => src.RowVersion))
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());
    }
}