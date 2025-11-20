using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UchebkaFinal.Data;

namespace UchebkaFinal;

public partial class StaffAdd : Window
{
    private readonly Staff _currentUser;
    public StaffAdd(Staff user)
    {
        _currentUser = user;
        InitializeComponent();
    }

    private void Button_Click(object? sender, RoutedEventArgs e) => this.Close();

    private async void Button_Click_1(object? sender, RoutedEventArgs e)
    {
        Errort.Text = "";

        try
        {
           
            string id = IdSta.Text?.Trim();
            string fullName = FullNameBox.Text?.Trim();
            string login = LoginBox.Text?.Trim();
            string password = PasswordBox.Text?.Trim(); 
            string position = PositionBox.Text?.Trim();
            string deptCode = DeptCodeBox.Text?.Trim();
            string salaryText = SalaryBox.Text?.Trim();
            string supIdText = SupervisorIdBox.Text?.Trim();

            
            if (string.IsNullOrEmpty(id))
            { Errort.Text = "❌ ID обязателен."; return; }
            if (!int.TryParse(id, out int staffIdInput) || staffIdInput <= 0)
            { Errort.Text = "❌ ID должен быть положительным целым числом."; return; }
            if (App.DbContext.Staff.AsNoTracking().Any(s => s.StaffId == staffIdInput))
            { Errort.Text = "❌ Сотрудник с таким ID уже существует."; return; }

            
            if (string.IsNullOrEmpty(fullName))
            { Errort.Text = "❌ Полное имя обязательно."; return; }
            if (string.IsNullOrEmpty(login))
            { Errort.Text = "❌ Логин обязателен."; return; }
            if (string.IsNullOrEmpty(password))
            { Errort.Text = "❌ Пароль обязателен."; return; }
            if (string.IsNullOrEmpty(position))
            { Errort.Text = "❌ Должность обязательна."; return; }
            if (string.IsNullOrEmpty(deptCode))
            { Errort.Text = "❌ Код отдела обязателен."; return; }

            
            if (fullName.Any(c => !char.IsLetter(c) && c != ' ' && c != '-' && c != '.'))
            { Errort.Text = "❌ Имя: только буквы, пробел, дефис, точка."; return; }
            if (position.Any(c => !char.IsLetter(c) && c != ' ' && c != '-' && c != '.'))
            { Errort.Text = "❌ Должность: только буквы, пробел, дефис, точка."; return; }
            if (login.Length < 3 || login.Contains(' '))
            { Errort.Text = "❌ Логин: от 3 символов, без пробелов."; return; }
            if (password.Length < 6 || !password.Any(char.IsDigit) || !password.Any(char.IsLetter))
            { Errort.Text = "❌ Пароль: ≥6 симв., буквы + цифры."; return; }

           
            if (!decimal.TryParse(salaryText, out decimal parsedSalary) || parsedSalary < 0)
            { Errort.Text = "❌ Зарплата: неотрицательное число (например: 50000.00)."; return; }

            if (_currentUser.Position != "зав. кафедрой")
            {
                if (!App.DbContext.Departments.AsNoTracking().Any(d => d.Code == deptCode))
                {
                    Errort.Text = $"❌ Отдел с кодом «{deptCode}» не найден. Проверьте список отделов.";
                    return;
                }
            }
            
            int? supervisorId = null;
            if (!string.IsNullOrWhiteSpace(supIdText))
            {
                if (!int.TryParse(supIdText, out int supId) || supId <= 0)
                { Errort.Text = "❌ ID руководителя: положительное целое."; return; }
                if (!App.DbContext.Staff.AsNoTracking().Any(s => s.StaffId == supId))
                { Errort.Text = "❌ Руководитель с таким ID не найден."; return; }
                supervisorId = supId;
            }

           
            if (App.DbContext.Staff.AsNoTracking().Any(s => s.Login == login))
            { Errort.Text = "❌ Логин уже занят."; return; }


            if (_currentUser.Position == "инженер")
            {
                var newStaff = new Staff
                {
                    StaffId = staffIdInput,
                    FullName = fullName,
                    Login = login,
                    Password = password,
                    Position = position,
                    DeptCode = deptCode,
                    Salary = parsedSalary,
                    SupervisorId = supervisorId
                };
                App.DbContext.Staff.Add(newStaff);
                await App.DbContext.SaveChangesAsync();
            }
            else if (_currentUser.Position == "зав. кафедрой")
            {
                DeptCodeBox.IsVisible = false;
                var newStaff = new Staff
                {
                    StaffId = staffIdInput,
                    FullName = fullName,
                    Login = login,
                    Password = password,
                    Position = position,
                    DeptCode = _currentUser.DeptCode,
                    Salary = parsedSalary,
                    SupervisorId = supervisorId
                };
                App.DbContext.Staff.Add(newStaff);
                await App.DbContext.SaveChangesAsync();
            }
            Errort.Foreground = Avalonia.Media.Brushes.Green;
            Errort.Text = "✅ Сотрудник успешно добавлен!";
            await Task.Delay(1200);
            this.Close(true);
        }
        catch (Exception ex)
        {
            
            var messages = new List<string> { "❌ Ошибки:" };
            var current = ex;
            int level = 1;
            while (current != null)
            {
                messages.Add($"{level}. {current.GetType().Name}: {current.Message}");
                current = current.InnerException;
                level++;
            }
            Errort.Text = string.Join("\n", messages);
        }
    }
}