using System;
using System.Globalization;

using Alternet.UI.Extensions;

namespace Alternet.UI;

/// <summary>
/// This class is a simple formula evaluator that can parse and evaluate mathematical expressions
/// with basic arithmetic operations such as addition, subtraction, multiplication, and division.
/// It also supports parentheses for grouping expressions and allows for negative numbers.
/// The evaluator processes the expression in a left-to-right manner, respecting operator precedence
/// and supports operations on floating-point numbers.
/// </summary>
public class SimpleFormulaEvaluator
{
    /// <summary>
    /// Represents the styles for parsing numbers in the formula.
    /// </summary>
    public static NumberStyles FormulaNumberStyle
        = NumberStyles.Float | NumberStyles.AllowLeadingSign;

    private readonly string text;
    private int pos;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleFormulaEvaluator"/> class.
    /// </summary>
    /// <param name="expression">The expression to evaluate.</param>
    public SimpleFormulaEvaluator(string expression)
    {
        text = expression.StripSpaces();
    }

    /// <summary>
    /// Evaluates the mathematical expression.
    /// </summary>
    public virtual double Evaluate()
    {
        pos = 0;
        var value = ParseExpression();
        if (pos < text.Length)
            throw new InvalidOperationException($"Unexpected character at position {pos}: {text[pos]}");
        return value;
    }

    /// <summary>
    /// Evaluates an expression for mathematical operations.
    /// </summary>
    /// <returns>The result of the evaluated expression as a double.</returns>
    protected virtual double ParseExpression()
    {
        double left = ParseTerm();
        while (pos < text.Length)
        {
            char op = text[pos];
            if (op == '+' || op == '-')
            {
                pos++;
                double right = ParseTerm();
                left = op == '+' ? left + right : left - right;
            }
            else break;
        }

        return left;
    }

    /// <summary>
    /// Parses a term in the expression.
    /// </summary>
    /// <returns>The evaluated term as a double.</returns>
    protected virtual double ParseTerm()
    {
        double left = ParseFactor();
        while (pos < text.Length)
        {
            char op = text[pos];
            if (op == '*' || op == '/')
            {
                pos++;
                double right = ParseFactor();
                left = op == '*' ? left * right : left / right;
            }
            else break;
        }

        return left;
    }

    /// <summary>
    /// Parses a factor in the expression.
    /// </summary>
    /// <returns>The evaluated factor as a double.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the expression is invalid.</exception>
    protected virtual double ParseFactor()
    {
        if (pos >= text.Length)
            throw new InvalidOperationException("Unexpected end of expression");

        if (text[pos] == '(')
        {
            pos++;
            double value = ParseExpression();
            if (pos >= text.Length || text[pos] != ')')
                throw new InvalidOperationException("Missing closing parenthesis");
            pos++;
            return value;
        }

        return ParseNumber();
    }

    /// <summary>
    /// Parses a number from the expression.
    /// </summary>
    /// <returns>The parsed number as a double.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the expression is invalid.</exception>
    protected virtual double ParseNumber()
    {
        int start = pos;

        while (pos < text.Length && (char.IsDigit(text[pos]) || text[pos] == '.' || text[pos] == '-'))
            pos++;

        string numStr = text.Substring(start, pos - start);

        var parseResult = double.TryParse(
            numStr,
            FormulaNumberStyle,
            CultureInfo.InvariantCulture,
            out double result);
        if (!parseResult)
            throw new InvalidOperationException($"Invalid number: {numStr}");
        return result;
    }
}
