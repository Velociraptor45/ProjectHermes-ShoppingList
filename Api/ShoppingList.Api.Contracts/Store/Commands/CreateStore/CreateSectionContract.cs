﻿namespace ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore
{
    public class CreateSectionContract
    {
        public CreateSectionContract(string name, int sortingIndex, bool isDefaultSection)
        {
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
        }

        public string Name { get; set; }
        public int SortingIndex { get; set; }
        public bool IsDefaultSection { get; set; }
    }
}