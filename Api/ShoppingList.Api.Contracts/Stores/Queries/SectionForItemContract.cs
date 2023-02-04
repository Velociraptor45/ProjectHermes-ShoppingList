﻿using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries
{
    public class SectionForItemContract
    {
        public SectionForItemContract(Guid id, string name, int sortingIndex)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
        }

        public Guid Id { get; }
        public string Name { get; }
        public int SortingIndex { get; }
    }
}