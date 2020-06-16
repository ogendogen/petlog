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
    /// Interaction logic for UsersWindow.xaml - window with users
    /// </summary>
    public partial class UsersWindow : Window
    {
        /// <summary>
        /// Users manager instance
        /// </summary>
        public UsersManager UsersManager { get; set; }
        /// <summary>
        /// Users window constructor - applies users data to datagrid
        /// </summary>
        public UsersWindow()
        {
            InitializeComponent();
            UsersManager = new UsersManager();
            UsersDataGrid.ItemsSource = UsersManager.GetAllUsers();
        }

        /// <summary>
        /// Save button clicked event - hashes plain passwords and save changes
        /// </summary>
        /// <param name="sender">Clicked save button</param>
        /// <param name="e">Event arguments</param>
        private void SaveUsersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UsersManager.SaveChanges();
                UsersManager.HashPlainPasswords();

                MessageBox.Show("Zmiany dokonane poprawnie!", "Powodzenie", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Cancel button clicked event - rollback changes
        /// </summary>
        /// <param name="sender">Clicked cancel button</param>
        /// <param name="e">Event arguments</param>
        private void CancelUsersButton_Click(object sender, RoutedEventArgs e)
        {
            UsersManager.RollBack();
            MessageBox.Show("Zmiany wycofane!", "Powodzenie", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            Close();
        }
    }
}
