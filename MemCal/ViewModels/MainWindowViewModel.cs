namespace MemCal.ViewModels;

using org.mariuszgromada.math.mxparser;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive;
using MemCal.DataTypes.Enums;
using MemCal.Models;
using Avalonia.Collections;

/// <summary>
/// The viewmodel for the main window.
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    // FIELDS
    // ============

    private bool decimalInputFlag = false;

    private bool evaluated = false;

    private string expression = string.Empty;

    private bool negativeValueFlag = false;

    private bool refocus = false;

    private string result = string.Empty;



    // CONSTRUCTORS
    // ============

    /// <summary>
    /// Creates a main window viewmodel.
    /// </summary>
    public MainWindowViewModel()
    {
        this.ActionClearCommand = ReactiveCommand.Create(this.ActionClear);
        this.ActionNegateCommand = ReactiveCommand.Create(this.ActionNegate);
        this.CalculateCommand = ReactiveCommand.Create(this.Calculate);
        this.InputDecimalCommand = ReactiveCommand.Create(this.InputDecimal);
        this.InputExponentCommand = ReactiveCommand.Create(this.InputExponent);
        this.InputNumberCommand = ReactiveCommand.Create<int>(this.InputNumber);
        this.InputOperatorCommand = ReactiveCommand.Create<Operation?>(this.InputOperator);
    }



    // PROPERTIES
    // ============

    /// <summary>
    /// Gets the reset command.
    /// </summary>
    public ReactiveCommand<Unit, Unit> ActionClearCommand { get; }

    /// <summary>
    /// Gets the negate command.
    /// </summary>
    public ReactiveCommand<Unit, Unit> ActionNegateCommand { get; }

    /// <summary>
    /// Gets the calculate command.
    /// </summary>
    public ReactiveCommand<Unit, Unit> CalculateCommand { get; }

    /// <summary>
    /// Gets a list of all the performed calculations.
    /// </summary>
    public AvaloniaList<Calculation> Calculations { get; } = new AvaloniaList<Calculation>();

    /// <summary>
    /// Gets or sets the current number.
    /// </summary>
    public double CurrentNumber { get; set; }


    /// <summary>
    /// Gets or sets the full mathematical expression the user has input.
    /// </summary>
    public string Expression
    {
        get => this.expression;
        set => this.RaiseAndSetIfChanged(ref this.expression, value);
    }

    /// <summary>
    /// Gets the decimal input command.
    /// </summary>
    public ReactiveCommand<Unit, Unit> InputDecimalCommand { get; }

    /// <summary>
    /// Gets the percentage input command.
    /// </summary>
    public ReactiveCommand<Unit, Unit> InputExponentCommand { get; }

    /// <summary>
    /// Gets the number input command.
    /// </summary>
    public ReactiveCommand<int, Unit> InputNumberCommand { get; }

    /// <summary>
    /// Gets the operator command.
    /// </summary>
    public ReactiveCommand<Operation?, Unit> InputOperatorCommand { get; }

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
    /// Gets or sets a value indicating whether the view should be updated.
    /// </summary>
    public bool Refocus
    {
        get => this.refocus;
        set => this.RaiseAndSetIfChanged(ref this.refocus, value);
    }

    /// <summary>
    /// Gets or sets the result string.
    /// </summary>
    public string Result
    {
        get => this.result;
        set => this.RaiseAndSetIfChanged(ref this.result, value);
    }



    // METHODS
    // ============

    /// <summary>
    /// Evalutae the input string and display the result.
    /// </summary>
    public void Calculate()
    {
        if (!this.evaluated)
        {
            this.CommitNumber();
            if (this.Expression != string.Empty)
            {
                string? result = string.Empty;
                try
                {
                    Expression evaluation = new Expression(this.Expression);
                    result = evaluation.calculate().ToString();
                    Calculation calculation = new Calculation(evaluation);
                    this.Calculations.Insert(0, calculation);
                }
                catch
                {
                    result = "NaN";
                }

                this.evaluated = true;
                this.UpdateExpression(" =");
                this.UpdateResult(result);
                this.PostInteraction();
            }
        }
    }

    /// <summary>
    /// Resolve the decimal input by adding the decimal flag only if the
    /// current number is not already a decimal.
    /// </summary>
    public void InputDecimal()
    {
        this.PreInput();
        if (this.CurrentNumber % 1 == 0)
        {
            this.decimalInputFlag = true;
        }

        this.PostInteraction();
    }

    /// <summary>
    /// Resolve the string number input by adding it to the current number.
    /// </summary>
    /// <param name="input">The string to attempt to resolve.</param>
    public void InputNumber(int input)
    {
        this.PreInput();
        string prefix = this.negativeValueFlag ? "-" : string.Empty;
        string divider = this.decimalInputFlag ? "." : string.Empty;
        string concatNumber = prefix + this.CurrentNumber.ToString() + divider + input;
        if (double.TryParse(concatNumber, out double userInput))
        {
            this.CurrentNumber = userInput;
            this.decimalInputFlag = false;
            this.negativeValueFlag = false;
            this.UpdateResult(this.CurrentNumber.ToString());
            this.PostInteraction();
        }
    }

    /// <summary>
    /// Resolve the operator by adding it to the existing expression, if possible.
    /// </summary>
    /// <param name="operation">The operation to add.</param>
    public void InputOperator(Operation? operation)
    {
        this.PreInput();
        if (operation != null)
        {
            if (this.CurrentNumber == 0 && operation == Operation.Subtraction)
            {
                this.negativeValueFlag = true;
            }
            else if (this.CurrentNumber != 0)
            {
                this.CommitNumber();
                string prefix = operation == Operation.Exponent ? string.Empty : " ";
                this.UpdateExpression($"{prefix}{(char)operation} ");
            }
        }

        this.PostInteraction();
    }

    /// <summary>
    /// Handles percentage input.
    /// </summary>
    public void InputExponent()
    {
        this.UpdateExpression(this.CurrentNumber.ToString() + "^");
        this.AfterCommit();
        this.PostInteraction();
    }

    /// <summary>
    /// Clears the input and views.
    /// </summary>
    private void ActionClear()
    {
        this.Reset();
        this.PostInteraction();
    }

    /// <summary>
    /// Negates the current number.
    /// </summary>
    private void ActionNegate()
    {
        this.CurrentNumber *= -1;
        this.UpdateResult(this.CurrentNumber.ToString());
        this.PostInteraction();
    }

    /// <summary>
    /// After committing to the input string, reset the last number and decimal flag.
    /// </summary>
    private void AfterCommit()
    {
        this.CurrentNumber = 0;
        this.decimalInputFlag = false;
        this.negativeValueFlag = false;
        this.Result = string.Empty;
    }

    /// <summary>
    /// <summary>
    /// Commit the current number to the input string and reset the last number value.
    /// </summary>
    private void CommitNumber()
    {
        if (this.CurrentNumber != 0 || this.Expression != string.Empty)
        {
            if (this.Expression != string.Empty && this.LastChar != '^')
            {
                this.UpdateExpression(" ");
            }

            this.UpdateExpression(this.CurrentNumber.ToString());
        }

        this.AfterCommit();
    }

    /// <summary>
    /// After resolving and interaction, notify the view.
    /// </summary>
    private void PostInteraction()
    {
        this.Refocus = !this.Refocus;
    }

    /// <summary>
    /// Actions to run prior resolving any input.
    /// </summary>
    private void PreInput()
    {
        if (this.evaluated)
        {
            this.Reset();
        }
    }

    /// <summary>
    /// Reset the state to intial values.
    /// </summary>
    private void Reset()
    {
        this.evaluated = false;
        this.Expression = string.Empty;
        this.AfterCommit();
    }

    /// <summary>
    /// Update the equation with the given string.
    /// </summary>
    /// <param name="update">The string to append to the expression.</param>
    private void UpdateExpression(string update)
    {
        this.Expression += update;
    }

    /// <summary>
    /// Update the view to display the given result.
    /// </summary>
    /// <param name="result">The result to display.</param>
    private void UpdateResult(string result)
    {
        this.Result = result;
    }
}