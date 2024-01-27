using Force.DeepCloner;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Shared;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Fixtures;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using ProjectHermes.ShoppingList.Api.TestTools.Extensions;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Models.ShoppingLists;

public class ShoppingListTests
{
    public class AddItem
    {
        private readonly AddItemFixture _fixture = new();

        [Fact]
        public void AddItem_WithItemIdIsAlreadyOnList_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupSections();
            _fixture.SetupValidSectionId();
            var sut = _fixture.CreateSut();
            _fixture.SetupItemAlreadyOnShoppingList(sut);

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.SectionId);

            // Act
            var action = () => sut.AddItem(_fixture.Item, _fixture.SectionId.Value);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.ItemAlreadyOnShoppingList);
        }

        [Fact]
        public void AddItem_WithSectionNotFound_ShouldThrowDomainException()
        {
            _fixture.SetupSections();
            _fixture.SetupItem();
            _fixture.SetupInvalidSectionId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.SectionId);

            // Act
            var action = () => sut.AddItem(_fixture.Item, _fixture.SectionId.Value);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.SectionNotPartOfStore);
        }

        [Fact]
        public void AddItem_WithValidItemWithActualId_ShouldAddItemToList()
        {
            // Arrange
            _fixture.SetupSections();
            _fixture.SetupItem();
            _fixture.SetupValidSectionId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Item);
            TestPropertyNotSetException.ThrowIfNull(_fixture.SectionId);

            // Act
            sut.AddItem(_fixture.Item, _fixture.SectionId.Value);

            // Assert
            _fixture.VerifySectionContainsItem(sut);
        }

        private sealed class AddItemFixture : ShoppingListFixture
        {
            private readonly CommonFixture _commonFixture = new();
            private List<ShoppingListSection>? _sections;
            public ShoppingListItem? Item { get; private set; }
            public SectionId? SectionId { get; private set; }

            public void SetupSections()
            {
                _sections = new ShoppingListSectionBuilder().CreateMany(3).ToList();
                Builder.WithSections(_sections);
            }

            public void SetupItem()
            {
                Item = new ShoppingListItemBuilder().Create();
            }

            public void SetupItemAlreadyOnShoppingList(Domain.ShoppingLists.Models.ShoppingList sut)
            {
                Item = _commonFixture.ChooseRandom(sut.Items);
            }

            public void SetupValidSectionId()
            {
                TestPropertyNotSetException.ThrowIfNull(_sections);

                SectionId = _commonFixture.ChooseRandom(_sections).Id;
            }

            public void SetupInvalidSectionId()
            {
                SectionId = Domain.Stores.Models.SectionId.New;
            }

            public void VerifySectionContainsItem(Domain.ShoppingLists.Models.ShoppingList result)
            {
                TestPropertyNotSetException.ThrowIfNull(SectionId);
                result.Sections.First(s => s.Id == SectionId.Value).Items.Should().Contain(x => x == Item);
            }
        }
    }

    public class RemoveItemAndItsTypes
    {
        private readonly RemoveItemAndItsTypesFixture _fixture = new();

        [Fact]
        public void RemoveItemAndItsTypes_WithItemIdNotOnList_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupItemId();
            var sut = _fixture.CreateSut();
            var expected = sut.DeepClone();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

            // Act
            sut.RemoveItemAndItsTypes(_fixture.ItemId.Value);

            // Assert
            sut.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void RemoveItemAndItsTypes_WithValidItemId_WithItemWithoutTypes_ShouldRemoveItemFromList()
        {
            // Arrange
            _fixture.SetupItemId();
            _fixture.SetupItem();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

            // Act
            sut.RemoveItemAndItsTypes(_fixture.ItemId.Value);

            // Assert
            sut.Items.Should().HaveCount(2);
            sut.Items.Should().NotContain(i => i.Id == _fixture.ItemId.Value);
        }

        [Fact]
        public void RemoveItemAndItsTypes_WithValidItemId_WithItemWithTypes_ShouldRemoveItemFromList()
        {
            // Arrange
            _fixture.SetupItemId();
            _fixture.SetupItemType();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

            // Act
            sut.RemoveItemAndItsTypes(_fixture.ItemId.Value);

            // Assert
            sut.Items.Should().HaveCount(2);
            sut.Items.Should().NotContain(i => i.Id == _fixture.ItemId.Value);
        }

        private sealed class RemoveItemAndItsTypesFixture : ShoppingListFixture
        {
            public ItemId? ItemId { get; private set; }

            public void SetupItemId()
            {
                ItemId = Domain.Items.Models.ItemId.New;
            }

            public void SetupItem()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                var items = new List<ShoppingListItem>()
                {
                    new ShoppingListItemBuilder().WithId(ItemId.Value).WithoutTypeId().Create(),
                    new ShoppingListItemBuilder().Create(),
                    new ShoppingListItemBuilder().Create(),
                };
                items.Shuffle();
                var section = ShoppingListSectionMother.Items(items).Create();
                Builder.WithSection(section);
            }

            public void SetupItemType()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                var items = new List<ShoppingListItem>()
                {
                    new ShoppingListItemBuilder().WithId(ItemId.Value).Create(),
                    new ShoppingListItemBuilder().Create(),
                    new ShoppingListItemBuilder().Create(),
                };
                items.Shuffle();
                var section = ShoppingListSectionMother.Items(items).Create();
                Builder.WithSection(section);
            }
        }
    }

    public class RemoveItem
    {
        private readonly CommonFixture _commonFixture = new();
        private readonly ShoppingListSectionMockFixture _shoppingListSectionMockFixture = new();

        [Fact]
        public void RemoveItem_WithItemIdNotOnList_ShouldDoNothing()
        {
            // Arrange
            var sectionMocks = _shoppingListSectionMockFixture.CreateMany(2).ToList();
            sectionMocks.ForEach(m => m.SetupContainsItem(false));
            var list = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            var shoppingListItemId = new ItemId(Guid.NewGuid());

            // Act
            list.RemoveItem(shoppingListItemId);

            // Assert
            using (new AssertionScope())
            {
                foreach (var mock in sectionMocks)
                {
                    mock.VerifyRemoveItemNever();
                }
            }
        }

        [Fact]
        public void RemoveItem_WithValidItemId_ShouldRemoveItemFromList()
        {
            // Arrange
            var sectionMocks = _shoppingListSectionMockFixture.CreateMany(3).ToList();
            var shoppingList = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            ShoppingListSectionMock chosenSectionMock = _commonFixture.ChooseRandom(sectionMocks);
            ShoppingListItem chosenItem = _commonFixture.ChooseRandom(chosenSectionMock.Object.Items);

            foreach (var sectionMock in sectionMocks)
                sectionMock.SetupContainsItem(chosenItem.Id, chosenItem.TypeId,
                    sectionMock.Object.IsEquivalentTo(chosenSectionMock.Object));

            // Act
            shoppingList.RemoveItem(chosenItem.Id, chosenItem.TypeId);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sectionMocks)
                {
                    if (section == chosenSectionMock)
                        section.VerifyRemoveItem(chosenItem.Id, chosenItem.TypeId, Times.Once);
                    else
                        section.VerifyRemoveItemNever();
                }
            }
        }
    }

    public class PutItemInBasket
    {
        private readonly CommonFixture _commonFixture = new();
        private readonly ShoppingListSectionMockFixture _shoppingListSectionMockFixture = new();

        [Fact]
        public void PutItemInBasket_WithItemIdNotOnList_ShouldThrowDomainException()
        {
            // Arrange
            var list = ShoppingListMother.ThreeSections().Create();
            var shoppingListItemId = new ItemId(Guid.NewGuid());

            // Act
            var action = () => list.PutItemInBasket(shoppingListItemId);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.ItemNotOnShoppingList);
        }

        [Fact]
        public void PutItemInBasket_WithValidItemId_ShouldPutItemInBasket()
        {
            // Arrange
            var sectionMocks = _shoppingListSectionMockFixture.CreateMany(3).ToList();
            var shoppingList = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            ShoppingListSectionMock chosenSectionMock = _commonFixture.ChooseRandom(sectionMocks);
            ShoppingListItem chosenItem = _commonFixture.ChooseRandom(chosenSectionMock.Object.Items);

            foreach (var sectionMock in sectionMocks)
                sectionMock.SetupContainsItem(chosenItem.Id, chosenItem.TypeId,
                    sectionMock.Object.IsEquivalentTo(chosenSectionMock.Object));

            // Act
            shoppingList.PutItemInBasket(chosenItem.Id, chosenItem.TypeId);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sectionMocks)
                {
                    if (section == chosenSectionMock)
                        section.VerifyPutItemInBasketOnce(chosenItem.Id, chosenItem.TypeId);
                    else
                        section.VerifyPutItemInBasketNever();
                }
            }
        }
    }

    public class RemoveFromBasket
    {
        private readonly CommonFixture _commonFixture = new();
        private readonly ShoppingListSectionMockFixture _shoppingListSectionMockFixture = new();

        [Fact]
        public void RemoveFromBasket_WithItemIdNotOnList_ShouldThrowDomainException()
        {
            // Arrange
            var list = ShoppingListMother.ThreeSections().Create();
            var shoppingListItemId = new ItemId(Guid.NewGuid());

            // Act
            Action action = () => list.RemoveFromBasket(shoppingListItemId, null);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.ItemNotOnShoppingList);
        }

        [Fact]
        public void RemoveFromBasket_WithValidItemId_ShouldRemoveItemFromList()
        {
            // Arrange
            var sectionMocks = _shoppingListSectionMockFixture.CreateMany(3).ToList();
            var shoppingList = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            ShoppingListSectionMock chosenSectionMock = _commonFixture.ChooseRandom(sectionMocks);
            ShoppingListItem chosenItem = _commonFixture.ChooseRandom(chosenSectionMock.Object.Items);

            foreach (var sectionMock in sectionMocks)
                sectionMock.SetupContainsItem(chosenItem.Id, chosenItem.TypeId,
                    sectionMock.Object.IsEquivalentTo(chosenSectionMock.Object));

            // Act
            shoppingList.RemoveFromBasket(chosenItem.Id, chosenItem.TypeId);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sectionMocks)
                {
                    if (section == chosenSectionMock)
                        section.VerifyRemoveItemFromBasketOnce(chosenItem.Id, chosenItem.TypeId);
                    else
                        section.VerifyRemoveItemFromBasketNever();
                }
            }
        }
    }

    public class ChangeItemQuantity
    {
        private readonly CommonFixture _commonFixture = new();
        private readonly ShoppingListSectionMockFixture _shoppingListSectionMockFixture = new();

        [Fact]
        public void ChangeItemQuantity_WithItemIdNotOnList_ShouldThrowDomainException()
        {
            // Arrange
            var list = ShoppingListMother.ThreeSections().Create();
            var shoppingListItemId = new ItemId(Guid.NewGuid());
            var quantity = new QuantityInBasketBuilder().Create();

            // Act
            Action action = () => list.ChangeItemQuantity(shoppingListItemId, null, quantity);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.ItemNotOnShoppingList);
        }

        [Fact]
        public void ChangeItemQuantity_WithValidItemId_ShouldChangeQuantity()
        {
            // Arrange
            var sectionMocks = _shoppingListSectionMockFixture.CreateMany(3).ToList();
            var shoppingList = ShoppingListMother.Initial()
                .WithSections(sectionMocks.Select(s => s.Object))
                .Create();

            ShoppingListSectionMock chosenSectionMock = _commonFixture.ChooseRandom(sectionMocks);
            ShoppingListItem chosenItem = _commonFixture.ChooseRandom(chosenSectionMock.Object.Items);

            foreach (var sectionMock in sectionMocks)
            {
                sectionMock.SetupContainsItem(chosenItem.Id, chosenItem.TypeId,
                    sectionMock.Object.IsEquivalentTo(chosenSectionMock.Object));
            }

            var quantity = new QuantityInBasketBuilder().Create();

            // Act
            shoppingList.ChangeItemQuantity(chosenItem.Id, chosenItem.TypeId, quantity);

            // Assert
            using (new AssertionScope())
            {
                foreach (var section in sectionMocks)
                {
                    if (section == chosenSectionMock)
                        section.VerifyChangeItemQuantityOnce(chosenItem.Id, chosenItem.TypeId, quantity);
                    else
                        section.VerifyChangeItemQuantityNever();
                }
            }
        }
    }

    public class Finish
    {
        [Fact]
        public void Finish_WithCompletedShoppingList_ShouldThrowDomainException()
        {
            // Arrange
            var shoppingList = ShoppingListMother.Completed().Create();
            var completionDate = new DomainTestBuilder<DateTimeOffset>().Create();

            // Act
            var action = () => shoppingList.Finish(completionDate);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.ShoppingListAlreadyFinished);
        }

        [Fact]
        public void Finish_WithUncompletedShoppingList_ShouldSetCompletionDate()
        {
            // Arrange
            var shoppingList = ShoppingListMother.OneSectionWithOneItemInBasketAndOneNot().Create();
            var itemsInBasket = shoppingList.Items.Where(i => i.IsInBasket);
            var itemsNotInBasket = shoppingList.Items.Where(i => !i.IsInBasket);

            var completionDate = new DomainTestBuilder<DateTimeOffset>().Create();

            // Act
            IShoppingList result = shoppingList.Finish(completionDate);

            // Assert
            using (new AssertionScope())
            {
                shoppingList.Sections.First().Items.Should().BeEquivalentTo(itemsInBasket);
                result.Sections.First().Items.Should().BeEquivalentTo(itemsNotInBasket);
                shoppingList.CompletionDate.Should().Be(completionDate);
            }
        }
    }

    public class AddSection
    {
        private readonly CommonFixture _commonFixture = new();

        [Fact]
        public void AddSection_WithSectionAlreadyInShoppingList_ShouldThrowDomainException()
        {
            IShoppingList shoppingList = ShoppingListMother.OneSectionWithOneItemInBasket().Create();
            IShoppingListSection section = _commonFixture.ChooseRandom(shoppingList.Sections);

            // Act
            var action = () => shoppingList.AddSection(section);

            // Assert
            action.Should().ThrowDomainException(ErrorReasonCode.SectionAlreadyInShoppingList);
        }

        [Fact]
        public void AddSection_WithNewSection_ShouldThrowDomainException()
        {
            IShoppingList shoppingList = ShoppingListMother.OneSectionWithOneItemInBasket().Create();
            IShoppingListSection section = new ShoppingListSectionBuilder().Create();

            // Act
            shoppingList.AddSection(section);

            // Assert
            shoppingList.Sections.Should().Contain(section);
        }
    }

    public class TransferItem
    {
        private readonly TransferItemFixture _fixture = new();

        [Fact]
        public void TransferItem_WithoutNewSection_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupSectionIds();
            _fixture.SetupItemIdAndItemTypeId();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

            // Act
            var act = () => sut.TransferItem(_fixture.OldSectionId.Value, _fixture.ItemId.Value, _fixture.ItemTypeId);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.SectionNotFound);
        }

        [Fact]
        public void TransferItem_WithoutItem_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupSectionIds();
            _fixture.SetupItemIdAndItemTypeId();
            _fixture.SetupOldAndNewSectionsWithoutItem();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);

            // Act
            var act = () => sut.TransferItem(_fixture.OldSectionId.Value, _fixture.ItemId.Value, _fixture.ItemTypeId);

            // Assert
            act.Should().ThrowDomainException(ErrorReasonCode.ItemNotOnShoppingList);
        }

        [Fact]
        public void TransferItem_WithOldSectionEqualsNewSection_ShouldNotTransferItem()
        {
            // Arrange
            _fixture.SetupSectionIds();
            _fixture.SetupItemIdAndItemTypeId();
            _fixture.SetupOldAndNewSections();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

            // Act
            sut.TransferItem(_fixture.OldSectionId.Value, _fixture.ItemId.Value, _fixture.ItemTypeId);

            // Assert
            var oldSection = sut.Sections.First(s => s.Id == _fixture.OldSectionId.Value);
            var newSection = sut.Sections.First(s => s.Id == _fixture.NewSectionId.Value);

            oldSection.Items.Should().Contain(i =>
                i.Id == _fixture.ItemId.Value && i.TypeId == _fixture.ItemTypeId.Value);
            newSection.Items.Should().NotContain(i =>
                i.Id == _fixture.ItemId.Value && i.TypeId == _fixture.ItemTypeId.Value);
        }

        [Fact]
        public void TransferItem_WithValidData_ShouldTransferItem()
        {
            // Arrange
            _fixture.SetupSectionIds();
            _fixture.SetupItemIdAndItemTypeId();
            _fixture.SetupOldAndNewSections();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.OldSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.NewSectionId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemTypeId);

            // Act
            sut.TransferItem(_fixture.NewSectionId.Value, _fixture.ItemId.Value, _fixture.ItemTypeId);

            // Assert
            var oldSection = sut.Sections.First(s => s.Id == _fixture.OldSectionId.Value);
            var newSection = sut.Sections.First(s => s.Id == _fixture.NewSectionId.Value);

            oldSection.Items.Should().NotContain(i =>
                i.Id == _fixture.ItemId.Value && i.TypeId == _fixture.ItemTypeId.Value);
            newSection.Items.Should().Contain(i =>
                i.Id == _fixture.ItemId.Value && i.TypeId == _fixture.ItemTypeId.Value);
        }

        private class TransferItemFixture
        {
            private readonly ShoppingListBuilder _builder = ShoppingListMother.Initial();

            public SectionId? OldSectionId { get; private set; }
            public SectionId? NewSectionId { get; private set; }
            public ItemId? ItemId { get; private set; }
            public ItemTypeId? ItemTypeId { get; private set; }

            public Domain.ShoppingLists.Models.ShoppingList CreateSut()
            {
                return _builder.Create();
            }

            public void SetupSectionIds()
            {
                OldSectionId = SectionId.New;
                NewSectionId = SectionId.New;
            }

            public void SetupItemIdAndItemTypeId()
            {
                ItemId = Domain.Items.Models.ItemId.New;
                ItemTypeId = Domain.Items.Models.ItemTypeId.New;
            }

            public void SetupOldAndNewSectionsWithoutItem()
            {
                TestPropertyNotSetException.ThrowIfNull(OldSectionId);
                TestPropertyNotSetException.ThrowIfNull(NewSectionId);
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                var sections = new List<IShoppingListSection>
                {
                    new ShoppingListSectionBuilder()
                        .WithId(OldSectionId.Value)
                        .Create(),
                    new ShoppingListSectionBuilder()
                        .WithId(NewSectionId.Value)
                        .Create()
                };

                _builder.WithSections(sections);
            }

            public void SetupOldAndNewSections()
            {
                TestPropertyNotSetException.ThrowIfNull(OldSectionId);
                TestPropertyNotSetException.ThrowIfNull(NewSectionId);
                TestPropertyNotSetException.ThrowIfNull(ItemId);

                var item = new ShoppingListItemBuilder()
                    .WithId(ItemId.Value)
                    .WithTypeId(ItemTypeId)
                    .Create();

                var sections = new List<IShoppingListSection>
                {
                    new ShoppingListSectionBuilder()
                        .WithId(OldSectionId.Value)
                        .WithItem(item)
                        .Create(),
                    new ShoppingListSectionBuilder()
                        .WithId(NewSectionId.Value)
                        .Create()
                };

                _builder.WithSections(sections);
            }
        }
    }

    private abstract class ShoppingListFixture
    {
        protected readonly ShoppingListBuilder Builder = new();

        protected ShoppingListFixture()
        {
            Builder.WithoutCompletionDate();
        }

        public Domain.ShoppingLists.Models.ShoppingList CreateSut()
        {
            return Builder.Create();
        }
    }
}