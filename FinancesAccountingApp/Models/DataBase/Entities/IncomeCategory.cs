using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancesAccountingApp.Models.DataBase.Entities
{
    public class IncomeCategory : Entity
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }


}
