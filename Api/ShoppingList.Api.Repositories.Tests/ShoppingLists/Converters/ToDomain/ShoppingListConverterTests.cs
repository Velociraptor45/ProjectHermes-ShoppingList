using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Tests.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Converters.ToDomain;
using ProjectHermes.ShoppingList.Api.Repositories.TestKit.ShoppingLists.Entities;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.ShoppingLists.Converters.ToDomain;

public class ShoppingListConverterTests
    : ToDomainConverterTestBase<Repositories.ShoppingLists.Entities.ShoppingList, IShoppingList, ShoppingListConverter>
{
    public override ShoppingListConverter CreateSut()
    {
        return new(new ShoppingListFactory(new ShoppingListSectionFactory()), new ShoppingListSectionFactory(),
            new ShoppingListItemConverter());
    }

    protected override Repositories.ShoppingLists.Entities.ShoppingList CreateSource()
    {
        return new ShoppingListEntityBuilder().Create();
    }

    protected override void AddMapping(IMappingExpression<Repositories.ShoppingLists.Entities.ShoppingList, IShoppingList> mapping)
    {
        mapping.As<Domain.ShoppingLists.Models.ShoppingList>();
    }

    protected override void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
        cfg.CreateMap<Repositories.ShoppingLists.Entities.ShoppingList, Domain.ShoppingLists.Models.ShoppingList>()
            .ForCtorParam(nameof(IShoppingList.Id).LowerFirstChar(), opt => opt.MapFrom(src => new ShoppingListId(src.Id)))
            .ForCtorParam(nameof(IShoppingList.StoreId).LowerFirstChar(), opt => opt.MapFrom(src => new StoreId(src.StoreId)))
            .ForCtorParam(nameof(IShoppingList.CompletionDate).LowerFirstChar(), opt => opt.MapFrom(src => src.CompletionDate))
            .ForCtorParam(nameof(IShoppingList.Sections).LowerFirstChar(),
                opt => opt.MapFrom((src, ctx) => src.ItemsOnList.GroupBy(
                    map => map.SectionId,
                    map => map,
                    (sectionId, maps) => new
                    {
                        SectionId = sectionId,
                        Items = maps.Select(i => ctx.Mapper.Map<ShoppingListItem>(i))
                    })
                    .Select(g => new ShoppingListSection(
                        new SectionId(g.SectionId),
                        g.Items))))
            .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => src.RowVersion))
            .ForMember(dest => dest.Items, opt => opt.Ignore())
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

        new ShoppingListItemConverterTests().AddMapping(cfg);
    }
}