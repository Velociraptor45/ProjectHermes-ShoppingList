﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Contexts;

#nullable disable

namespace ProjectHermes.ShoppingList.Api.Repositories.Migrations.Items
{
    [DbContext(typeof(ItemContext))]
    [Migration("20230318154137_AddDeleteFlagToItemTypes")]
    partial class AddDeleteFlagToItemTypes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.AvailableAt", b =>
                {
                    b.Property<Guid>("ItemId")
                        .HasColumnType("char(36)")
                        .HasColumnOrder(1);

                    b.Property<Guid>("StoreId")
                        .HasColumnType("char(36)")
                        .HasColumnOrder(2);

                    b.Property<Guid>("DefaultSectionId")
                        .HasColumnType("char(36)");

                    b.Property<float>("Price")
                        .HasColumnType("float");

                    b.HasKey("ItemId", "StoreId");

                    b.ToTable("AvailableAts");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("CreatedFrom")
                        .HasColumnType("char(36)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsTemporary")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid?>("ItemCategoryId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ManufacturerId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("PredecessorId")
                        .HasColumnType("char(36)");

                    b.Property<float?>("QuantityInPacket")
                        .HasColumnType("float");

                    b.Property<int>("QuantityType")
                        .HasColumnType("int");

                    b.Property<int?>("QuantityTypeInPacket")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("PredecessorId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.ItemType", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("PredecessorId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("PredecessorId");

                    b.ToTable("ItemTypes");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.ItemTypeAvailableAt", b =>
                {
                    b.Property<Guid>("ItemTypeId")
                        .HasColumnType("char(36)")
                        .HasColumnOrder(1);

                    b.Property<Guid>("StoreId")
                        .HasColumnType("char(36)")
                        .HasColumnOrder(2);

                    b.Property<Guid>("DefaultSectionId")
                        .HasColumnType("char(36)");

                    b.Property<float>("Price")
                        .HasColumnType("float");

                    b.HasKey("ItemTypeId", "StoreId");

                    b.ToTable("ItemTypeAvailableAts");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.AvailableAt", b =>
                {
                    b.HasOne("ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.Item", "Item")
                        .WithMany("AvailableAt")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.Item", b =>
                {
                    b.HasOne("ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.Item", "Predecessor")
                        .WithMany()
                        .HasForeignKey("PredecessorId");

                    b.Navigation("Predecessor");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.ItemType", b =>
                {
                    b.HasOne("ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.Item", "Item")
                        .WithMany("ItemTypes")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.ItemType", "Predecessor")
                        .WithMany()
                        .HasForeignKey("PredecessorId");

                    b.Navigation("Item");

                    b.Navigation("Predecessor");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.ItemTypeAvailableAt", b =>
                {
                    b.HasOne("ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.ItemType", "ItemType")
                        .WithMany("AvailableAt")
                        .HasForeignKey("ItemTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ItemType");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.Item", b =>
                {
                    b.Navigation("AvailableAt");

                    b.Navigation("ItemTypes");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.ItemType", b =>
                {
                    b.Navigation("AvailableAt");
                });
#pragma warning restore 612, 618
        }
    }
}