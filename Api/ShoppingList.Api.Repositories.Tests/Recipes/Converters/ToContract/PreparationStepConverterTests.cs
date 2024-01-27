using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Converters.ToContract;
using PreparationStep = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.PreparationStep;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Recipes.Converters.ToContract;

public class PreparationStepConverterTests
    : ToContractConverterTestBase<(RecipeId, IPreparationStep), PreparationStep, PreparationStepConverter>
{
    protected override PreparationStepConverter CreateSut()
    {
        return new();
    }

    protected override void AddMapping(IMappingExpression<(RecipeId, IPreparationStep), PreparationStep> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Item2.Id))
            .ForMember(dest => dest.RecipeId, opt => opt.MapFrom(src => src.Item1))
            .ForMember(dest => dest.Instruction, opt => opt.MapFrom(src => src.Item2.Instruction.Value))
            .ForMember(dest => dest.SortingIndex, opt => opt.MapFrom(src => src.Item2.SortingIndex))
            .ForMember(dest => dest.Recipe, opt => opt.Ignore());
    }
}