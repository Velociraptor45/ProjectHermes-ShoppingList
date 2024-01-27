using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons;

public class SectionNotPartOfStoreReason : IReason
{
    public SectionNotPartOfStoreReason(SectionId sectionId, StoreId storeId)
    {
        Message = $"Section {sectionId} is not part of store {storeId}.";
    }

    public string Message { get; }

    public ErrorReasonCode ErrorCode => ErrorReasonCode.SectionNotPartOfStore;
}