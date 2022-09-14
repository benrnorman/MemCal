namespace MemCal.Views;

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Input;

/// <summary>
/// Code-behind from the MainWindow the app launches to.
/// </summary>
public partial class MainWindow : Window
{
    // FIELDS
    // ============

    private readonly DataTable dt = new DataTable();

    private readonly HashSet<string> operators = new HashSet<string> { "+", "-", "*", "/", "^", "%" };

    private bool decimalInputFlag = false;

    private bool negativeValueFlag = false;



    // CONSTRUCTORS
    // ============

    /// <summary>
    /// Creates a MainWindow.
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();
        this.ActionClear.Click += this.ActionClearClick;
        this.ActionNegate.Click += this.ActionNegateClick;
        this.ActionPercent.Click += this.ActionPercentClick;
        this.InputDecimal.Click += this.InputDecimalClick;
        this.InputNumber0.Click += this.InputNumberClick;
        this.InputNumber1.Click += this.InputNumberClick;
        this.InputNumber2.Click += this.InputNumberClick;
        this.InputNumber3.Click += this.InputNumberClick;
        this.InputNumber4.Click += this.InputNumberClick;
        this.InputNumber5.Click += this.InputNumberClick;
        this.InputNumber6.Click += this.InputNumberClick;
        this.InputNumber7.Click += this.InputNumberClick;
        this.InputNumber8.Click += this.InputNumberClick;
        this.InputNumber9.Click += this.InputNumberClick;
        this.OperatorAdd.Click += this.OperatorClick;
        this.OperatorSubtract.Click += this.OperatorClick;
        this.OperatorMultiply.Click += this.OperatorClick;
        this.OperatorDivide.Click += this.OperatorClick;
        this.OperatorCalculate.Click += this.CalculateClick;
    }



    // PROPERTIES
    // ============

    /// <summary>
    /// Gets or sets the current number.
    /// </summary>
    public double CurrentNumber { get; set; }

    /// <summary>
    /// Gets or sets the full mathematical expression the user has input.
    /// </summary>
    public string Expression { get; set; } = string.Empty;



    // METHODS
    // ============

    /// <summary>
    /// Clears the input and views.
    /// </summary>
    /// <param name="sender">The element that sent this command.</param>
    /// <param name="e">The event parameters.</param>
    private void ActionClearClick(object? sender, RoutedEventArgs e)
    {
        this.Reset();
    }

    /// <summary>
    /// Handles clicks on negate button.
    /// </summary>
    /// <param name="sender">The element that sent this command.</param>
    /// <param name="e">The event parameters.</param>
    private void ActionNegateClick(object? sender, RoutedEventArgs e)
    {
        this.CurrentNumber *= -1;
    }

    /// <summary>
    /// Handles clicks on percent button.
    /// </summary>
    /// <param name="sender">The element that sent this command.</param>
    /// <param name="e">The event parameters.</param>
    private void ActionPercentClick(object? sender, RoutedEventArgs e)
    {
        if (this.CurrentNumber != 0)
        {
            this.Expression += this.CurrentNumber.ToString() + "%";
        }

        this.AfterCommit();
    }

    /// <summary>
    /// After committing to the input string, reset the last number and decimal flag.
    /// </summary>
    private void AfterCommit()
    {
        this.CurrentNumber = 0;
        this.decimalInputFlag = false;
        this.negativeValueFlag = false;
        this.UpdateResult(string.Empty);
        this.UpdateHistory();
    }

    /// <summary>
    /// Evalutae the input string and display the result.
    /// </summary>
    private void Calculate()
    {
        this.CommitNumber();
        Debug.WriteLine(this.Expression);
        if (this.Expression != string.Empty)
        {
            try
            {
                string? result = this.dt.Compute(this.Expression, null).ToString();
                if (result != null)
                {
                    this.UpdateResult(result);
                }
                else
                {
                    this.UpdateResult(string.Empty);
                }

                this.UpdateHistory();
            }
            catch
            {
                this.Reset();
                this.UpdateResult("NaN");
            }
        }
    }

    /// <summary>
    /// Fire the calculate function.
    /// </summary>
    /// <param name="sender">The element that sent this command.</param>
    /// <param name="e">The event parameters.</param>
    private void CalculateClick(object? sender, RoutedEventArgs e)
    {
        this.Calculate();
    }

    /// <summary>
    /// <summary>
    /// Commit the current number to the input string and reset the last number value.
    /// </summary>
    private void CommitNumber()
    {
        if (this.CurrentNumber != 0 || this.Expression != string.Empty)
        {
            if (this.Expression != string.Empty)
            {
                this.Expression += " ";
            }

            this.Expression += this.CurrentNumber;
        }

        this.AfterCommit();
    }

    /// <summary>
    /// Handles clicks on the decimal input button.
    /// </summary>
    /// <param name="sender">The element that sent this command.</param>
    /// <param name="e">The event parameters.</param>
    private void InputDecimalClick(object? sender, RoutedEventArgs e)
    {
        this.ResolveDecimalInput();
    }

    /// <summary>
    /// Handles clicks on number input buttons.
    /// </summary>
    /// <param name="sender">The element that sent this command.</param>
    /// <param name="e">The event parameters.</param>
    private void InputNumberClick(object? sender, RoutedEventArgs e)
    {
        if (sender != null)
        {
            Button inputBtn = (Button)sender;
            this.ResolveNumberlInput(inputBtn.Content.ToString());
        }
    }

    /// <summary>
    /// Handles keyboard input.
    /// </summary>
    /// <param name="sender">The element that sent this command.</param>
    /// <param name="e">The key event parameters.</param>
    private void KeyDownHandler(object sender, KeyEventArgs e)
    {
        Debug.WriteLine(e.Key.ToString());
        switch (e.Key)
        {
            case Key.D1:
            case Key.D2:
            case Key.D3:
            case Key.D4:
            case Key.D5:
            case Key.D6:
            case Key.D7:
            case Key.D8:
            case Key.D9:
            case Key.D0:
            case Key.NumPad1:
            case Key.NumPad2:
            case Key.NumPad3:
            case Key.NumPad4:
            case Key.NumPad5:
            case Key.NumPad6:
            case Key.NumPad7:
            case Key.NumPad8:
            case Key.NumPad9:
            case Key.NumPad0:
                string inputString = e.Key.ToString().Replace("NumPad", string.Empty).Replace("D", string.Empty);
                this.ResolveNumberlInput(inputString);
                break;
            case Key.Add:
                this.ResolveOperator("+");
                break;
            case Key.Subtract:
                this.ResolveOperator("-");
                break;
            case Key.Multiply:
                this.ResolveOperator("*");
                break;
            case Key.Divide:
                this.ResolveOperator("/");
                break;
            case Key.Decimal:
                this.ResolveDecimalInput();
                break;
            case Key.Return:
                this.Calculate();
                break;
        }
    }

    /// <summary>
    /// Handles clicks on operator buttons.
    /// </summary>
    /// <param name="sender">The element that sent this command.</param>
    /// <param name="e">The event parameters.</param>
    private void OperatorClick(object? sender, RoutedEventArgs e)
    {
        if (sender != null)
        {
            Button inputBtn = (Button)sender;
            this.ResolveOperator(inputBtn.Content.ToString());
        }
    }

    /// <summary>
    /// Reset the state to intial values.
    /// </summary>
    private void Reset()
    {
        this.Expression = string.Empty;
        this.AfterCommit();
    }

    /// <summary>
    /// Resolve the decimal input by adding the decimal flag only if the
    /// current number is not already a decimal.
    /// </summary>
    private void ResolveDecimalInput()
    {
        if (this.CurrentNumber % 1 == 0)
        {
            this.decimalInputFlag = true;
        }
    }

    /// <summary>
    /// Resolve the string number input by adding it to the current number.
    /// </summary>
    /// <param name="inputString">The string to attempt to resolve.</param>
    private void ResolveNumberlInput(string? inputString)
    {
        string prefix = this.negativeValueFlag ? "-" : string.Empty;
        string divider = this.decimalInputFlag ? "." : string.Empty;
        string concatNumber = prefix + this.CurrentNumber.ToString() + divider + inputString;
        if (double.TryParse(concatNumber, out double userInput))
        {
            this.CurrentNumber = userInput;
            this.decimalInputFlag = false;
            this.negativeValueFlag = false;
            this.UpdateResult(this.CurrentNumber.ToString());
        }
    }

    /// <summary>
    /// Resolve the operator by adding it to the existing expression, if possible.
    /// </summary>
    /// <param name="operation">The operation to add.</param>
    private void ResolveOperator(string? operation)
    {
        if (operation != null && this.operators.Contains(operation))
        {
            if (this.CurrentNumber != 0)
            {
                this.CommitNumber();
                this.Expression += $" {operation} ";
            }
            else if (operation == "-")
            {
                this.negativeValueFlag = true;
            }
        }

        this.UpdateHistory();
    }

    /// <summary>
    /// Update the equation history.
    /// </summary>
    private void UpdateHistory()
    {
        this.CurrentEquation.Content = this.Expression;
    }

    /// <summary>
    /// Update the view to display the given result.
    /// </summary>
    /// <param name="result">The result to display.</param>
    private void UpdateResult(string result)
    {
        this.Result.Content = result;
    }
}