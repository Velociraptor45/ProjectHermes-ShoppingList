﻿using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using ShoppingList.Frontend.Models.TestKit.ShoppingLists.Models;
using Xunit;

namespace ShoppingList.Frontend.Models.Tests.ShoppingLists.Models;

public class SearchItemForShoppingListResultTests
{
    private readonly SearchItemForShoppingListResultFixture _fixture;

    public SearchItemForShoppingListResultTests()
    {
        _fixture = new SearchItemForShoppingListResultFixture();
    }

    [Theory]
    [InlineData("Melon", 32.1f, "€", "Melon | 32,1€")]
    [InlineData("Cheese", 6f, "€", "Cheese | 6€")]
    public void DisplayValue_WithManufacturerNameEmpty_ShouldReturnExpectedValue(string name, float price,
        string priceLabel, string expected)
    {
        // Arrange
        _fixture.SetupManufacturerNameEmpty();
        _fixture.SetupName(name);
        _fixture.SetupPrice(price);
        _fixture.SetupPriceLabel(priceLabel);
        var sut = _fixture.CreateSut();

        // Act
        var result = sut.DisplayValue;

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData("Butter", "MyMan", 13.5f, "€", "Butter | MyMan | 13,5€")]
    [InlineData("Bread", "AnotherMan", 1f, "€", "Bread | AnotherMan | 1€")]
    public void DisplayValue_WithManufacturerName_ShouldReturnExpectedValue(string name, string manufacturerName,
        float price, string priceLabel, string expected)
    {
        // Arrange
        _fixture.SetupManufacturerName(manufacturerName);
        _fixture.SetupName(name);
        _fixture.SetupPrice(price);
        _fixture.SetupPriceLabel(priceLabel);
        var sut = _fixture.CreateSut();

        // Act
        var result = sut.DisplayValue;

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    private class SearchItemForShoppingListResultFixture
    {
        private readonly SearchItemForShoppingListResultBuilder _builder;

        public SearchItemForShoppingListResultFixture()
        {
            _builder = new SearchItemForShoppingListResultBuilder();
        }

        public SearchItemForShoppingListResult CreateSut()
        {
            return _builder.Create();
        }

        public void SetupManufacturerNameEmpty()
        {
            _builder.WithManufacturerName(string.Empty);
        }

        public void SetupManufacturerName(string manufacturerName)
        {
            _builder.WithManufacturerName(manufacturerName);
        }

        public void SetupName(string name)
        {
            _builder.WithName(name);
        }

        public void SetupPrice(float price)
        {
            _builder.WithPrice(price);
        }

        public void SetupPriceLabel(string priceLabel)
        {
            _builder.WithPriceLabel(priceLabel);
        }
    }
}