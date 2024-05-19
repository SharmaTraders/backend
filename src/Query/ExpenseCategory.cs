using System;
using System.Collections.Generic;

namespace Query;

public partial class ExpenseCategory
{
    public string Name { get; set; } = null!;

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
