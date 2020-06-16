using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Database;

namespace PetLog
{
    /// <summary>
    /// Interaction logic for ExpiringVaccinationsWindow.xaml - window with expiring vaccinations
    /// </summary>
    public partial class ExpiringVaccinationsWindow : Window
    {
        /// <summary>
        /// Expiring vaccinations manager
        /// </summary>
        public ExpiringVaccinationsManager Manager { get; set; }
        /// <summary>
        /// Expiring vaccinations window - initialize data from manager
        /// </summary>
        /// <param name="manager">Expiring vaccinations manager</param>
        public ExpiringVaccinationsWindow(ExpiringVaccinationsManager manager)
        {
            InitializeComponent();
            Manager = manager;
            ExpiringVaccinationsDataGrid.ItemsSource = Manager.GetExpiringVaccinations();
            ExpiringVaccinationsDataGrid.DataContext = Manager.GetExpiringVaccinations();
        }
    }
}
