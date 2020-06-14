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
    /// Interaction logic for LoginWindow.xaml - login window
    /// </summary>
    public partial class LoginWindow : Window
    {
        /// <summary>
        /// Users manager
        /// </summary>
        public UsersManager UsersManager { get; set; }
        /// <summary>
        /// Login window constructor - initialize users manager, checks connection to database and center window
        /// </summary>
        public LoginWindow()
        {
            try
            {
                UsersManager = new UsersManager();
            }
            catch (InvalidOperationException e)
            {
                if (e.InnerException.Message.ToString() == "Unable to connect to any of the specified MySQL hosts.")
                {
                    MessageBox.Show("Problem z połączeniem się do bazy danych! Sprawdź połączenie internetowe i spróbuj ponownie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show(e.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                Environment.Exit(0);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                throw e;
            }

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        /// <summary>
        /// Login button clicked - verify account credentials
        /// </summary>
        /// <param name="sender">Clicked button event</param>
        /// <param name="e">Event arguments</param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordPasswordBox.Password;

            var user = UsersManager.Login(login, password);
            if (user == null)
            {
                MessageBox.Show("Niepoprawny login lub hasło!", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            AnimalsWindow animalsWindow = new AnimalsWindow(user);
            animalsWindow.Show();

            Close();
        }
    }
}
