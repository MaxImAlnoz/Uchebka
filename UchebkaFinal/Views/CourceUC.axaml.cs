using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using UchebkaFinal.Data;

namespace UchebkaFinal;

public partial class CourceUC : UserControl
{
    private List<Course> _courses = new();
    
    public CourceUC(Staff user)
    {
        
        var _CurrentUser = user;
        InitializeComponent();
        LoadData(user);

    }

    private void LoadData(Staff user)
    {
        _courses = App.DbContext.Courses.ToList();
        CoursesGrid.ItemsSource = _courses;
        if (user.Position == "зав. кафедрой")
        {
            CoursesGrid.IsReadOnly = false;
        }
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
        var query = _courses.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            query = query.Where(x => x.Title.Contains(filter, System.StringComparison.OrdinalIgnoreCase));
        }

        else
        {
            query = _courses;
        }

        CoursesGrid.ItemsSource = query;
    }

    private void TitleFilterTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        ApplyFilter();
    }

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }

    private void Button_Click_3(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ClearError();

        var errors = new List<string>();

        foreach (var course in _courses)
        {
            if (string.IsNullOrWhiteSpace(course.Title))
                errors.Add($"Курс ID={course.CourseId}: название обязательно.");

            if (string.IsNullOrWhiteSpace(course.DeptCode) || course.DeptCode.Length > 10)
                errors.Add($"Курс ID={course.CourseId}: код кафедры обязателен и ≤10 символов.");

            if (course.Workload < 0 || course.Workload > 1000)
                errors.Add($"Курс ID={course.CourseId}: нагрузка от 0 до 1000.");
        }

        if (errors.Count > 0)
        {
            
            return;
        }

        try
        {
           
            var newCourses = _courses.Where(c => c.CourseId == 0).ToList();
            foreach (var c in newCourses)
            {
                var maxId = App.DbContext.Courses.Any() ? App.DbContext.Courses.Max(x => x.CourseId) : 0;
                c.CourseId = maxId + 1;
                App.DbContext.Courses.Add(c);
            }

            App.DbContext.SaveChanges();
           
        }
        catch (Exception ex)
        {
          
        }
    }

    private void ClearError()
    {
        return;
    }

}