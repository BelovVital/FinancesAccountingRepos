using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancesAccountingApp.Models.DataBase.Entities
{
    public class Expense : Entity
    {
        public double Summa { get; set; }
        public string Currency { get; set; }
        public string Category { get; set; }
        public string Source { get; set; }
        public DateTime Date { get; set; }
        public Guid WalletId { get; set; }
        [ForeignKey("WalletId")]
        public Wallet Wallet { get; set; }
    }
}
