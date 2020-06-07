using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using Microsoft.EntityFrameworkCore.Internal;

namespace PetLog
{
    /// <summary>
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        public Mode Mode { get; set; }
        public AnimalsManager AnimalsManager { get; set; }
        public Animal Animal { get; set; }
        public ObservableCollection<Animal> Animals { get; set; }
        public DetailsWindow(AnimalsManager animalsManager, Animal animal)
        {
            AnimalsManager = animalsManager;
            Animal = animal;

            Mode = Mode.Edit;

            InitializeComponent();

            SaveButton.Content = "Zapisz zmiany";
            VaccinationsDataGrid.ItemsSource = new ObservableCollection<Vaccination>(AnimalsManager.GetAnimalVaccinations().Where(vacc => vacc.Animal.ID == Animal.ID));
        }

        public DetailsWindow(AnimalsManager animalsManager)
        {
            InitializeComponent();
            AnimalsManager = animalsManager;
            Animal = new Animal();

            Mode = Mode.Add;
            SaveButton.Content = "Dodaj";
        }

        private void AnimalChipTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string input = sender.ToString();
            e.Handled = input.All(x => Char.IsDigit(x));
        }

        private void InformationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AnimalTypeComboBox.ItemsSource = Enum.GetValues(typeof(AnimalType)).Cast<AnimalType>();
            
            AnimalTab.DataContext = Animal;
            AdoptiveTab.DataContext = Animal.Adoptive;
            DeathTab.DataContext = Animal.DeathInfo;
            LostTab.DataContext = Animal.LostInfo;
            VaccinationTab.DataContext = new Vaccination();

            IsAliveCheckbox.IsChecked = (Animal.DeathInfo != null);
            IsLostCheckbox.IsChecked = (Animal.LostInfo != null);
            IsAdoptedCheckbox.IsChecked = (Animal.Adoptive != null);

            AdoptivesComboBox.ItemsSource = new ObservableCollection<Adoptive>(AnimalsManager.GetAllAdoptivesInAlphabeticalOrder());
            AdoptivesComboBox.SelectedItem = Animal.Adoptive;

            List<Animal> thisAnimalList = new List<Animal>
            {
                Animal
            };
            AnimalsComboBox.ItemsSource = new ObservableCollection<Animal>(thisAnimalList);
        }

        private void AdoptivesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            if (comboBox.SelectedItem is Adoptive adoptive)
            {
                Animal.Adoptive = adoptive;
                AdoptiveTab.DataContext = adoptive;
                IsAdoptedCheckbox.IsChecked = true;
                DiscardChoosenAdoptive.IsEnabled = true;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Mode == Mode.Add)
            {
                try
                {
                    AnimalsManager.AddNewAnimal(Animal.Type,
                            Animal.BirthDate,
                            Animal.JoinDate,
                            Animal.Vaccinations,
                            Animal.Chip,
                            Animal.Description,
                            Animal.State,
                            Animal.Treatments,
                            Animal.Adoptive,
                            Animal.DeathInfo,
                            Animal.LostInfo);
                    AnimalsManager.SaveChanges();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DiscardChoosenAdoptive_Click(object sender, RoutedEventArgs e)
        {
            IsAdoptedCheckbox.IsChecked = false;
            DiscardChoosenAdoptive.IsEnabled = false;

            Adoptive adoptive = new Adoptive();
            Animal.Adoptive = adoptive;
            AdoptiveTab.DataContext = adoptive;

            AdoptivesComboBox.SelectedItem = null;
        }
    }
}
