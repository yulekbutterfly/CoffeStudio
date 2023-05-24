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
}
