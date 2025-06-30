using Microsoft.Maui.Controls;

namespace TravelBuddy.Pages;

public partial class CalculatorPage : ContentPage
{
    private double _currentValue = 0;
    private string _operator = "";
    private double _previousValue = 0;

    public CalculatorPage()
    {
        InitializeComponent();
    }

    private void OnNumberClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        ResultEntry.Text += button.Text;
    }

    private void OnOperatorClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        if (!string.IsNullOrEmpty(ResultEntry.Text))
        {
            _previousValue = double.Parse(ResultEntry.Text);
            _operator = button.Text;
            ResultEntry.Text = "";
        }
    }

    private void OnCalculateClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ResultEntry.Text) && !string.IsNullOrEmpty(_operator))
        {
            _currentValue = double.Parse(ResultEntry.Text);
            double result = 0;
            switch (_operator)
            {
                case "+": result = _previousValue + _currentValue; break;
                case "-": result = _previousValue - _currentValue; break;
                case "*": result = _previousValue * _currentValue; break;
                case "/": result = _currentValue != 0 ? _previousValue / _currentValue : double.NaN; break;
            }
            ResultEntry.Text = result.ToString();
            _operator = "";
        }
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        ResultEntry.Text = "";
        _currentValue = 0;
        _previousValue = 0;
        _operator = "";
    }
}