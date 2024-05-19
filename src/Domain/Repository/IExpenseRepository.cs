using Domain.Entity;

namespace Domain.Repository;

public interface IExpenseRepository:  IRepository<ExpenseEntity, Guid>{
    
}