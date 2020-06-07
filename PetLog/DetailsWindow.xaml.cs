﻿using System;
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
        public DetailsWindow(AnimalsManager animalsManager, Animal animal)
        {
            AnimalsManager = animalsManager;
            Animal = animal;

            Mode = Mode.Edit;
            InitializeComponent();
            VaccinationsDataGrid.ItemsSource = AnimalsManager.GetAnimalVaccinations(Animal);
        }

        public DetailsWindow(AnimalsManager animalsManager)
        {
            InitializeComponent();
            AnimalsManager = animalsManager;
            Animal = new Animal();

            Mode = Mode.Add;
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
        }
    }
}
