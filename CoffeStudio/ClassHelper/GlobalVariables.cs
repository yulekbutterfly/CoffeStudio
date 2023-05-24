using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CoffeStudio.ClassHelper
{
    internal class GlobalVariables
    {
        public class preOrder : EF.Dish
        {

            public int qty { get; set; }
            public decimal sum
            {
                get { return Cost * qty; }

            }
        }
        public static List<preOrder> preOrderList = new List<preOrder>();

        public static EF.Client GlobalClient = new EF.Client();

        public static int useBonuses = -1;

        public static EF.Employee UsedEmployee = new EF.Employee();
    }

}
