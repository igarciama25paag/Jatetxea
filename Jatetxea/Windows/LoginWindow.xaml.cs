using Jatetxea.Conexions;
using Jatetxea.Data;
using Jatetxea.Windows;
using System.Windows;

namespace Jatetxea.Windows
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private class NoMatchingWindowTypeForUserTypeException(string message) : Exception(message);

        private void Sartu(object sender, RoutedEventArgs e) => SaioaHasi();

        private void SaioaHasi_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter) SaioaHasi();
        }

        private async void SaioaHasi()
        {
            try
            {
                User.Login(await JatetxeaDB.GetErabiltzailea(user.Text, pass.Password));
                switch (User.GetUserType())
                {
                    case Erabiltzailea.ErabiltzaileMotak.admin:
                        new AdminWindow().Show(); break;
                    case Erabiltzailea.ErabiltzaileMotak.arrunta:
                        new ArruntaWindow().Show(); break;
                    default:
                        throw new NoMatchingWindowTypeForUserTypeException($"No window found for UserType '{User.GetUserType()}'");
                }
                Close();
            }
            catch (InvalidOperationException)
            {
                message.Text = "Error: erabiltzailea edo pasahitza ez da zuzena";
            }
            catch (NoMatchingWindowTypeForUserTypeException ex)
            {
                message.Text = ex.Message;
            }
            finally { pass.Password = null; }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Aplikazioa itxi nahi al duzu?",
                "Irten",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
                Close();
        }
    }
}