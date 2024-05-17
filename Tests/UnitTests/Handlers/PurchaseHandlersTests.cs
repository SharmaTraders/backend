using Application.CommandHandlers.invoice.purchase;
using CommandContracts.invoice.purchase;
using Domain.Entity;
using Domain.Repository;
using Moq;
using UnitTests.Fakes;

namespace UnitTests.Handlers;

public class PurchaseHandlersTests {
    [Fact]
    public async Task CannotCreatePurchase_With_InvalidBillingPartyId() {
        var purchaseRepoMock = new Mock<IPurchaseRepository>();
        var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
        var itemRepoMock = new Mock<IItemRepository>();
        var unitOfWorkMock = new MockUnitOfWork();

        var handler = new AddPurchaseHandler(purchaseRepoMock.Object, billingPartyRepoMock.Object, itemRepoMock.Object,
            unitOfWorkMock);

        var request = new AddPurchase.Request(
            BillingPartyId: "invalid-guid",
            Date: "2023-01-01",
            PurchaseLines: new List<AddPurchase.PurchaseLines> {
                new AddPurchase.PurchaseLines(ItemId: Guid.NewGuid().ToString(), Quantity: 1, UnitPrice: 10, Report: 1)
            },
            Remarks: "remarks",
            VatAmount: 3,
            TransportFee: 2,
            PaidAmount: 0,
            InvoiceNumber: 2
        );

        // Act and Assert
        var exception = await Assert.ThrowsAsync<DomainValidationException>(
            () => handler.Handle(request, new CancellationToken()));

        Assert.Equal("billingpartyid", exception.Type.ToLower());
        Assert.Equal(ErrorMessages.IdInvalid("invalid-guid"), exception.Message);
    }

    [Fact]
    public async Task CannotCreatePurchase_BillingPartyNotFound() {
        var purchaseRepoMock = new Mock<IPurchaseRepository>();
        var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
        var itemRepoMock = new Mock<IItemRepository>();
        var unitOfWorkMock = new MockUnitOfWork();

        billingPartyRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((BillingPartyEntity) null);
        var handler = new AddPurchaseHandler(purchaseRepoMock.Object, billingPartyRepoMock.Object, itemRepoMock.Object,
            unitOfWorkMock);


        Guid billingPartyId = Guid.NewGuid();
        var request = new AddPurchase.Request(
            BillingPartyId: billingPartyId.ToString(),
            Date: "2023-01-01",
            PurchaseLines: new List<AddPurchase.PurchaseLines> {
                new AddPurchase.PurchaseLines(ItemId: Guid.NewGuid().ToString(), Quantity: 1, UnitPrice: 10, Report: 1)
            },
            Remarks: "remarks",
            VatAmount: 3,
            TransportFee: 2,
            PaidAmount: 0,
            InvoiceNumber: 2
        );

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<DomainValidationException>(() => handler.Handle(request, new CancellationToken()));
        Assert.Equal("BillingPartyId", exception.Type);
        Assert.Equal(ErrorMessages.BillingPartyNotFound((billingPartyId)), exception.Message);
    }


    [Fact]
    public async Task CannotCreatePurchase_ItemNotFound() {
        // Arrange
        var purchaseRepositoryMock = new Mock<IPurchaseRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        var billingPartyRepositoryMock = new Mock<IBillingPartyRepository>();
        var itemRepositoryMock = new Mock<IItemRepository>();


        var billingParty = new BillingPartyEntity
            {Id = Guid.NewGuid(), Name = "Valid Party", Address = "Valid Address"};
        billingPartyRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(billingParty);

        itemRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((ItemEntity) null);

        var handler = new AddPurchaseHandler(purchaseRepositoryMock.Object,
            billingPartyRepositoryMock.Object, itemRepositoryMock.Object, unitOfWorkMock);

        Guid itemId = Guid.NewGuid();
        var request = new AddPurchase.Request(
            BillingPartyId: billingParty.Id.ToString(),
            Date: "2023-01-01",
            PurchaseLines: new List<AddPurchase.PurchaseLines> {
                new AddPurchase.PurchaseLines(ItemId: itemId.ToString(), Quantity: 1, UnitPrice: 10, Report: 1)
            },
            Remarks: "remarks",
            VatAmount: 3,
            TransportFee: 2,
            PaidAmount: 0,
            InvoiceNumber: 2
        );

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<DomainValidationException>(() => handler.Handle(request, new CancellationToken()));
        Assert.Equal("ItemId", exception.Type);
        Assert.Equal(ErrorMessages.ItemNotFound(itemId), exception.Message);
    }

