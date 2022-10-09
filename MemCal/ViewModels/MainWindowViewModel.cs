namespace MemCal.ViewModels;

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using org.mariuszgromada.math.mxparser;
using ReactiveUI;
using System.Reactive;
using MemCal.Models.DataTypes.Enums;
using Avalonia.Animation;

/// <summary>
/// The viewmodel for the main window.
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    // FIELDS
    // ============

    private bool decimalInputFlag = false;

    private bool evaluted = false;

    private bool negativeValueFlag = false;



    // CONSTRUCTORS
    // ============

    /// <summary>
    /// Creates a main window viewmodel.
    /// </summary>
    public MainWindowViewModel()
    {
        this.NumberInputCommand = ReactiveCommand.Create<int>(this.ResolveNumberInput);
        this.OperatorCommand = ReactiveCommand.Create<Operation?>(this.ResolveOperator);
        this.ResetCommand = ReactiveCommand.Create(this.ActionClear);
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

    /// <summary>
    /// Gets the numner input command.
    /// </summary>
    public ReactiveCommand<int, Unit> NumberInputCommand { get; }

    /// <summary>
    /// Gets the operator command.
    /// </summary>
    public ReactiveCommand<Operation?, Unit> OperatorCommand { get; }

    /// <summary>
    /// Gets the reset command.
    /// </summary>
    public ReactiveCommand<Unit, Unit> ResetCommand { get; }



    // METHODS
    // ============

    /// <summary>
    /// Clears the input and views.
    /// </summary>
    private void ActionClear()
    {
        this.Reset();
        this.PostInputAction();
    }

    /// <summary>
    /// Resolve the string number input by adding it to the current number.
    /// </summary>
    /// <param name="input">The string to attempt to resolve.</param>
    private void ResolveNumberInput(int input)
    {
        this.PreResolve();
        string prefix = this.negativeValueFlag ? "-" : string.Empty;
        string divider = this.decimalInputFlag ? "." : string.Empty;
        string concatNumber = prefix + this.CurrentNumber.ToString() + divider + input;
        if (double.TryParse(concatNumber, out double userInput))
        {
            this.CurrentNumber = userInput;
            this.decimalInputFlag = false;
            this.negativeValueFlag = false;
            this.UpdateResultOutput(this.CurrentNumber.ToString());
        }

        this.PostInputAction();
    }

    /// <summary>
    /// Resolve the operator by adding it to the existing expression, if possible.
    /// </summary>
    /// <param name="operation">The operation to add.</param>
    private void ResolveOperator(Operation? operation)
    {
        this.PreResolve();
        if (operation != null)
        {
            if (this.CurrentNumber == 0 && operation == Operation.Subtraction)
            {
                this.negativeValueFlag = true;
            }
            else if (this.CurrentNumber != 0)
            {
                this.CommitNumber();
                string prefix = operation == Operation.Percentage ? string.Empty : " ";
                this.Expression += $"{prefix}{(char)operation} ";
            }
        }

        this.UpdateHistoryOutput();
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
    /// Handles clicks on negate button.
    /// </summary>
    /// <param name="sender">The element that sent this command.</param>
    /// <param name="e">The event parameters.</param>
    private void ActionNegateClick(object? sender, RoutedEventArgs e)
    {
        this.CurrentNumber *= -1;
        this.UpdateResultOutput(this.CurrentNumber.ToString());
        this.PostInputAction();
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
        this.PostInputAction();
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
        this.PostInputAction();
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
        this.PostInputAction();
    }

    /// <summary>
    /// After clicking an input, reset focus.
    /// </summary>
    private void PostInputAction()
    {
        // this.Result.Focus();
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
    /// Update the equation history.
    /// </summary>
    private void UpdateHistoryOutput()
    {
        // this.CurrentEquation.Content = this.Expression;
    }

    /// <summary>
    /// Update the view to display the given result.
    /// </summary>
    /// <param name="result">The result to display.</param>
    private void UpdateResultOutput(string result)
    {
        // this.Result.Content = result;
        this.UpdateHistoryOutput();
    }
}