using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Converters.ToContract;
using Section = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Stores.Converters.ToContract;

public class SectionConverterTests : ToContractConverterTestBase<(StoreId, ISection), Section, SectionConverter>
{
    protected override SectionConverter CreateSut()
    {
        return new SectionConverter();
    }

    protected override void AddMapping(IMappingExpression<(StoreId, ISection), Section> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Item2.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item2.Name))
            .ForMember(dest => dest.SortIndex, opt => opt.MapFrom(src => src.Item2.SortingIndex))
            .ForMember(dest => dest.IsDefaultSection, opt => opt.MapFrom(src => src.Item2.IsDefaultSection))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.Item2.IsDeleted))
            .ForMember(dest => dest.StoreId, opt => opt.MapFrom(src => src.Item1.Value))
            .ForMember(dest => dest.Store, opt => opt.Ignore());
    }
}