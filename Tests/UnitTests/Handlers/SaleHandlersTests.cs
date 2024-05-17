using Application.CommandHandlers.invoice.sale;
using CommandContracts.invoice.sale;
using Domain.Entity;
using Domain.Repository;
using Moq;
using UnitTests.Fakes;

namespace UnitTests.Handlers
{
    public class AddSaleHandlerTests
    {
        [Fact]
        public async Task CannotCreateSale_With_InvalidBillingPartyId()
        {
            var saleRepoMock = new Mock<ISaleRepository>();
            var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
            var itemRepoMock = new Mock<IItemRepository>();
            var unitOfWorkMock = new MockUnitOfWork();

            var handler = new AddSaleHandler(saleRepoMock.Object, billingPartyRepoMock.Object, itemRepoMock.Object, unitOfWorkMock);

            var request = new AddSale.Request(
                BillingPartyId: "invalid-guid",
                Date: "2023-01-01",
                SaleLines: new List<AddSale.SaleLines> {
                    new AddSale.SaleLines(ItemId: Guid.NewGuid().ToString(), Quantity: 1, UnitPrice: 10, Report: 1)
                },
                Remarks: "remarks",
                VatAmount: 3,
                TransportFee: 2,
                ReceivedAmount: 0,
                InvoiceNumber: 2
            );

            // Act and Assert
            var exception = await Assert.ThrowsAsync<DomainValidationException>(() => handler.Handle(request, new CancellationToken()));

            Assert.Equal("billingpartyid", exception.Type.ToLower());
            Assert.Equal(ErrorMessages.IdInvalid("invalid-guid"), exception.Message);
        }

        [Fact]
        public async Task CannotCreateSale_BillingPartyNotFound()
        {
            var saleRepoMock = new Mock<ISaleRepository>();
            var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
            var itemRepoMock = new Mock<IItemRepository>();
            var unitOfWorkMock = new MockUnitOfWork();

            billingPartyRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((BillingPartyEntity)null);
            var handler = new AddSaleHandler(saleRepoMock.Object, billingPartyRepoMock.Object, itemRepoMock.Object, unitOfWorkMock);

            Guid billingPartyId = Guid.NewGuid();
            var request = new AddSale.Request(
                BillingPartyId: billingPartyId.ToString(),
                Date: "2023-01-01",
                SaleLines: new List<AddSale.SaleLines> {
                    new AddSale.SaleLines(ItemId: Guid.NewGuid().ToString(), Quantity: 1, UnitPrice: 10, Report: 1)
                },
                Remarks: "remarks",
                VatAmount: 3,
                TransportFee: 2,
                ReceivedAmount: 0,
                InvoiceNumber: 2
            );

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainValidationException>(() => handler.Handle(request, new CancellationToken()));
            Assert.Equal("BillingPartyId", exception.Type);
            Assert.Equal(ErrorMessages.BillingPartyNotFound(billingPartyId), exception.Message);
        }

        [Fact]
        public async Task CannotCreateSale_ItemNotFound()
        {
            var saleRepoMock = new Mock<ISaleRepository>();
            var unitOfWorkMock = new MockUnitOfWork();
            var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
            var itemRepoMock = new Mock<IItemRepository>();

            var billingParty = new BillingPartyEntity { Id = Guid.NewGuid(), Name = "Valid Party", Address = "Valid Address" };
            billingPartyRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(billingParty);

            itemRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ItemEntity)null);

            var handler = new AddSaleHandler(saleRepoMock.Object, billingPartyRepoMock.Object, itemRepoMock.Object, unitOfWorkMock);

