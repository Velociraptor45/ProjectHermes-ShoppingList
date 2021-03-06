﻿using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public class StoreNotFoundReason : IReason
    {
        public StoreNotFoundReason(StoreId id)
        {
            Message = $"Store {id.Value} not found.";
        }

        public string Message { get; }

        public ErrorReasonCode ErrorCode => ErrorReasonCode.StoreNotFound;
    }
}