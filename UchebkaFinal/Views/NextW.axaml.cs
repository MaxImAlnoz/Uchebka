using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using UchebkaFinal.Data;

namespace UchebkaFinal;

public partial class NextW : Window
{
    private readonly UserAccount _user;
    public NextW(UserAccount user)
    {
        InitializeComponent();

        _user = user;

        var roleName = _user.Role?.Name ?? string.Empty;
        RoleTextBlock.Text = roleName;

        StudentsButton.IsEnabled = true;
        ExamsButton.IsEnabled = true;
        CoursesButton.IsEnabled = roleName == Role.Admin || roleName == Role.Head;
        StaffButton.IsEnabled = roleName == Role.Admin;
    }

    private void StudentsButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ContentCon.Content = new StudentUC();
    }

    private void ExamsButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ContentCon.Content = new ExamUC();
    }

    private void StaffButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ContentCon.Content = new StaffUC();
    }

    private void CoursesButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ContentCon.Content = new CourceUC();
    }
}