using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSharingApp.Views.Interfaces
{
    public interface IAddEditWindow
    {
        void Show();
        bool? ShowDialog();
        bool? DialogResult { get; set; }
        void Close();
    }
}
