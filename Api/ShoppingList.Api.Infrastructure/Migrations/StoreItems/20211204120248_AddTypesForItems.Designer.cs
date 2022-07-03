﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Contexts;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Migrations.StoreItems
{
    [DbContext(typeof(ItemContext))]
    [Migration("20211204120248_AddTypesForItems")]
    partial class AddTypesForItems
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.8");

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

                    b.HasIndex("ItemId");

                    b.ToTable("AvailableAts");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("longtext");

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
                        .HasColumnType("longtext");

                    b.Property<int?>("PredecessorId")
                        .HasColumnType("int");

                    b.Property<float>("QuantityInPacket")
                        .HasColumnType("float");

                    b.Property<int>("QuantityType")
                        .HasColumnType("int");

                    b.Property<int>("QuantityTypeInPacket")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PredecessorId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.ItemType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("ItemType");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.ItemTypeAvailableAt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DefaultSectionId")
                        .HasColumnType("int");

                    b.Property<int>("ItemTypeId")
                        .HasColumnType("int");

                    b.Property<float>("Price")
                        .HasColumnType("float");

                    b.Property<int>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ItemTypeId");

                    b.ToTable("ItemTypeAvailableAt");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.AvailableAt", b =>
                {
                    b.HasOne("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.Item", "Item")
                        .WithMany("AvailableAt")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.Item", b =>
                {
                    b.HasOne("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.Item", "Predecessor")
                        .WithMany()
                        .HasForeignKey("PredecessorId");

                    b.Navigation("Predecessor");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.ItemType", b =>
                {
                    b.HasOne("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.Item", "Item")
                        .WithMany("ItemTypes")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.ItemTypeAvailableAt", b =>
                {
                    b.HasOne("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.ItemType", "ItemType")
                        .WithMany("AvailableAt")
                        .HasForeignKey("ItemTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ItemType");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.Item", b =>
                {
                    b.Navigation("AvailableAt");

                    b.Navigation("ItemTypes");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities.ItemType", b =>
                {
                    b.Navigation("AvailableAt");
                });
#pragma warning restore 612, 618
        }
    }
}
