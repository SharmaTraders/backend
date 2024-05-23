using System.Reflection;
using Domain.common;

namespace Domain.Entity
{
    public class EmployeeStatusCategory : Enumeration
    {
        public static readonly EmployeeStatusCategory Active = new EmployeeStatusCategory(1, "Active");
        public static readonly EmployeeStatusCategory Inactive = new EmployeeStatusCategory(2, "Inactive");

        private EmployeeStatusCategory() { }

        private EmployeeStatusCategory(int id, string name) : base(id, name) { }

        public static IEnumerable<EmployeeStatusCategory> GetAll()
        {
            var fields =
                typeof(EmployeeStatusCategory).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                var instance = new EmployeeStatusCategory();

                if (info.GetValue(instance) is EmployeeStatusCategory locatedValue)
                {
                    yield return locatedValue;
                }
            }
        }
    }
}