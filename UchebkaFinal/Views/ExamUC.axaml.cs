using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using UchebkaFinal.Data;

namespace UchebkaFinal;

public partial class ExamUC : UserControl
{
    private List<Exam> _exam = new();
    public ExamUC()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        _exam = App.DbContext.Exams.ToList();
        ExamsGrid.ItemsSource = _exam;
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ApplyFilter();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ClassroomFilterTextBox.Clear();
    }

    private void ApplyFilter()
    {
        if (ExamsGrid is null)
        {
            return;
        }

        var filter = ClassroomFilterTextBox.Text ?? string.Empty;
        var query = _exam.AsEnumerable();

        if (!string.IsNullOrEmpty(filter))
        {
            query = query.Where(x => x.Classroom.Contains(filter, StringComparison.OrdinalIgnoreCase));
        }
        else
        {
            query = _exam;
        }

        ExamsGrid.ItemsSource = query;

    }

    private void ClassroomFilterTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        ApplyFilter();
    }
}