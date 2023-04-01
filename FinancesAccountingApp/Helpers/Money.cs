using FinancesAccounting.Models.DataBase;
using FinancesAccountingApp.Models.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancesAccountingApp.Helpers
{
    public class Money
    {
        public static double ConvertToWalletCurrency(Wallet wallet, double summa, string currency)
        {
            if (wallet.Currency == currency)
                return summa;

            using (var dbContext = new AppDbContext())
            {
                var currensies = dbContext.Currencies;
                var Currencies = new ObservableCollection<Currency>(currensies);

                var walletCurrencyScale = Currencies.First(x => x.Name == wallet.Currency).Scale;
                var currentCurrencyScale = currensies.First(x => x.Name == currency).Scale;

                double convertSumma = summa * currentCurrencyScale / walletCurrencyScale;
                return convertSumma;
            }
        }
    }
}
