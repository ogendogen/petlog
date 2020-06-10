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
    /// Interaction logic for UsersWindow.xaml
    /// </summary>
    public partial class UsersWindow : Window
    {
        public UsersManager UsersManager { get; set; }
        public UsersWindow(UsersManager usersManager)
        {
            InitializeComponent();
            UsersManager = usersManager;
            UsersDataGrid.ItemsSource = UsersManager.GetAllUsers();
        }

        private void SaveUsersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UsersManager.HashPasswords();
                UsersManager.SaveChanges();

                MessageBox.Show("Zmiany dokonane poprawnie!", "Powodzenie", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
