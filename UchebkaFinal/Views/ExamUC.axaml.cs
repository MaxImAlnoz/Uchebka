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

    public ExamUC(Staff user)
    {
        InitializeComponent();
        LoadData(user);
        if (user.Position == "зав. кафедрой")
        {
            ExamsGrid.IsReadOnly = false;
            Create.IsEnabled = true;
        }


    }

    private void LoadData(Staff user)
    {
        if (user.Position != "зав. кафедрой")
        {
            _exam = App.DbContext.Exams.Where(x => x.StaffId == user.StaffId).ToList();

            ExamsGrid.ItemsSource = _exam;
        }
        else
        {
            _exam = App.DbContext.Exams.ToList();

            ExamsGrid.ItemsSource = _exam;
        }

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

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {

            ExamsGrid?.CommitEdit();

            var saved = App.DbContext.SaveChanges();

            ErrorTextBlock.Text = $"Сохранено записей: {saved}.";
        }
        catch (Exception ex)
        {
            ErrorTextBlock.Text = $"от 2 до 5";
        }
    }

    private void Button_Click_3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var window = new CreateExam();
        window.Show();
    }
}