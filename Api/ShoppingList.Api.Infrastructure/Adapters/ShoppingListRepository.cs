﻿using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Adapters
{
    public class ShoppingListRepository : IShoppingListRepository
    {
        private readonly ShoppingContext dbContext;
        private readonly IShoppingListFactory shoppingListFactory;

        public ShoppingListRepository(ShoppingContext dbContext, IShoppingListFactory shoppingListFactory)
        {
            this.dbContext = dbContext;
            this.shoppingListFactory = shoppingListFactory;
        }

        #region public methods

        public async Task StoreAsync(IShoppingList shoppingList, CancellationToken cancellationToken)
        {
            if (shoppingList == null)
                throw new ArgumentNullException(nameof(shoppingList));

            if (shoppingList.Id.Value <= 0)
            {
                await StoreAsNewListAsync(shoppingList, cancellationToken);
                return;
            }

            cancellationToken.ThrowIfCancellationRequested();

            var listEntity = await FindEntityByIdAsync(shoppingList.Id);
            if (listEntity == null)
            {
                throw new DomainException(new ShoppingListNotFoundReason(shoppingList.Id));
            }

            cancellationToken.ThrowIfCancellationRequested();

            await StoreModifiedListAsync(listEntity, shoppingList, cancellationToken);
        }

        public async Task<IEnumerable<IShoppingList>> FindByAsync(StoreItemId storeItemId,
            CancellationToken cancellationToken)
        {
            if (storeItemId is null)
            {
                throw new ArgumentNullException(nameof(storeItemId));
            }

            List<Entities.ShoppingList> entities = await dbContext.ShoppingLists.AsNoTracking()
                .Include(l => l.Store)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.Manufacturer)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.ItemCategory)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.AvailableAt)
                .Where(l => l.ItemsOnList.FirstOrDefault(i => i.ItemId == storeItemId.Actual.Value) != null)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return entities.Select(entity =>
            {
                return shoppingListFactory.Create(
                    new ShoppingListId(entity.Id),
                    entity.Store.ToDomain(),
                    entity.ItemsOnList.Select(map => map.Item.ToShoppingListItemDomain(entity.StoreId, entity.Id)),
                    entity.CompletionDate);
            });
        }

        public async Task<IEnumerable<IShoppingList>> FindActiveByAsync(StoreItemId storeItemId,
            CancellationToken cancellationToken)
        {
            if (storeItemId is null)
            {
                throw new ArgumentNullException(nameof(storeItemId));
            }

            List<Entities.ShoppingList> entities = await dbContext.ShoppingLists.AsNoTracking()
                .Include(l => l.Store)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.Manufacturer)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.ItemCategory)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.AvailableAt)
                .Where(l => l.ItemsOnList.FirstOrDefault(i => i.ItemId == storeItemId.Actual.Value) != null
                    && l.CompletionDate == null)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return entities.Select(entity =>
            {
                return shoppingListFactory.Create(
                    new ShoppingListId(entity.Id),
                    entity.Store.ToDomain(),
                    entity.ItemsOnList.Select(map => map.Item.ToShoppingListItemDomain(entity.StoreId, entity.Id)),
                    entity.CompletionDate);
            });
        }

        public async Task<IShoppingList> FindActiveByAsync(StoreId storeId, CancellationToken cancellationToken)
        {
            if (storeId == null)
                throw new ArgumentNullException(nameof(storeId));

            var entity = await dbContext.ShoppingLists.AsNoTracking()
                .Include(l => l.Store)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.Manufacturer)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.ItemCategory)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.AvailableAt)
                .FirstOrDefaultAsync(list => list.CompletionDate == null
                    && list.StoreId == storeId.Value);

            cancellationToken.ThrowIfCancellationRequested();

            if (entity == null)
                return null;

            return shoppingListFactory.Create(
                new ShoppingListId(entity.Id),
                entity.Store.ToDomain(),
                entity.ItemsOnList.Select(map => map.Item.ToShoppingListItemDomain(entity.StoreId, entity.Id)),
                entity.CompletionDate);
        }

        public async Task<IShoppingList> FindByAsync(ShoppingListId id, CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var entity = await dbContext.ShoppingLists.AsNoTracking()
                .Include(l => l.Store)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.Manufacturer)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.ItemCategory)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.AvailableAt)
                .FirstOrDefaultAsync(list => list.Id == id.Value);

            if (entity == null) //todo throw in command handler
                throw new DomainException(new ShoppingListNotFoundReason(id));

            cancellationToken.ThrowIfCancellationRequested();

            return shoppingListFactory.Create(
                new ShoppingListId(entity.Id),
                entity.Store.ToDomain(),
                entity.ItemsOnList.Select(map => map.Item.ToShoppingListItemDomain(entity.StoreId, entity.Id)),
                entity.CompletionDate);
        }

        public async Task<bool> ActiveShoppingListExistsForAsync(StoreId storeId, CancellationToken cancellationToken)
        {
            var list = await dbContext.ShoppingLists.AsNoTracking()
                .FirstOrDefaultAsync(list => list.StoreId == storeId.Value
                    && list.CompletionDate == null);

            cancellationToken.ThrowIfCancellationRequested();

            return list != null;
        }

        #endregion public methods

        #region private methods

        private async Task StoreModifiedListAsync(Entities.ShoppingList existingShoppingListEntity,
            IShoppingList shoppingList, CancellationToken cancellationToken)
        {
            var shoppingListEntityToStore = shoppingList.ToEntity();
            var onListMappings = existingShoppingListEntity.ItemsOnList.ToDictionary(map => map.ItemId);

            dbContext.Entry(shoppingListEntityToStore).State = EntityState.Modified;
            foreach (var map in shoppingListEntityToStore.ItemsOnList)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (onListMappings.TryGetValue(map.ItemId, out var existingMapping))
                {
                    // mapping was modified
                    map.Id = existingMapping.Id;
                    dbContext.Entry(map).State = EntityState.Modified;
                    onListMappings.Remove(map.ItemId);
                }
                else
                {
                    // mapping was added
                    dbContext.Entry(map).State = EntityState.Added;
                }
            }

            // mapping was deleted
            foreach (var map in onListMappings.Values)
            {
                dbContext.Entry(map).State = EntityState.Deleted;
            }

            cancellationToken.ThrowIfCancellationRequested();

            await dbContext.SaveChangesAsync();
        }

        private async Task StoreAsNewListAsync(IShoppingList shoppingList, CancellationToken cancellationToken)
        {
            var entity = shoppingList.ToEntity();

            dbContext.Entry(entity).State = EntityState.Added;
            foreach (var onListMap in entity.ItemsOnList)
            {
                dbContext.Entry(onListMap).State = EntityState.Added;
            }

            cancellationToken.ThrowIfCancellationRequested();

            await dbContext.SaveChangesAsync();
        }

        private async Task<Entities.ShoppingList> FindEntityByIdAsync(ShoppingListId id)
        {
            return await dbContext.ShoppingLists.AsNoTracking()
                .Include(l => l.Store)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.Manufacturer)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.ItemCategory)
                .Include(l => l.ItemsOnList)
                .ThenInclude(map => map.Item)
                .ThenInclude(item => item.AvailableAt)
                .FirstOrDefaultAsync(list => list.Id == id.Value);
        }

        #endregion private methods
    }
}