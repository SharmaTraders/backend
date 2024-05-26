using Application.CommandHandlers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryContracts.reports;
using Tools;

namespace Query.QueryHandlers.reports;

public class
    ExpenseByCategoryReportHandler : IRequestHandler<ExpenseByCategoryReport.Query, ExpenseByCategoryReport.Answer> {
    private readonly SharmaTradersContext _context;

    public ExpenseByCategoryReportHandler(SharmaTradersContext context) {
        _context = context;
    }


    public async Task<ExpenseByCategoryReport.Answer> Handle(ExpenseByCategoryReport.Query request,
        CancellationToken cancellationToken) {
        var query = _context.Expenses
            .AsNoTracking()
            .AsQueryable();

        if (request.DateFrom is not null) {
            DateOnly dateFrom = DateParser.ParseDate(request.DateFrom);
            // If dateTo is not specified, set it to the maximum value
            DateOnly dateTo = request.DateTo is not null ? DateParser.ParseDate(request.DateTo) : DateOnly.MaxValue;

            if (dateFrom > dateTo) {
                throw new DomainValidationException("dates", ErrorCode.BadRequest,
                    ErrorMessages.FromDateBeforeToDate);
            }

            query = query.Where(expense => expense.Date >= dateFrom && expense.Date <= dateTo);
        }

        var expenseByCategory = await query
            .GroupBy(expense => expense.CategoryName)
            .Select(group => new ExpenseByCategoryReport.ExpenseByCategoryDto(
                group.Key,
                Math.Round(group.Sum(expense => expense.Amount))
            ))
            .ToListAsync(cancellationToken);

        return new ExpenseByCategoryReport.Answer(expenseByCategory);
    }
}