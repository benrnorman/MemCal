namespace MemCal.Models.DataTypes.Enums;

/// <summary>
/// Represents a valid mathemitcal operator.
/// </summary>
public enum Operation
{
    /// <summary>
    /// Operation returning the product of two numbers.
    /// </summary>
    Multiplication = '*',

    /// <summary>
    /// Operation returning the quotient of two numbers.
    /// </summary>
    Division = '/',

    /// <summary>
    /// Operation returning the sum of two numbers.
    /// </summary>
    Addition = '+',

    /// <summary>
    /// Operation returning the difference between two numbers.
    /// </summary>
    Subtraction = '-',

    /// <summary>
    /// Operation returning the power of two numebrs.
    /// </summary>
    Exponent = '^',

    /// <summary>
    /// Operation that returns the percentage of a number.
    /// </summary>
    Percentage = '%'
}
