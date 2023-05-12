using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.RecipeTags.Models;

public class RecipeTagBuilder : DomainTestBuilderBase<RecipeTag>
{
    public RecipeTagBuilder WithId(RecipeTagId id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public RecipeTagBuilder WithName(RecipeTagName name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }
}