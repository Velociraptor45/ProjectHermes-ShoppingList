using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Converters.ToContract;
using Section = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Section;
using Store = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Store;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Stores.Converters.ToContract;

public class StoreConverterTests : ToContractConverterTestBase<IStore, Store, StoreConverter>
{
    protected override StoreConverter CreateSut()
    {
        return new StoreConverter(new SectionConverter());
    }

    protected override void AddMapping(IMappingExpression<IStore, Store> mapping)
    {
        mapping
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => src.IsDeleted))
            .ForMember(dest => dest.Sections, opt => opt.MapFrom((src, _, _, ctx) =>
                src.Sections.Select(s => ctx.Mapper.Map<Section>((src.Id, s)))))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => ((AggregateRoot)src).RowVersion));
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        new SectionConverterTests().AddMapping(cfg);
    }
}