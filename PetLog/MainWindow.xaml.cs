using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Database;

namespace PetLog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public UsersManager UsersManager { get; set; }
        public AnimalsManager AnimalManager { get; set; }
        public MainWindow()
        {
            UsersManager = new UsersManager();
            AnimalManager = new AnimalsManager();
            InitializeComponent();
        }
    }
}
