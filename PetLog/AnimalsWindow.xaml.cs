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
            MessageBox.Show("Tutaj powstanie okno z użytkownikami :)", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
