using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Linq;
using UchebkaFinal.Data;

namespace UchebkaFinal.Views;

public partial class AddUserWindow : Window
{
    public AddUserWindow()
    {
        InitializeComponent();
        LoadRoles();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void LoadRoles()
    {
        var roleNames = App.DbContext.UserRoles
            .Select(r => r.Name)
            .ToList();

        var roleCombo = this.FindControl<ComboBox>("RoleComboBox");
        if (roleCombo is null)
        {
            return;
        }

        roleCombo.ItemsSource = roleNames;

        if (roleNames.Count > 0)
            roleCombo.SelectedIndex = 0;
    }

    private void OkButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var login = LoginTextBox.Text?.Trim() ?? string.Empty;
        var password = PasswordTextBox.Text?.Trim() ?? string.Empty;

        var roleCombo = this.FindControl<ComboBox>("RoleComboBox");
        var selectedRoleName = roleCombo?.SelectedItem as string;

        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(selectedRoleName))
        {
            ErrorTextBlock.Text = "Заполните логин, пароль и выберите роль.";
            return;
        }

        var role = App.DbContext.UserRoles.FirstOrDefault(r => r.Name == selectedRoleName);
        if (role is null)
        {
            ErrorTextBlock.Text = "Выбранная роль не найдена.";
            return;
        }

        var exists = App.DbContext.UserAccounts.Any(u => u.Login == login);
        if (exists)
        {
            ErrorTextBlock.Text = "Такой логин уже существует.";
            return;
        }

        var user = new UserAccount
        {
            Login = login,
            Password = password,
            RoleId = role.Id
        };

        App.DbContext.UserAccounts.Add(user);
        App.DbContext.SaveChanges();

        Close();
    }

    private void CancelButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }
}


