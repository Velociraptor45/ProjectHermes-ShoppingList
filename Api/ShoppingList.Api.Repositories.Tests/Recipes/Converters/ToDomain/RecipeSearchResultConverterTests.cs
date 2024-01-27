using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.Recipes.Entities;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;
using Recipe = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.Recipe;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Recipes.Converters.ToDomain;

public class RecipeSearchResultConverterTests
    : ToDomainConverterTestBase<Recipe, RecipeSearchResult, RecipeSearchResultConverter>
{
    public override RecipeSearchResultConverter CreateSut()
    {
        return new();
    }

    protected override Recipe CreateSource()
    {
        return new RecipeEntityBuilder().Create();
    }

    protected override void AddMapping(IMappingExpression<Recipe, RecipeSearchResult> mapping)
    {
        mapping
            .ForCtorParam(nameof(RecipeSearchResult.Id).LowerFirstChar(), opt => opt.MapFrom(src => new RecipeId(src.Id)))
            .ForCtorParam(nameof(RecipeSearchResult.Name).LowerFirstChar(), opt => opt.MapFrom(src => new RecipeName(src.Name)));
    }
}