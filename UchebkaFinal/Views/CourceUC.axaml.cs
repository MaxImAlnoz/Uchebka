using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.Linq;
using UchebkaFinal.Data;

namespace UchebkaFinal;

public partial class CourceUC : UserControl
{
    private List<Course> _cource = new();
    public CourceUC()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        _cource = App.DbContext.Courses.ToList();
        CoursesGrid.ItemsSource = _cource;
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ApplyFilter();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        TitleFilterTextBox.Clear();
    }

    private void ApplyFilter()
    {
        if (CoursesGrid == null)
        {
            return;
        }

        var filter = TitleFilterTextBox.Text ?? string.Empty;
        var query = _cource.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(x => x.Title.Contains(filter, System.StringComparison.OrdinalIgnoreCase));
        }

        else
        {
            query = _cource;
        }

        CoursesGrid.ItemsSource = query;
    }

    private void TitleFilterTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        ApplyFilter();
    }
}