    [Fact]
    public async Task CannotCreatePurchase_With_InvalidItemId() {
        // Arrange
        var purchaseRepositoryMock = new Mock<IPurchaseRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        var billingPartyRepositoryMock = new Mock<IBillingPartyRepository>();
        var itemRepositoryMock = new Mock<IItemRepository>();


        var billingParty = new BillingPartyEntity
            {Id = Guid.NewGuid(), Name = "Valid Party", Address = "Valid Address"};
        billingPartyRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(billingParty);

        itemRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((ItemEntity) null);

        var handler = new AddPurchaseHandler(purchaseRepositoryMock.Object,
            billingPartyRepositoryMock.Object, itemRepositoryMock.Object, unitOfWorkMock);

        Guid itemId = Guid.NewGuid();
        var request = new AddPurchase.Request(
            BillingPartyId: billingParty.Id.ToString(),
            Date: "2023-01-01",
            PurchaseLines: new List<AddPurchase.PurchaseLines> {
                new AddPurchase.PurchaseLines(ItemId: "invalid-guid", Quantity: 1, UnitPrice: 10, Report: 1)
            },
            Remarks: "remarks",
            VatAmount: 3,
            TransportFee: 2,
            PaidAmount: 0,
            InvoiceNumber: 2
        );

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<DomainValidationException>(() => handler.Handle(request, new CancellationToken()));
        Assert.Equal("ItemId", exception.Type);
        Assert.Equal(ErrorMessages.IdInvalid("invalid-guid"), exception.Message);
    }

    [Fact]
    public async Task CanCreatePurchase_Successfully() {
        // Arrange
        var purchaseRepositoryMock = new Mock<IPurchaseRepository>();
        var unitOfWorkMock = new MockUnitOfWork();
        var billingPartyRepositoryMock = new Mock<IBillingPartyRepository>();
        var itemRepositoryMock = new Mock<IItemRepository>();

        var billingParty = new BillingPartyEntity
            {Id = Guid.NewGuid(), Name = "Valid Party", Address = "Valid Address", Balance = 50};
        var item = new ItemEntity {
            Id = Guid.NewGuid(), Name = "Valid Item", CurrentStockAmount = 100, CurrentEstimatedStockValuePerKilo = 50
        };

        billingPartyRepositoryMock.Setup(repo => repo.GetByIdAsync(billingParty.Id))
            .ReturnsAsync(billingParty);

        itemRepositoryMock.Setup(repo => repo.GetByIdAsync(item.Id))
            .ReturnsAsync(item);

        var handler = new AddPurchaseHandler(purchaseRepositoryMock.Object,
            billingPartyRepositoryMock.Object, itemRepositoryMock.Object, unitOfWorkMock);

        var request = new AddPurchase.Request(
            BillingPartyId: billingParty.Id.ToString(),
            Date: "2023-01-01",
            PurchaseLines: new List<AddPurchase.PurchaseLines> {
                new AddPurchase.PurchaseLines(ItemId: item.Id.ToString(), Quantity: 1, UnitPrice: 10, Report: 1)
            },
            Remarks: "remarks",
            VatAmount: 3,
            TransportFee: 2,
            PaidAmount: 5,
            InvoiceNumber: 2
        );

        // Act
        var response = await handler.Handle(request, new CancellationToken());

        // Assert
        Assert.NotNull(response);
        Assert.False(string.IsNullOrEmpty(response.PurchaseId));
        Assert.True(Guid.TryParse(response.PurchaseId, out _));
        purchaseRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<PurchaseEntity>()), Times.Once);
        // 50-9 = 41 (50 is the initial balance, 9 is the total amount of the purchase after deducting the paid amount)
        Assert.Equal(41, billingParty.Balance);

        // 100 + 1 = 101 (100 is the initial stock amount, 1 is the quantity of the purchase)
        Assert.Equal(101, item.CurrentStockAmount);
        Assert.Equal(1, unitOfWorkMock.CallCount);
    }
}