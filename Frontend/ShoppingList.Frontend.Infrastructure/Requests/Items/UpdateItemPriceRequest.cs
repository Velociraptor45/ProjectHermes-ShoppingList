﻿using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Items;

public class UpdateItemPriceRequest : IApiRequest
{
    public UpdateItemPriceRequest(Guid itemId, Guid? itemTypeId, Guid storeId, float price)
    {
        RequestId = Guid.NewGuid();
        ItemId = itemId;
        ItemTypeId = itemTypeId;
        StoreId = storeId;
        Price = price;
    }

    public Guid ItemId { get; }
    public Guid? ItemTypeId { get; }
    public Guid StoreId { get; }
    public float Price { get; }
    public Guid RequestId { get; }
}