            Guid itemId = Guid.NewGuid();
            var request = new AddSale.Request(
                BillingPartyId: billingParty.Id.ToString(),
                Date: "2023-01-01",
                SaleLines: new List<AddSale.SaleLines> {
                    new AddSale.SaleLines(ItemId: itemId.ToString(), Quantity: 1, UnitPrice: 10, Report: 1)
                },
                Remarks: "remarks",
                VatAmount: 3,
                TransportFee: 2,
                ReceivedAmount: 0,
                InvoiceNumber: 2
            );

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainValidationException>(() => handler.Handle(request, new CancellationToken()));
            Assert.Equal("ItemId", exception.Type);
            Assert.Equal(ErrorMessages.ItemNotFound(itemId), exception.Message);
        }

        [Fact]
        public async Task CannotCreateSale_With_InvalidItemId()
        {
            var saleRepoMock = new Mock<ISaleRepository>();
            var unitOfWorkMock = new MockUnitOfWork();
            var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
            var itemRepoMock = new Mock<IItemRepository>();

            var billingParty = new BillingPartyEntity { Id = Guid.NewGuid(), Name = "Valid Party", Address = "Valid Address" };
            billingPartyRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(billingParty);

            itemRepoMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ItemEntity)null);

            var handler = new AddSaleHandler(saleRepoMock.Object, billingPartyRepoMock.Object, itemRepoMock.Object, unitOfWorkMock);

            var request = new AddSale.Request(
                BillingPartyId: billingParty.Id.ToString(),
                Date: "2024-01-01",
                SaleLines: new List<AddSale.SaleLines> {
                    new AddSale.SaleLines(ItemId: "invalid-guid", Quantity: 1, UnitPrice: 10, Report: 1)
                },
                Remarks: "remarks",
                VatAmount: 3,
                TransportFee: 2,
                ReceivedAmount: 0,
                InvoiceNumber: 2
            );

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainValidationException>(() => handler.Handle(request, new CancellationToken()));
            Assert.Equal("ItemId", exception.Type);
            Assert.Equal(ErrorMessages.IdInvalid("invalid-guid"), exception.Message);
        }

        [Fact]
        public async Task CanCreateSale_Successfully()
        {
            var saleRepoMock = new Mock<ISaleRepository>();
            var unitOfWorkMock = new MockUnitOfWork();
            var billingPartyRepoMock = new Mock<IBillingPartyRepository>();
            var itemRepoMock = new Mock<IItemRepository>();

            var billingParty = new BillingPartyEntity { Id = Guid.NewGuid(), Name = "Valid Party", Address = "Valid Address", Balance = 50 };
            var item = new ItemEntity { Id = Guid.NewGuid(), Name = "Valid Item", CurrentStockAmount = 100, CurrentEstimatedStockValuePerKilo = 50 };

            billingPartyRepoMock.Setup(repo => repo.GetByIdAsync(billingParty.Id)).ReturnsAsync(billingParty);
            itemRepoMock.Setup(repo => repo.GetByIdAsync(item.Id)).ReturnsAsync(item);

            var handler = new AddSaleHandler(saleRepoMock.Object, billingPartyRepoMock.Object, itemRepoMock.Object, unitOfWorkMock);

            var request = new AddSale.Request(
                BillingPartyId: billingParty.Id.ToString(),
                Date: "2024-01-01",
                SaleLines: new List<AddSale.SaleLines> {
                    new AddSale.SaleLines(ItemId: item.Id.ToString(), Quantity: 1, UnitPrice: 10, Report: 1)
                },
                Remarks: "remarks",
                VatAmount: 3,
                TransportFee: 2,
                ReceivedAmount: 5,
                InvoiceNumber: 2
            );

            // Act
            var response = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.NotNull(response);
            Assert.False(string.IsNullOrEmpty(response.SaleId));
            Assert.True(Guid.TryParse(response.SaleId, out _));
            saleRepoMock.Verify(repo => repo.AddAsync(It.IsAny<SaleEntity>()), Times.Once);
            // 50+9 = 59 (50 is the initial balance, 9 is the total amount of the sale after subtracting report and  received amount)
            Assert.Equal(59, billingParty.Balance);

            // 100 - 1 = 99 (100 is the initial stock amount, 1 is the quantity of the sale)
            Assert.Equal(99, item.CurrentStockAmount);
            Assert.Equal(1, unitOfWorkMock.CallCount);
        }
    }
}
