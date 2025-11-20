using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using UchebkaFinal.Data;

namespace UchebkaFinal;

public partial class StaffUC : UserControl
{
    private ObservableCollection<Staff> _staff = new();

    private Staff? _pendingNewStaff = null;
    private readonly Staff _currentUser;

    public StaffUC(Staff user)
    {
        _currentUser = user;
        InitializeComponent();
        LoadData(user);
    }

    private void LoadData(Staff user)
    {
        if (user.Position == "инженер")
        {

            var staffFromDb = App.DbContext.Staff.Where(x => x.Position != "инженер").ToList();

            _staff.Clear();
            foreach (var s in staffFromDb)
                _staff.Add(s);

            StaffGrid.ItemsSource = _staff;
            _pendingNewStaff = null;
        }
        if (user.Position == "зав. кафедрой")
        {
            var staffFromDb = App.DbContext.Staff.Where(x => x.DeptCode == user.DeptCode).ToList();

            _staff.Clear();
            foreach (var s in staffFromDb)
                _staff.Add(s);

            StaffGrid.ItemsSource = _staff;
            _pendingNewStaff = null;
        }

    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ApplyFilter();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        NameFilterTextBox.Clear();
        ApplyFilter();
    }

    private void NameFilterTextBox_TextChanged(object? sender, TextChangedEventArgs e)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        if (StaffGrid == null) return;

        var filter = NameFilterTextBox.Text?.Trim() ?? string.Empty;

        var source = _staff;

        var filtered = App.DbContext.Staff.AsEnumerable();

        if (!string.IsNullOrEmpty(filter))
        {
            filtered = filtered.Where(x =>
                x.FullName.StartsWith(filter, StringComparison.OrdinalIgnoreCase));
        }

        
        _staff.Clear();
        foreach (var s in filtered)
            _staff.Add(s);

        
        if (_pendingNewStaff != null &&
            (string.IsNullOrEmpty(filter) ||
             _pendingNewStaff.FullName.StartsWith(filter, StringComparison.OrdinalIgnoreCase)))
        {
            _staff.Add(_pendingNewStaff);
        }
    }

    private void AddButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        /*var departments = App.DbContext.Departments.Select(d => d.Code).ToList();
        if (departments.Count == 0)
        {
            ErrorTextBlock.Text = "Нет доступных кафедр. Сначала создайте кафедру.";
            return;
        }

        _pendingNewStaff = new Staff
        {
            DeptCode = departments[0], 
            FullName = "", 
            Position = "",
            Salary = 0
        };

        _staff.Add(_pendingNewStaff);

        StaffGrid.SelectedItem = _pendingNewStaff;
        StaffGrid.Focus();*/
        var nextWindow = new Window();
        nextWindow = new StaffAdd(_currentUser);
        nextWindow.Show();

    }

    private void DeleteButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (StaffGrid.SelectedItem is not Staff selected) return;

        if (selected == _pendingNewStaff)
        {
            _staff.Remove(selected);
            _pendingNewStaff = null;
        }
        else
        {
            App.DbContext.Staff.Remove(selected);
            _staff.Remove(selected);
        }

        ErrorTextBlock.Text = "";

        
    }

    private void SaveButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            if (_pendingNewStaff != null)
            {
                if (string.IsNullOrWhiteSpace(_pendingNewStaff.FullName))
                {
                    ErrorTextBlock.Text = "Поле 'Name' обязательно для заполнения.";
                    return;
                }

                App.DbContext.Staff.Add(_pendingNewStaff);
                _pendingNewStaff = null; 
            }

            App.DbContext.SaveChanges();

            
            LoadData(_currentUser);

            ErrorTextBlock.Text = "Сохранено.";
        }
        catch (Exception ex)
        {
            ErrorTextBlock.Text = $"Ошибка сохранения: {ex.Message}";
        }
    }
}