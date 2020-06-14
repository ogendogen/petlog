using System;
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
    /// Interaction logic for DetailsWindow.xaml - window with details of certain animal
    /// </summary>
    public partial class DetailsWindow : Window
    {
        /// <summary>
        /// Add or edit mode
        /// </summary>
        public Mode Mode { get; set; }
        /// <summary>
        /// Animals manager instance
        /// </summary>
        public AnimalsManager AnimalsManager { get; set; }
        /// <summary>
        /// Choosen animal or empty animal instance if adding new
        /// </summary>
        public Animal Animal { get; set; }

        /// <summary>
        /// Email regular expression
        /// </summary>
        private readonly Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        /// <summary>
        /// Postal code regular expression
        /// </summary>
        private readonly Regex postalCodeRegex = new Regex(@"\d{2}-\d{3}");

        /// <summary>
        /// Details window constructor for edit mode
        /// </summary>
        /// <param name="animalsManager">Animals manager object</param>
        /// <param name="animal">Animal entity choosen to edit</param>
        public DetailsWindow(AnimalsManager animalsManager, Animal animal)
        {
            AnimalsManager = animalsManager;
            Animal = animal;

            Mode = Mode.Edit;

            InitializeComponent();

            SaveButton.Content = "Zapisz zmiany";
            VaccinationsDataGrid.ItemsSource = new ObservableCollection<Vaccination>(AnimalsManager.GetAnimalVaccinations().Where(vacc => vacc.Animal.ID == Animal.ID));
        }

        /// <summary>
        /// Details window constructor for add mode
        /// </summary>
        /// <param name="animalsManager">Animals manager object</param>
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

        /// <summary>
        /// Preview animal's chip input - allow only digits
        /// </summary>
        /// <param name="sender">Input textbox object</param>
        /// <param name="e">Event arguments</param>
        private void AnimalChipTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            AllowOnlyDigits(e);
        }

        /// <summary>
        /// Details windows loaded event - prepare data and components
        /// </summary>
        /// <param name="sender">Window object</param>
        /// <param name="e">Event arguments</param>
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

            AnimalBirthDateDatePicker.DisplayDateStart = DateTime.MinValue;
            AnimalBirthDateDatePicker.DisplayDateEnd = DateTime.Today;

            AnimalJoinDateDatePicker.DisplayDateStart = DateTime.MinValue;
            AnimalJoinDateDatePicker.DisplayDateEnd = DateTime.Today;

            AnimalDeathDateDatePicker.DisplayDateStart = DateTime.MinValue;
            AnimalDeathDateDatePicker.DisplayDateEnd = DateTime.Today;

            AnimalLostDateDatePicker.DisplayDateStart = DateTime.MinValue;
            AnimalLostDateDatePicker.DisplayDateEnd = DateTime.Today;
        }

        /// <summary>
        /// Adoptives combobox selected event - if adoptive choosen then apply data
        /// </summary>
        /// <param name="sender">Combobox object</param>
        /// <param name="e">Event arguments</param>
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

        /// <summary>
        /// Saves changes click event - does validations for add/edit mode
        /// </summary>
        /// <param name="sender">Clicked button object</param>
        /// <param name="e">Event arguments</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Mode == Mode.Add)
            {
                try
                {
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

                    Animal.Vaccinations = GetVaccinations();
                    VerifyDeath();
                    VerifyLost();

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
                    MessageBox.Show("Nowe zwierze dodane!", "Powodzenie", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (Mode == Mode.Edit)
            {
                try
                {
                    Animal.Vaccinations = GetVaccinations();
                    VerifyDeath();
                    VerifyLost();

                    AnimalsManager.UpdateAnimal(Animal);
                    MessageBox.Show("Edycja powiodła się!", "Powodzenie", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Validate lost info data
        /// </summary>
        private void VerifyLost()
        {
            if (IsLostCheckbox.IsChecked.Value)
            {
                if (!AnimalLostDateDatePicker.SelectedDate.HasValue)
                {
                    MessageBox.Show("Data ucieczki nie jest wybrana!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Animal.LostInfo == null)
                {
                    Animal.LostInfo = new Lost()
                    {
                        Date = AnimalLostDateDatePicker.SelectedDate.Value,
                        Description = AnimalLostDescriptionTextBox.Text
                    };
                }
                else
                {
                    Animal.LostInfo.Date = AnimalLostDateDatePicker.SelectedDate.Value;
                    Animal.LostInfo.Description = AnimalLostDescriptionTextBox.Text;
                }
            }
            else if (!IsLostCheckbox.IsChecked.Value && Animal.LostInfo != null)
            {
                AnimalsManager.RemoveLost(Animal.LostInfo);
            }
        }

        /// <summary>
        /// Validate death info data
        /// </summary>
        private void VerifyDeath()
        {
            if (IsAliveCheckbox.IsChecked.Value)
            {
                if (!AnimalDeathDateDatePicker.SelectedDate.HasValue)
                {
                    MessageBox.Show("Data śmierci nie jest wybrana!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Animal.DeathInfo == null)
                {
                    Animal.DeathInfo = new Death()
                    {
                        Date = AnimalDeathDateDatePicker.SelectedDate.Value,
                        Description = AnimalDeathDescriptionTextBox.Text
                    };
                }
                else
                {
                    Animal.DeathInfo.Date = AnimalDeathDateDatePicker.SelectedDate.Value;
                    Animal.DeathInfo.Description = AnimalDeathDescriptionTextBox.Text;
                }
            }
            else if (!IsAliveCheckbox.IsChecked.Value && Animal.DeathInfo != null)
            {
                AnimalsManager.RemoveDeath(Animal.DeathInfo);
            }
        }

        /// <summary>
        /// Get current vaccinations of animal and concat newly added to datagrid
        /// </summary>
        /// <returns>Hashset of all animal's vaccinations</returns>
        private HashSet<Vaccination> GetVaccinations()
        {
            HashSet<Vaccination> vaccinations = new HashSet<Vaccination>();
            vaccinations = vaccinations.Concat(VaccinationsDataGrid.Items.OfType<Vaccination>()).ToHashSet();
            foreach (var vacc in vaccinations)
            {
                vacc.Animal = Animal;
            }
            return vaccinations;
        }

        /// <summary>
        /// Discard adoptive button click event - discards adoptive
        /// </summary>
        /// <param name="sender">Clicked button object</param>
        /// <param name="e">Event arguments</param>
        private void DiscardChoosenAdoptive_Click(object sender, RoutedEventArgs e)
        {
            IsAdoptedCheckbox.IsChecked = false;
            DiscardChoosenAdoptive.IsEnabled = false;

            Adoptive adoptive = new Adoptive();
            Animal.Adoptive = adoptive;
            AdoptiveTab.DataContext = adoptive;

            AdoptivesComboBox.SelectedItem = null;
        }

        /// <summary>
        /// Cancel changes button event
        /// </summary>
        /// <param name="sender">Clicked button object</param>
        /// <param name="e">Event arguments</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Lost focus on email textbox event - fires validation
        /// </summary>
        /// <param name="sender">Email textbox object</param>
        /// <param name="e">Event arguments</param>
        private void AdoptiveEmailTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (!emailRegex.IsMatch(input))
            {
                MessageBox.Show("Niepoprawny adres email!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Preview adoptive's telephone number input - allow only digits
        /// </summary>
        /// <param name="sender">Input textbox object</param>
        /// <param name="e">Event arguments</param>
        private void AdoptiveTelephoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            AllowOnlyDigits(e);
        }

        /// <summary>
        /// Verifies if input textbox contains only digits
        /// </summary>
        /// <param name="e">Event arguments</param>
        private static void AllowOnlyDigits(TextCompositionEventArgs e)
        {
            string input = e.Text;
            e.Handled = !input.All(x => Char.IsDigit(x));
        }

        /// <summary>
        /// Validate postal code input on postal code textbox lost focus
        /// </summary>
        /// <param name="sender">Input textbox object</param>
        /// <param name="e">Event arguments</param>
        private void AdoptivePostalCodeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (!postalCodeRegex.IsMatch(input))
            {
                MessageBox.Show("Niepoprawny format kodu pocztowego!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Preview adoptive's house number input - allow only digits
        /// </summary>
        /// <param name="sender">Input textbox object</param>
        /// <param name="e">Event arguments</param>
        private void AdoptiveHouseNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            AllowOnlyDigits(e);
        }

        /// <summary>
        /// Preview adoptive's flat number input - allow only digits
        /// </summary>
        /// <param name="sender">Input textbox object</param>
        /// <param name="e">Event arguments</param>
        private void AdoptiveFlatNumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            AllowOnlyDigits(e);
        }

        /// <summary>
        /// Set death day to today if death checked is checked
        /// </summary>
        /// <param name="sender">Input checkbox object</param>
        /// <param name="e">Event arguments</param>
        private void IsAliveCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (AnimalDeathDateDatePicker.SelectedDate == null)
            {
                AnimalDeathDateDatePicker.SelectedDate = DateTime.Today;
            }
        }

        /// <summary>
        /// Set lost day to today if lost checked is checked
        /// </summary>
        /// <param name="sender">Input checkbox object</param>
        /// <param name="e">Event arguments</param>
        private void IsLostCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (AnimalLostDateDatePicker.SelectedDate == null)
            {
                AnimalLostDateDatePicker.SelectedDate = DateTime.Today;
            }
        }
    }
}
