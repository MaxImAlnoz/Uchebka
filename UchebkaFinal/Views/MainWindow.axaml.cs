using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace UchebkaFinal.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var login = LoginTextBox?.Text.Trim();
            var password = PasswordTextBox?.Text.Trim();

            if (login == null || password == null)
            {
                ErrorTextBlock.Text = "Поля логин и пароль не должны быть пустыми";
                return;
            }

            var user = App.DbContext.Staff.FirstOrDefault(x => x.Login == login && x.Password == password);

            if (user == null)
            {
                ErrorTextBlock.Text = "Пользователь не найден";
                return;
            }

            ErrorTextBlock.Text = string.Empty;
            App.CurrentUser = user;

            var nextWindow = new NextW(user);
            nextWindow.Show();
            Close();
        }

        private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var user = App.DbContext.Staff.FirstOrDefault(x => x.Position == "Доцент");
            App.CurrentUser = user;
            var nextWindow = new NextW(user);
            nextWindow.Show();
            Close();
        }
    }
}