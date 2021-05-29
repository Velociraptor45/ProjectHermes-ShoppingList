﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Contexts;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Migrations.Manufacturers
{
    [DbContext(typeof(ManufacturerContext))]
    [Migration("20210529134726_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities.ItemCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("ItemCategory");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Entities.Manufacturer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities.ItemsOnList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("InBasket")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<float>("Quantity")
                        .HasColumnType("float");

                    b.Property<int?>("SectionId")
                        .HasColumnType("int");

                    b.Property<int>("ShoppingListId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("SectionId");

                    b.HasIndex("ShoppingListId");

                    b.ToTable("ItemsOnList");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities.ShoppingList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("CompletionDate")
                        .HasColumnType("datetime");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.ToTable("ShoppingList");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.AvailableAt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DefaultSectionId")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<float>("Price")
                        .HasColumnType("float");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DefaultSectionId");

                    b.HasIndex("ItemId");

                    b.HasIndex("StoreId");

                    b.ToTable("AvailableAt");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid?>("CreatedFrom")
                        .HasColumnType("char(36)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsTemporary")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("ItemCategoryId")
                        .HasColumnType("int");

                    b.Property<int?>("ManufacturerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("PredecessorId")
                        .HasColumnType("int");

                    b.Property<float>("QuantityInPacket")
                        .HasColumnType("float");

                    b.Property<int>("QuantityType")
                        .HasColumnType("int");

                    b.Property<int>("QuantityTypeInPacket")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ItemCategoryId");

                    b.HasIndex("ManufacturerId");

                    b.HasIndex("PredecessorId");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsDefaultSection")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("SortIndex")
                        .HasColumnType("int");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.ToTable("Section");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Store");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities.ItemsOnList", b =>
                {
                    b.HasOne("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Section", "Section")
                        .WithMany("ActualItemsSections")
                        .HasForeignKey("SectionId");

                    b.HasOne("ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities.ShoppingList", "ShoppingList")
                        .WithMany("ItemsOnList")
                        .HasForeignKey("ShoppingListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Section");

                    b.Navigation("ShoppingList");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities.ShoppingList", b =>
                {
                    b.HasOne("ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Store");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.AvailableAt", b =>
                {
                    b.HasOne("ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Section", "Section")
                        .WithMany("DefaultItemsInSection")
                        .HasForeignKey("DefaultSectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.Item", "Item")
                        .WithMany("AvailableAt")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Store", "Store")
                        .WithMany("AvailableItems")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Section");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.Item", b =>
                {
                    b.HasOne("ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities.ItemCategory", "ItemCategory")
                        .WithMany("Items")
                        .HasForeignKey("ItemCategoryId");

                    b.HasOne("ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Entities.Manufacturer", "Manufacturer")
                        .WithMany("Products")
                        .HasForeignKey("ManufacturerId");

                    b.HasOne("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.Item", "Predecessor")
                        .WithMany()
                        .HasForeignKey("PredecessorId");

                    b.Navigation("ItemCategory");

                    b.Navigation("Manufacturer");

                    b.Navigation("Predecessor");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Section", b =>
                {
                    b.HasOne("ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Store", "Store")
                        .WithMany("Sections")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Store");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Entities.ItemCategory", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Entities.Manufacturer", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities.ShoppingList", b =>
                {
                    b.Navigation("ItemsOnList");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.Item", b =>
                {
                    b.Navigation("AvailableAt");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Section", b =>
                {
                    b.Navigation("ActualItemsSections");

                    b.Navigation("DefaultItemsInSection");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Store", b =>
                {
                    b.Navigation("AvailableItems");

                    b.Navigation("Sections");
                });
#pragma warning restore 612, 618
        }
    }
}
