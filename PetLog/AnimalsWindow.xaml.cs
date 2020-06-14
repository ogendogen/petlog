using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AnimalsWindow.xaml - main window with animals datagrid
    /// </summary>
    public partial class AnimalsWindow : Window
    {
        /// <summary>
        /// Logged in user passed from LoginWindow
        /// </summary>
        public User User { get; }
        /// <summary>
        /// Animals manager
        /// </summary>
        public AnimalsManager AnimalsManager { get; set; }
        /// <summary>
        /// Collection of all animals
        /// </summary>
        public ObservableCollection<Animal> Animals { get; set; }
        /// <summary>
        /// Expiring vaccinations manager
        /// </summary>
        public ExpiringVaccinationsManager ExpiringVaccinationsManager { get; set; }
        
        /// <summary>
        /// Animals window constructor - initialize managers and loads animals to datagrid
        /// </summary>
        /// <param name="user">Entity of logged in user</param>
        public AnimalsWindow(User user)
        {
            InitializeComponent();
            User = user;
            AnimalsManager = new AnimalsManager();
            ExpiringVaccinationsManager = new ExpiringVaccinationsManager();
            Animals = AnimalsManager.Load();
            AnimalsGrid.ItemsSource = Animals;

            MessageBox.Show($"Witaj {User.Name} {User.Surname}", "Powodzenie", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Loaded window event, fires when window is loaded. Fits buttons to the user and content
        /// </summary>
        /// <param name="sender">Loaded window object</param>
        /// <param name="e">Event arguments</param>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int expiringVaccinationsCount = ExpiringVaccinationsManager.Count();
            if (User.IsAdmin && expiringVaccinationsCount > 0)
            {
                UsersButton.Visibility = Visibility.Visible;
                ExpiringVaccinationsButton.Visibility = Visibility.Visible;
                ExpiringVaccinationsButton.Content += $" ({expiringVaccinationsCount})";
            }
            else if (!User.IsAdmin && expiringVaccinationsCount > 0)
            {
                UsersButton.Visibility = Visibility.Collapsed;
                ExpiringVaccinationsButton.Visibility = Visibility.Visible;
                ExpiringVaccinationsButton.Content += $" ({expiringVaccinationsCount})";
                ExpiringVaccinationsButton.Margin = new Thickness(ExpiringVaccinationsButton.Margin.Left - 240, ExpiringVaccinationsButton.Margin.Top, ExpiringVaccinationsButton.Margin.Right, ExpiringVaccinationsButton.Margin.Bottom);
            }
            else if (User.IsAdmin && expiringVaccinationsCount == 0)
            {
                UsersButton.Visibility = Visibility.Visible;
                ExpiringVaccinationsButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                UsersButton.Visibility = Visibility.Collapsed;
                ExpiringVaccinationsButton.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Add button click event handler
        /// </summary>
        /// <param name="sender">Clicked button object</param>
        /// <param name="e">Event arguments</param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DetailsWindow detailsWindow = new DetailsWindow(AnimalsManager);
            detailsWindow.ShowDialog();
        }

        /// <summary>
        /// Edit button click event handler
        /// </summary>
        /// <param name="sender">Clicked button object</param>
        /// <param name="e">Event arguments</param>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalsGrid.SelectedItem != null && !AnimalsGrid.SelectedItem.ToString().Contains("Placeholder"))
            {
                Animal animal = (Animal)AnimalsGrid.SelectedItem;
                DetailsWindow detailsWindow = new DetailsWindow(AnimalsManager, animal);
                detailsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Najpierw wybierz zwierzę do edycji!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Delete button click event handler
        /// </summary>
        /// <param name="sender">Clicked button object</param>
        /// <param name="e">Event arguments</param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (AnimalsGrid.SelectedItem != null && !AnimalsGrid.SelectedItem.ToString().Contains("Placeholder"))
            {
                Animal animal = (Animal)AnimalsGrid.SelectedItem;
                if (MessageBox.Show("Czy na pewno chcesz usunąć to zwierzę z bazy? Operacja jest NIEODWRACALNA!", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    AnimalsManager.RemoveAnimal(animal);
                    AnimalsManager.SaveChanges();
                    MessageBox.Show("Pomyślnie usunięto zwierze", "Powodzenie", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Najpierw wybierz zwierzę do usunięcia!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Users button click event handler
        /// </summary>
        /// <param name="sender">Clicked button object</param>
        /// <param name="e">Event arguments</param>
        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            UsersWindow usersWindow = new UsersWindow();
            usersWindow.Show();
        }

        /// <summary>
        /// Expiring vaccinations button click event handler
        /// </summary>
        /// <param name="sender">Clicked button object</param>
        /// <param name="e">Event arguments</param>
        private void ExpiringVaccinationsButton_Click(object sender, RoutedEventArgs e)
        {
            ExpiringVaccinationsWindow expiringVaccinationsWindow = new ExpiringVaccinationsWindow(ExpiringVaccinationsManager);
            expiringVaccinationsWindow.Show();
        }
    }
}
