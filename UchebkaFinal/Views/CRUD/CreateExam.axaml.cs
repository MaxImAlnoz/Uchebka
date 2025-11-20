using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using UchebkaFinal.Data;

namespace UchebkaFinal;

public partial class CreateExam : Window
{
    private readonly Exam _exam = new();
    private TextBlock? _errorTextBlock;

    public CreateExam()
    {
        InitializeComponent();
        _errorTextBlock = this.FindControl<TextBlock>("ErrorTextBlock");

        var db = App.DbContext;
        CBDiscipline.ItemsSource = db.Courses.ToList();
        CBStudent.ItemsSource = db.Students1.ToList();
        if (CBDiscipline.SelectedItem == null)
        {
            CBTeacher.ItemsSource = db.Staff.Where(s => s.Position == "преподаватель").ToList();
        }
      
        
           
     
    }

    private void Button_Click_1(object? sender, RoutedEventArgs e)
    {
        ClearError();

        var audience = TBAudience.Text?.Trim();
        if (string.IsNullOrEmpty(audience))
        {
            ShowError("Аудитория обязательна.");
            return;
        }

        if (!double.TryParse(TBGrade.Text, out double gradeValue) || gradeValue < 2 || gradeValue > 5)
        {
            ShowError("Оценка должна быть числом от 2 до 5.");
            return;
        }

        int grade = (int)gradeValue;

        if (DPDate.SelectedDate is not DateTimeOffset)
        {
            ShowError("Дата экзамена не выбрана.");
            return;
        }

        if (CBDiscipline.SelectedItem is not Course)
        {
            ShowError("Дисциплина не выбрана.");
            return;
        }

        if (CBStudent.SelectedItem is not Student1)
        {
            ShowError("Студент не выбран.");
            return;
        }

        if (CBTeacher.SelectedItem is not Staff)
        {
            ShowError("Преподаватель не выбран.");
            return;
        }

        var dto = (DateTimeOffset)DPDate.SelectedDate;
        var examDateTime = dto.UtcDateTime;
        if (examDateTime <= DateTime.UtcNow)
        {
            ShowError("Дата экзамена должна быть в будущем.");
            return;
        }

        // Заполнение и сохранение
        _exam.ExamDate = DateOnly.FromDateTime(examDateTime);
        _exam.CourseId = ((Course)CBDiscipline.SelectedItem).CourseId;
        _exam.RegNum = ((Student1)CBStudent.SelectedItem).RegNum;
        _exam.StaffId = ((Staff)CBTeacher.SelectedItem).StaffId;
        _exam.Classroom = audience;
        _exam.Grade = grade;

        App.DbContext.Exams.Add(_exam);
        App.DbContext.SaveChanges();

        this.Close();
    }

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void ShowError(string message)
    {
        if (_errorTextBlock != null)
        {
            _errorTextBlock.Text = message;
            _errorTextBlock.IsVisible = true;
        }
    }

    private void ClearError()
    {
        if (_errorTextBlock != null)
        {
            _errorTextBlock.Text = string.Empty;
            _errorTextBlock.IsVisible = false;
        }
    }
}