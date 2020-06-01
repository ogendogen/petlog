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
using Database.Models;

namespace PetLog
{
    /// <summary>
    /// Interaction logic for AnimalsWindow.xaml
    /// </summary>
    public partial class AnimalsWindow : Window
    {
        public User User { get; }
        public AnimalsWindow(User user)
        {
            InitializeComponent();
            User = user;

            MessageBox.Show($"Witaj {User.Name} {User.Surname}");
        }
    }
}
