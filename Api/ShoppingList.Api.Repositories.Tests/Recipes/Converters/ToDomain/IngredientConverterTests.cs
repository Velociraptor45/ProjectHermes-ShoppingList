using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Items.Services.Validation;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.Recipes.Entities;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;
using Ingredient = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.Ingredient;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Recipes.Converters.ToDomain;

public class IngredientConverterTests : ToDomainConverterTestBase<Ingredient, IIngredient, IngredientConverter>
{
    public override IngredientConverter CreateSut()
    {
        return new(_ => new IngredientFactory(new ValidatorMock(MockBehavior.Strict).Object));
    }

    protected override Ingredient CreateSource()
    {
        return new IngredientEntityBuilder().Create();
    }

    protected override void AddMapping(IMappingExpression<Ingredient, IIngredient> mapping)
    {
        mapping.As<Domain.Recipes.Models.Ingredient>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Ingredient, Domain.Recipes.Models.Ingredient>()
            .ForCtorParam(nameof(IIngredient.Id).LowerFirstChar(), opt => opt.MapFrom(src => new IngredientId(src.Id)))
            .ForCtorParam(nameof(IIngredient.ItemCategoryId).LowerFirstChar(), opt => opt.MapFrom(src => new ItemCategoryId(src.ItemCategoryId)))
            .ForCtorParam(nameof(IIngredient.QuantityType).LowerFirstChar(), opt => opt.MapFrom(src => src.QuantityType.ToEnum<IngredientQuantityType>()))
            .ForCtorParam(nameof(IIngredient.Quantity).LowerFirstChar(), opt => opt.MapFrom(src => new IngredientQuantity(src.Quantity)))
            .ForCtorParam(nameof(IIngredient.ShoppingListProperties).LowerFirstChar(),
                opt => opt.MapFrom(src => new IngredientShoppingListProperties(
                        new ItemId(src.DefaultItemId!.Value),
                        new ItemTypeId(src.DefaultItemTypeId!.Value),
                        new StoreId(src.DefaultStoreId!.Value),
                        src.AddToShoppingListByDefault!.Value)));
    }
}