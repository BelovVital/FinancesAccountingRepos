using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancesAccountingApp.Models.DataBase.Entities
{
    public class Wallet : Entity
    {
        public string Name { get; set; }
        public double Summa { get; set; }
        public string Currency { get; set; }
        public DateTime Date { get; set; }
    }
}
