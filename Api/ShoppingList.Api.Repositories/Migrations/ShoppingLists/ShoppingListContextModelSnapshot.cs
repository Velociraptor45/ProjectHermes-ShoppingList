﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Contexts;

#nullable disable

namespace ProjectHermes.ShoppingList.Api.Repositories.Migrations.ShoppingLists
{
    [DbContext(typeof(ShoppingListContext))]
    partial class ShoppingListContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Entities.ItemsOnList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("InBasket")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ItemTypeId")
                        .HasColumnType("char(36)");

                    b.Property<float>("Quantity")
                        .HasColumnType("float");

                    b.Property<Guid>("SectionId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ShoppingListId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ShoppingListId");

                    b.ToTable("ItemsOnLists");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Entities.ShoppingList", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset?>("CompletionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("RowVersion")
                        .IsRowVersion()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp(6)");

                    b.Property<Guid>("StoreId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.ToTable("ShoppingLists");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Entities.ItemsOnList", b =>
                {
                    b.HasOne("ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Entities.ShoppingList", "ShoppingList")
                        .WithMany("ItemsOnList")
                        .HasForeignKey("ShoppingListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShoppingList");
                });

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Entities.ShoppingList", b =>
                {
                    b.Navigation("ItemsOnList");
                });
#pragma warning restore 612, 618
        }
    }
}
