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
    /// Interaction logic for AnimalsWindow.xaml
    /// </summary>
    public partial class AnimalsWindow : Window
    {
        public User User { get; }
        public UsersManager UsersManager { get; }
        public AnimalsManager AnimalsManager { get; set; }
        public ObservableCollection<Animal> Animals { get; set; }
        public ExpiringVaccinationsManager ExpiringVaccinationsManager { get; set; }
        public AnimalsWindow(User user, UsersManager usersManager)
        {
            InitializeComponent();
            User = user;
            UsersManager = usersManager;
            AnimalsManager = new AnimalsManager();
            ExpiringVaccinationsManager = new ExpiringVaccinationsManager();
            Animals = AnimalsManager.Load();
            AnimalsGrid.ItemsSource = Animals;

            MessageBox.Show($"Witaj {User.Name} {User.Surname}", "Powodzenie", MessageBoxButton.OK, MessageBoxImage.Information);
        }

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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            DetailsWindow detailsWindow = new DetailsWindow(AnimalsManager);
            detailsWindow.ShowDialog();
        }

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

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            UsersWindow usersWindow = new UsersWindow(UsersManager);
            usersWindow.Show();
        }

        private void ExpiringVaccinationsButton_Click(object sender, RoutedEventArgs e)
        {
            ExpiringVaccinationsWindow expiringVaccinationsWindow = new ExpiringVaccinationsWindow(ExpiringVaccinationsManager);
            expiringVaccinationsWindow.Show();
        }
    }
}
