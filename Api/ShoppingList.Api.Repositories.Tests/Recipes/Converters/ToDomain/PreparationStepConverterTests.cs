using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.Recipes.Entities;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;
using PreparationStep = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.PreparationStep;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Recipes.Converters.ToDomain;

public class PreparationStepConverterTests
    : ToDomainConverterTestBase<PreparationStep, IPreparationStep, PreparationStepConverter>
{
    public override PreparationStepConverter CreateSut()
    {
        return new(new PreparationStepFactory());
    }

    protected override PreparationStep CreateSource()
    {
        return new PreparationStepEntityBuilder().Create();
    }

    protected override void AddMapping(IMappingExpression<PreparationStep, IPreparationStep> mapping)
    {
        mapping.As<Domain.Recipes.Models.PreparationStep>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<PreparationStep, Domain.Recipes.Models.PreparationStep>()
            .ForCtorParam(nameof(IPreparationStep.Id).LowerFirstChar(), opt => opt.MapFrom(src => new PreparationStepId(src.Id)))
            .ForCtorParam(nameof(IPreparationStep.Instruction).LowerFirstChar(), opt => opt.MapFrom(src => new PreparationStepInstruction(src.Instruction)));
    }
}