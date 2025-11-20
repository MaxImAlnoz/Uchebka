using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UchebkaFinal.Data;

namespace UchebkaFinal;

public partial class StudentUC : UserControl
{
    private List<Student1> _allStudent = new();
    private ObservableCollection<Student1> _student = new();
    private Student? _pendingNewStudent = null;
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

    private void Button_Click_3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var window = new CreateStudent();
        window.Show();
    }

    private void Button_Click_4(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
       if (StudentsGrid.SelectedItem is not Student1 selected)
            return;

        try
        {
            App.DbContext.Students1.Remove(selected);
            App.DbContext.SaveChanges();

            _allStudent.Remove(selected);
            ApplyFilter(); // или StudentsGrid.ItemsSource = _allStudents;
        }
        catch (Exception ex)
        {
            // Например, есть связанные экзамены → нельзя удалить
            ErrorTextBlock.Text = $"Ошибка удаления: {ex.Message}";
            ErrorTextBlock.IsVisible = true;
        }
    }
}