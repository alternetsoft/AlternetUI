using System;
using System.Globalization;

namespace Alternet.UI;

internal class SimpleFormulaEvaluator
{
    private readonly string text;
    private int pos;

    public SimpleFormulaEvaluator(string expression)
    {
        text = expression.Replace(" ", string.Empty); // strip spaces for simplicity
    }

    public virtual double Evaluate()
    {
        pos = 0;
        var value = ParseExpression();
        if (pos < text.Length)
            throw new InvalidOperationException($"Unexpected character at position {pos}: {text[pos]}");
        return value;
    }

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

    protected virtual double ParseNumber()
    {
        int start = pos;
        while (pos < text.Length && (char.IsDigit(text[pos]) || text[pos] == '.' || text[pos] == '-'))
            pos++;

        string numStr = text.Substring(start, pos - start);
        if (!double.TryParse(numStr, NumberStyles.Float | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out double result))
            throw new InvalidOperationException($"Invalid number: {numStr}");
        return result;
    }
}
