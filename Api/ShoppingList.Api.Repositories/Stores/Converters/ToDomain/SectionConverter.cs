﻿using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using Section = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.ShoppingList.Api.Repositories.Stores.Converters.ToDomain;

public class SectionConverter : IToDomainConverter<Entities.Section, ISection>
{
    private readonly ISectionFactory _sectionFactory;

    public SectionConverter(ISectionFactory sectionFactory)
    {
        _sectionFactory = sectionFactory;
    }

    public ISection ToDomain(Section source)
    {
        return _sectionFactory.Create(
            new SectionId(source.Id),
            new SectionName(source.Name),
            source.SortIndex,
            source.IsDefaultSection,
            source.IsDeleted);
    }
}