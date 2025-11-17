using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using UchebkaFinal.Data;

namespace UchebkaFinal;

public partial class StudentUC : UserControl
{
    private List<Student1> _allStudent = new();
    public StudentUC()
    {
        InitializeComponent();
        LoadData();
    }

    public void LoadData()
    {
        _allStudent = App.DbContext.Students1.ToList();
        StudentsGrid.ItemsSource = _allStudent;
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ApplyFilter();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SurnameFilterTextBox.Text = string.Empty;
        ApplyFilter();
    }

    private void SurnameFilterTextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        if (StudentsGrid is null)
        {
            return;
        }

        var filter = SurnameFilterTextBox.Text ?? string.Empty;
        var query = _allStudent.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(s => s.FullName.Contains(filter, StringComparison.OrdinalIgnoreCase));
        }
        else
        {
            query = _allStudent;
        }

        StudentsGrid.ItemsSource = query.ToList();
    }

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}