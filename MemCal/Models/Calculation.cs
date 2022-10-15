namespace MemCal.Models;

using org.mariuszgromada.math.mxparser;

/// <summary>
/// Represents an expression and result.
/// </summary>
public class Calculation
{
    // CONSTRUCTORS
    // ============

    /// <summary>
    /// Creates a new Calculation.
    /// </summary>
    /// <param name="expression">The expression to add on creation.</param>
    public Calculation(Expression expression)
    {
        this.Expression = expression;
    }



    // PROPERTIES
    // ============

    /// <summary>
    /// Gets the mathematical expression for this Calculation.
    /// </summary>
    public Expression Expression { get; }

    /// <summary>
    /// Gets the expression as a string.
    /// </summary>
    public string ExpressionString
    {
        get => this.Expression.getExpressionString();
    }

    /// <summary>
    /// Gets the result of the Calculation.
    /// </summary>
    public string Result
    {
        get => this.Expression.calculate().ToString();
    }



    // METHODS
    // ============

    /// <summary>
    /// Overrides the default ToString method.
    /// </summary>
    /// <returns>The expression and result.</returns>
    public override string ToString()
    {
        return $"{this.ExpressionString} = {this.Result}";
    }
}
