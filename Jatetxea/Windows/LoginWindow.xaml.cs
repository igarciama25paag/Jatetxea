using Jatetxea.Conexions;
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
                User.SetUser(user.Text, await JatetxeaDB.GetUserType(user.Text, pass.Password));
                switch (User.GetUserType())
                {
                    case User.UserTypes.admin:
                        new AdminWindow().Show(); break;
                    case User.UserTypes.arrunta:
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