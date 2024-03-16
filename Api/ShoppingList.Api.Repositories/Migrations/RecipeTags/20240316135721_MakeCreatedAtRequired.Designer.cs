﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Contexts;

#nullable disable

namespace ProjectHermes.ShoppingList.Api.Repositories.Migrations.RecipeTags
{
    [DbContext(typeof(RecipeTagContext))]
    [Migration("20240316135721_MakeCreatedAtRequired")]
    partial class MakeCreatedAtRequired
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Entities.RecipeTag", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp(6)");

                    b.HasKey("Id");

                    b.ToTable("RecipeTags");
                });
#pragma warning restore 612, 618
        }
    }
}
