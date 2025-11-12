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

        private async void Sartu(object sender, RoutedEventArgs e)
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
    }
}