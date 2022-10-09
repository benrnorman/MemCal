namespace MemCal.Views;

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using org.mariuszgromada.math.mxparser;
using System;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// Code-behind from the MainWindow the app launches to.
/// </summary>
public partial class MainWindow : Window
{
    // FIELDS
    // ============

    private static HashSet<string> operators = new HashSet<string> { "+", "-", "*", "/", "^", "%" };

    private bool decimalInputFlag = false;

    private bool evaluted = false;

    private bool negativeValueFlag = false;

    private bool shiftLeftDownFlag = false;

    private bool shiftRightDownFlag = false;



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
        this.OperatorCalculate.Click += this.CalculateClick;
    }



    // PROPERTIES
    // ============

    /// <summary>
    /// Gets or sets the current number.
    /// </summary>
    public double CurrentNumber { get; set; }

    /// <summary>
    /// Gets the last character in the expression.
    /// </summary>
    public char? LastChar
    {
        get
        {
            string trimmedExpression = this.Expression.Trim(' ');
            if (trimmedExpression.Length > 0)
            {
                return trimmedExpression[trimmedExpression.Length - 1];
            }

            return null;
        }
    }

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
        this.PostClickAction();
    }

    /// <summary>
    /// Handles clicks on negate button.
    /// </summary>
    /// <param name="sender">The element that sent this command.</param>
    /// <param name="e">The event parameters.</param>
    private void ActionNegateClick(object? sender, RoutedEventArgs e)
    {
        this.CurrentNumber *= -1;
        this.UpdateResultOutput(this.CurrentNumber.ToString());
        this.PostClickAction();
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
        this.PostClickAction();
    }

    /// <summary>
    /// After committing to the input string, reset the last number and decimal flag.
    /// </summary>
    private void AfterCommit()
    {
        this.CurrentNumber = 0;
        this.decimalInputFlag = false;
        this.negativeValueFlag = false;
        this.UpdateResultOutput(string.Empty);
    }

    /// <summary>
    /// Evalutae the input string and display the result.
    /// </summary>
    private void Calculate()
    {
        if (!this.evaluted)
        {
            this.CommitNumber();
            if (this.Expression != string.Empty)
            {
                string? result = string.Empty;
                try
                {
                    Expression evaluation = new Expression(this.Expression);
                    result = evaluation.calculate().ToString();
                }
                catch
                {
                    result = "NaN";
                }

                this.evaluted = true;
                this.Expression += " =";
                this.UpdateResultOutput(result);
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
        this.PostClickAction();
    }

    /// <summary>
    /// <summary>
    /// Commit the current number to the input string and reset the last number value.
    /// </summary>
    private void CommitNumber()
    {
        if (this.CurrentNumber != 0 || this.Expression != string.Empty)
        {
            if (this.LastChar != '%')
            {
                if (this.Expression != string.Empty)
                {
                    this.Expression += " ";
                }

                this.Expression += this.CurrentNumber;
            }
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
        this.PostClickAction();
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

        this.PostClickAction();
    }

    /// <summary>
    /// Handles keyboard key presses.
    /// </summary>
    /// <param name="sender">The element that sent this command.</param>
    /// <param name="e">The key event parameters.</param>
    private void KeyDownHandler(object sender, KeyEventArgs e)
    {
        string inputString = string.Empty;
        switch (e.Key)
        {
            case Key.LeftShift:
                this.shiftLeftDownFlag = true;
                break;
            case Key.RightShift:
                this.shiftRightDownFlag = true;
                break;
            case Key.D1:
            case Key.D2:
            case Key.D3:
            case Key.D4:
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
                inputString = e.Key.ToString().Replace("NumPad", string.Empty).Replace("D", string.Empty);
                this.ResolveNumberlInput(inputString);
                break;
            case Key.D5:
                if (this.shiftRightDownFlag || this.shiftLeftDownFlag)
                {
                    this.ResolveOperator("%");
                }
                else
                {
                    inputString = e.Key.ToString().Replace("NumPad", string.Empty).Replace("D", string.Empty);
                    this.ResolveNumberlInput(inputString);
                }

                break;
            case Key.Add:
            case Key.OemPlus:
                this.ResolveOperator("+");
                break;
            case Key.Subtract:
            case Key.OemMinus:
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
    /// Handles keyboard key presses.
    /// </summary>
    /// <param name="sender">The element that sent this command.</param>
    /// <param name="e">The key event parameters.</param>
    private void KeyUpHandler(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.LeftShift:
                this.shiftLeftDownFlag = false;
                break;
            case Key.RightShift:
                this.shiftRightDownFlag = false;
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

        this.PostClickAction();
    }

    /// <summary>
    /// After clicking an input, reset focus.
    /// </summary>
    private void PostClickAction()
    {
        this.Result.Focus();
    }

    /// <summary>
    /// Actions to run prior resolving any input.
    /// </summary>
    private void PreResolve()
    {
        if (this.evaluted)
        {
            this.Reset();
        }
    }

    /// <summary>
    /// Reset the state to intial values.
    /// </summary>
    private void Reset()
    {
        this.evaluted = false;
        this.Expression = string.Empty;
        this.AfterCommit();
    }

    /// <summary>
    /// Resolve the decimal input by adding the decimal flag only if the
    /// current number is not already a decimal.
    /// </summary>
    private void ResolveDecimalInput()
    {
        this.PreResolve();
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
        this.PreResolve();
        string prefix = this.negativeValueFlag ? "-" : string.Empty;
        string divider = this.decimalInputFlag ? "." : string.Empty;
        string concatNumber = prefix + this.CurrentNumber.ToString() + divider + inputString;
        if (double.TryParse(concatNumber, out double userInput))
        {
            this.CurrentNumber = userInput;
            this.decimalInputFlag = false;
            this.negativeValueFlag = false;
            this.UpdateResultOutput(this.CurrentNumber.ToString());
        }
    }

    /// <summary>
    /// Resolve the operator by adding it to the existing expression, if possible.
    /// </summary>
    /// <param name="operation">The operation to add.</param>
    private void ResolveOperator(string? operation)
    {
        this.PreResolve();
        if (operation != null && operators.Contains(operation))
        {
            if (this.CurrentNumber == 0 && operation == "-")
            {
                this.negativeValueFlag = true;
            }
            else if (this.CurrentNumber != 0)
            {
                this.CommitNumber();
                string prefix = operation == "%" ? string.Empty : " ";
                this.Expression += $"{prefix}{operation} ";
            }
        }

        this.UpdateHistoryOutput();
    }

    /// <summary>
    /// Update the equation history.
    /// </summary>
    private void UpdateHistoryOutput()
    {
        this.CurrentEquation.Content = this.Expression;
    }

    /// <summary>
    /// Update the view to display the given result.
    /// </summary>
    /// <param name="result">The result to display.</param>
    private void UpdateResultOutput(string result)
    {
        this.Result.Content = result;
        this.UpdateHistoryOutput();
    }
}