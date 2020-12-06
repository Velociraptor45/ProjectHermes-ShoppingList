﻿using Microsoft.EntityFrameworkCore;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Entities
{
    public class ShoppingContext : DbContext
    {
        public DbSet<AvailableAt> AvailableAts { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<ItemsOnList> ItemsOnLists { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<Store> Stores { get; set; }

        public ShoppingContext(DbContextOptions<ShoppingContext> options)
            : base(options)
        {
        }
    }
}