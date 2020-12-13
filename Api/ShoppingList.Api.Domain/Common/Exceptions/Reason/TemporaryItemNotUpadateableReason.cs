﻿using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class TemporaryItemNotUpadateableReason : IReason
    {
        public TemporaryItemNotUpadateableReason(StoreItemId id)
        {
            Message = $"Item {id} is temporary and thus cannot be updated.";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.TemporaryItemNotUpadateable;
    }
}