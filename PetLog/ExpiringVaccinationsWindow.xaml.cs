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
    /// Interaction logic for ExpiringVaccinationsWindow.xaml
    /// </summary>
    public partial class ExpiringVaccinationsWindow : Window
    {
        public ExpiringVaccinationsManager Manager { get; set; }
        public ExpiringVaccinationsWindow(ExpiringVaccinationsManager manager)
        {
            InitializeComponent();
            Manager = manager;
            ExpiringVaccinationsDataGrid.ItemsSource = Manager.GetExpiringVaccinations();
            ExpiringVaccinationsDataGrid.DataContext = Manager.GetExpiringVaccinations();
        }
    }
}
