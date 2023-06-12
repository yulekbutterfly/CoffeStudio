using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeStudio.EF
{
    public partial class Employee
    {
        public string FullName { get => $"{FirstName} {LastName} {Patronymic}"; }
    }
    public partial class Order
    {
        public decimal TotalPrice { get => OrderDish.Where(i=> i.IDOrder==IDOrder).Sum(i=> i.TotalPrice); }
    }
}
