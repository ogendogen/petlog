﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        private Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        private Regex postalCodeRegex = new Regex(@"\d{2}-\d{3}");

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
            Animal = new Animal()
            {
                BirthDate = new DateTime(DateTime.Now.Year, 1, 1),
                JoinDate = DateTime.Today
            };

            Mode = Mode.Add;
            SaveButton.Content = "Dodaj";

            VaccinationsDataGrid.ItemsSource = new ObservableCollection<Vaccination>();
        }

        private void AnimalChipTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            AllowOnlyDigits(e);
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

            AnimalBirthDateDatePicker.DisplayDateStart = DateTime.MinValue;
            AnimalBirthDateDatePicker.DisplayDateEnd = DateTime.Today;

            AnimalJoinDateDatePicker.DisplayDateStart = DateTime.MinValue;
            AnimalJoinDateDatePicker.DisplayDateEnd = DateTime.Today;

            AnimalDeathDateDatePicker.DisplayDateStart = DateTime.MinValue;
            AnimalDeathDateDatePicker.DisplayDateEnd = DateTime.Today;

            AnimalLostDateDatePicker.DisplayDateStart = DateTime.MinValue;
            AnimalLostDateDatePicker.DisplayDateEnd = DateTime.Today;

            // TODO: add ranges for date picker in vaccination datagrid
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
                    if (IsAliveCheckbox.IsChecked.Value)
                    {
                        if (!AnimalDeathDateDatePicker.SelectedDate.HasValue)
                        {
                            MessageBox.Show("Data śmierci nie jest wybrana!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        Animal.DeathInfo = new Death()
                        {
                            Date = AnimalDeathDateDatePicker.SelectedDate.Value,
                            Description = AnimalDeathDescriptionTextBox.Text
                        };
                    }

                    if (IsLostCheckbox.IsChecked.Value)
                    {
                        if (!AnimalLostDateDatePicker.SelectedDate.HasValue)
                        {
                            MessageBox.Show("Data ucieczki nie jest wybrana!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        Animal.LostInfo = new Lost()
                        {
                            Date = AnimalLostDateDatePicker.SelectedDate.Value,
                            Description = AnimalLostDescriptionTextBox.Text
                        };
                    }

                    if (AdoptiveTelephoneTextBox.Text.Any(c => !Char.IsDigit(c)))
                    {
                        MessageBox.Show("Numer telefonu zawiera inne znaki niż cyfry!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    int? flatNumber = null;
                    string s_flatNumber = AdoptiveFlatNumberTextBox.Text;
                    if (!String.IsNullOrEmpty(s_flatNumber))
                    {
                        if (s_flatNumber.Any(c => !Char.IsDigit(c)))
                        {
                            MessageBox.Show("Numer klatki zawiera inne znaki niż cyfry!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        else
                        {
                            flatNumber = Int32.Parse(s_flatNumber);
                        }
                    }

                    if (IsAdoptedCheckbox.IsChecked.Value && Animal.Adoptive == null)
                    {
                        Animal.Adoptive = new Adoptive()
                        {
                            Name = AdoptiveNameTextBox.Text,
                            Surname = AdoptiveSurnameTextBox.Text,
                            Email = AdoptiveEmailTextBox.Text,
                            Telephone = Int32.Parse(AdoptiveTelephoneTextBox.Text),
                            City = AdoptiveCityTextBox.Text,
                            Street = AdoptiveStreetTextBox.Text,
                            PostalCode = AdoptivePostalCodeTextBox.Text,
                            HouseNumber = Int32.Parse(AdoptiveHouseNumberTextBox.Text),
                            FlatNumber = flatNumber,
                        };
                    }

                    HashSet<Vaccination> vaccinations = new HashSet<Vaccination>();
                    vaccinations = vaccinations.Concat(VaccinationsDataGrid.Items.OfType<Vaccination>()).ToHashSet();
                    Animal.Vaccinations = vaccinations;

                    var addedAnimal = AnimalsManager.AddNewAnimal(Animal.Name,
                            Animal.Type,
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
                    MessageBox.Show("Nowe zwierze dodane!", "Powodzenie", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AdoptiveEmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (!emailRegex.IsMatch(input))
            {
                MessageBox.Show("Niepoprawny adres email!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void AdoptiveTelephoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            AllowOnlyDigits(e);
        }

        private static void AllowOnlyDigits(TextCompositionEventArgs e)
        {
            string input = e.Text;
            e.Handled = !input.All(x => Char.IsDigit(x));
        }

        private void AdoptivePostalCodeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (!postalCodeRegex.IsMatch(input))
            {
                MessageBox.Show("Niepoprawny format kodu pocztowego!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void AdoptiveHouseNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            AllowOnlyDigits(e);
        }

        private void AdoptiveFlatNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            AllowOnlyDigits(e);
        }

        private void IsAliveCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (AnimalDeathDateDatePicker.SelectedDate == null)
            {
                AnimalDeathDateDatePicker.SelectedDate = DateTime.Today;
            }
        }

        private void IsLostCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (AnimalLostDateDatePicker.SelectedDate == null)
            {
                AnimalLostDateDatePicker.SelectedDate = DateTime.Today;
            }
        }
    }
}
