﻿namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

public class Store : IStore
{
    private StoreSections _sections;

    public Store(StoreId id, string name, bool isDeleted, IEnumerable<IStoreSection> sections)
    {
        Id = id;
        Name = name;
        IsDeleted = isDeleted;
        _sections = new StoreSections(sections);
    }

    public StoreId Id { get; }
    public string Name { get; private set; }
    public bool IsDeleted { get; }
    public IReadOnlyCollection<IStoreSection> Sections => _sections.AsReadOnly();

    public IStoreSection GetDefaultSection()
    {
        return _sections.GetDefaultSection();
    }

    public bool ContainsSection(SectionId sectionId)
    {
        return _sections.Contains(sectionId);
    }

    public void ChangeName(string name)
    {
        Name = name;
    }

    public void UpdateStores(IEnumerable<IStoreSection> storeSections)
    {
        _sections = new StoreSections(storeSections);
    }
}