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
using Database.Models;

namespace PetLog
{
    /// <summary>
    /// Interaction logic for AnimalsWindow.xaml
    /// </summary>
    public partial class AnimalsWindow : Window
    {
        public User User { get; }
        public AnimalsManager AnimalsManager { get; set; }
        public AnimalsWindow(User user)
        {
            InitializeComponent();
            User = user;
            AnimalsManager = new AnimalsManager();
            AnimalsGrid.ItemsSource = AnimalsManager.Load();

            MessageBox.Show($"Witaj {User.Name} {User.Surname}", "Powodzenie", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (User.IsAdmin)
            {
                UsersButton.Visibility = Visibility.Visible;
            }
            else
            {
                UsersButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
