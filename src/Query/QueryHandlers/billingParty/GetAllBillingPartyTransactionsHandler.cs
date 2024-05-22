using Application.CommandHandlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryContracts.billingParty;

namespace Query.QueryHandlers.billingParty;

public class GetAllBillingPartyTransactionsHandler : IRequestHandler<GetAllBillingPartyTransaction.Query, GetAllBillingPartyTransaction.Answer> {
    private readonly SharmaTradersContext _context;

    public GetAllBillingPartyTransactionsHandler(SharmaTradersContext context) {
        _context = context;
    }


    public async Task<GetAllBillingPartyTransaction.Answer> Handle(GetAllBillingPartyTransaction.Query request, CancellationToken cancellationToken) {
       Guid id = GuidParser.ParseGuid(request.BillingPartyId, "BillingPartyId");
       var expenses = await GetExpensesAsync(id, request.PageNumber, request.PageSize, cancellationToken);
       var incomes = await GetIncomeAsync(id, request.PageNumber, request.PageSize, cancellationToken);
       var sales = await GetSalesAsync(id, request.PageNumber, request.PageSize, cancellationToken);
       var purchases = await GetPurchasesAsync(id, request.PageNumber, request.PageSize, cancellationToken);

       // Merge all transactions and order them by date
       var allTransactions = expenses.Item1
           .Concat(incomes.Item1)
           .Concat(sales.Item1)
           .Concat(purchases.Item1)
           .OrderByDescending(transaction => DateTime.Parse(transaction.Date))
           .Take(request.PageSize) // Take only the required number of items
           .ToList();

       int totalCount = expenses.Item2 + incomes.Item2 + sales.Item2 + purchases.Item2;

       return new GetAllBillingPartyTransaction.Answer(allTransactions, totalCount, request.PageNumber, request.PageSize);
       
    }

private async Task<(GetAllBillingPartyTransaction.TransactionDto[], int)> GetExpensesAsync(Guid billingPartyId, int pageNumber, int pageSize, CancellationToken cancellationToken) {

    var queryable = _context.Expenses
        .AsNoTracking()
        .Where(expense => expense.BillingPartyId == billingPartyId);

    int totalCount = await queryable.CountAsync(cancellationToken);

     var allExpenses =await queryable
        .OrderByDescending(expense => expense.Date)
        .Skip((pageNumber - 1) * pageSize) // Skip to the start of the current page
        .Take(pageSize) // Take only the required number of items for the current page
        .Select(expense => new GetAllBillingPartyTransaction.TransactionDto(
            expense.Date.ToString(),
            expense.Remarks,
            expense.CategoryName,
            expense.Amount,
            "Expense"))
        .ToArrayAsync(cancellationToken);

     return (allExpenses, totalCount);
}

private async Task<(GetAllBillingPartyTransaction.TransactionDto[], int)> GetIncomeAsync(Guid billingPartyId, int pageNumber, int pageSize, CancellationToken cancellationToken) {
    var queryable = _context.Incomes
        .AsNoTracking()
        .Where(income => income.BillingPartyId == billingPartyId);

    int totalCount = await queryable.CountAsync(cancellationToken);

    var allIncome = await queryable
        .OrderByDescending(income => income.Date)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(income => new GetAllBillingPartyTransaction.TransactionDto(
            income.Date.ToString(),
            income.Remarks,
            "From Party: " + income.BillingParty.Name,
            income.Amount,
            "Income"))
        .ToArrayAsync(cancellationToken);

    return (allIncome, totalCount);
}

private async Task<(GetAllBillingPartyTransaction.TransactionDto[], int)> GetPurchasesAsync(Guid billingPartyId, int pageNumber, int pageSize, CancellationToken cancellationToken) {
    var queryable = _context.Purchases
        .AsNoTracking()
        .Where(purchase => purchase.BillingPartyId == billingPartyId);

    int totalCount = await queryable.CountAsync(cancellationToken);

    var allPurchases = await queryable
        .OrderByDescending(purchase => purchase.Date)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(purchase => new GetAllBillingPartyTransaction.TransactionDto(
            purchase.Date.ToString(),
            purchase.Remarks,
            "Bought from: " + purchase.BillingParty.Name,
            purchase.PaidAmount ?? 0,
            "Purchase"))
        .ToArrayAsync(cancellationToken);

    return (allPurchases, totalCount);
}

private async Task<(GetAllBillingPartyTransaction.TransactionDto[], int)> GetSalesAsync(Guid billingPartyId, int pageNumber, int pageSize, CancellationToken cancellationToken) {
    var queryable = _context.Sales
        .AsNoTracking()
        .Where(sale => sale.BillingPartyId == billingPartyId);

    int totalCount = await queryable.CountAsync(cancellationToken);

    var allSales = await queryable
        .OrderByDescending(sale => sale.Date)
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(sale => new GetAllBillingPartyTransaction.TransactionDto(
            sale.Date.ToString(),
            sale.Remarks,
            "Sold to: " + sale.BillingParty.Name,
            sale.ReceivedAmount ?? 0,
            "Sales"))
        .ToArrayAsync(cancellationToken);

    return (allSales, totalCount);
}

}