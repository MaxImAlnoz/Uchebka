using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Linq;
using UchebkaFinal.Data;

namespace UchebkaFinal;

public partial class CreateStudent : Window
{
    public CreateStudent()
    {
        InitializeComponent();
        CBProgram.ItemsSource = App.DbContext.Programs.ToList();
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        ErrorTextBlock.IsVisible = false;

        var fullName = TBFullName.Text?.Trim();
        if (string.IsNullOrEmpty(fullName))
        {
            ErrorTextBlock.Text = "ФИО обязательно";
            ErrorTextBlock.IsVisible = true;
            return;
        }

        var program = CBProgram.SelectedItem as Data.Program;
        if (program == null)
        {
            ErrorTextBlock.Text = "Выберите программу";
            ErrorTextBlock.IsVisible = true;
            return;
        }
            
        var student = new Student1
        {
            FullName = fullName,
            ProgramCode = program.Code,
            RegNum = App.DbContext.Students1.Any()
                ? App.DbContext.Students1.Max(s => s.RegNum) + 1
                : 1
        };

        App.DbContext.Students1.Add(student);
        App.DbContext.SaveChanges();

        this.Close();
    }

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}