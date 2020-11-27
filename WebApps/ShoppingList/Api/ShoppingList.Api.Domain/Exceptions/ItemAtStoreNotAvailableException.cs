﻿using ShoppingList.Api.Domain.Models;
using System;

namespace ShoppingList.Api.Domain.Exceptions
{
    public class ItemAtStoreNotAvailableException : Exception
    {
        public ItemAtStoreNotAvailableException(StoreItemId itemId, StoreId storeId)
            : base($"Item {itemId} not available at store {storeId.Value}")
        {
        }

        public ItemAtStoreNotAvailableException(string message) : base(message)
        {
        }

        public ItemAtStoreNotAvailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}