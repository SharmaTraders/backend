using Application.CommandHandlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryContracts.reports;
using Tools;

namespace Query.QueryHandlers.reports;

public class AllTransactionsReportHandler : IRequestHandler<AllTransactionsReport.Query, AllTransactionsReport.Answer> {
    private readonly SharmaTradersContext _context;

    public AllTransactionsReportHandler(SharmaTradersContext context) {
        _context = context;
    }


    public async Task<AllTransactionsReport.Answer> Handle(AllTransactionsReport.Query request,
        CancellationToken cancellationToken) {
        DateOnly dateFrom = DateParser.ParseDate(request.DateFrom);
        DateOnly dateTo = DateParser.ParseDate(request.DateTo);

        if (dateFrom > dateTo) {
            throw new DomainValidationException("dates", ErrorCode.BadRequest,
                ErrorMessages.ReportFromDateBeforeToDate);
        }

        var expenses =await GetExpensesAsync(dateFrom, dateTo, cancellationToken);
        var incomes = await GetIncomeAsync(dateFrom, dateTo, cancellationToken);
        var sales =await GetSalesAsync(dateFrom, dateTo, cancellationToken);
        var purchases =await GetPurchasesAsync(dateFrom, dateTo, cancellationToken);

        var allTransactions = expenses
            .Concat(incomes)
            .Concat(sales)
            .Concat(purchases)
            .OrderByDescending(transaction => DateTime.Parse(transaction.Date))
            .ToList();

        return new AllTransactionsReport.Answer(allTransactions);
    }

    private Task<AllTransactionsReport.TransactionDto[]> GetExpensesAsync(DateOnly from, DateOnly to,
        CancellationToken cancellationToken) {
        return _context.Expenses
            .AsNoTracking()
            .Where(expense => expense.Date >= from && expense.Date <= to)
            .Select(expense => new AllTransactionsReport.TransactionDto(
                expense.Date.ToString(),
                expense.Remarks,
                expense.CategoryName,
                expense.Amount,
                "Expense"))
            .ToArrayAsync(cancellationToken);
    }

    private Task<AllTransactionsReport.TransactionDto[]> GetIncomeAsync(DateOnly from, DateOnly to,
        CancellationToken cancellationToken) {
        return _context.Incomes
            .AsNoTracking()
            .Include(income => income.BillingParty)
            .Where(income => income.Date >= from && income.Date <= to)
            .Select(income => new AllTransactionsReport.TransactionDto(
                income.Date.ToString(),
                income.Remarks,
                "From Party: " + income.BillingParty.Name,
                income.Amount,
                "Income"))
            .ToArrayAsync(cancellationToken);
    }

    private Task<AllTransactionsReport.TransactionDto[]> GetPurchasesAsync(DateOnly from, DateOnly to,
        CancellationToken cancellationToken) {
        return _context.Purchases
            .AsNoTracking()
            .Include(purchase => purchase.BillingParty)
            .Where(purchase => purchase.Date >= from && purchase.Date <= to)
            .Select(purchase => new AllTransactionsReport.TransactionDto(
                purchase.Date.ToString(),
                purchase.Remarks,
                "Bought from: " + purchase.BillingParty.Name,
                purchase.PaidAmount ?? 0,
                "Purchase"))
            .ToArrayAsync(cancellationToken);
    }

    private Task<AllTransactionsReport.TransactionDto[]> GetSalesAsync(DateOnly from, DateOnly to,
        CancellationToken cancellationToken) {
        return _context.Sales
            .AsNoTracking()
            .Include(sale => sale.BillingParty)
            .Where(sale => sale.Date >= from && sale.Date <= to)
            .Select(sale => new AllTransactionsReport.TransactionDto(
                sale.Date.ToString(),
                sale.Remarks,
                "Sold to: " + sale.BillingParty.Name,
                sale.ReceivedAmount ?? 0,
                "Sales"))
            .ToArrayAsync(cancellationToken);
    }
}