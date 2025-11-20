using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using UchebkaFinal.Data;
using UchebkaFinal.Views;

namespace UchebkaFinal;

public partial class NextW : Window
{
    private readonly Staff _user;
    
    public NextW(Staff user)
    {
        InitializeComponent();

        _user = user;

        var roleName = _user.Position ?? string.Empty;
        RoleTextBlock.Text = roleName;

        StudentsButton.IsEnabled = roleName == "инженер" || roleName == "зав. кафедрой" || roleName == "преподаватель";
        ExamsButton.IsEnabled = roleName == "зав. кафедрой" || roleName == "преподаватель" || roleName == "Доцент";
        CoursesButton.IsEnabled = roleName == "зав. кафедрой";
        StaffButton.IsEnabled = roleName == "инженер"|| roleName == "зав. кафедрой";

        
    }

    private void StudentsButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ContentCon.Content = new StudentUC();
    }

    private void ExamsButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ContentCon.Content = new ExamUC(_user);
    }

    private void StaffButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ContentCon.Content = new StaffUC(_user);
    }

    private void CoursesButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ContentCon.Content = new CourceUC(_user);
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var window = new MainWindow();
        window.Show();
        Close();
        
        
    }
}