using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancesAccountingApp.Models.DataBase.Entities
{
    public class Currency : Entity
    {
        public string Name { get; set; }
        public string Scale { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

}
