using System.Linq.Expressions;
using System.Text;
using Dapper;

namespace BLogicCodeBase.Visitors;

public class SqlExpressionVisitor : ExpressionVisitor
{
    private readonly StringBuilder _sql = new();
    private readonly DynamicParameters _parameters = new();
    private int _paramIndex = 0;

    public (string Sql, DynamicParameters Parameters) Translate(Expression expression)
    {
        Visit(expression);
        return (_sql.ToString(), _parameters);
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        _sql.Append("(");
        Visit(node.Left);

        _sql.Append(node.NodeType switch
        {
            ExpressionType.Equal => " = ",
            ExpressionType.NotEqual => " <> ",
            ExpressionType.GreaterThan => " > ",
            ExpressionType.GreaterThanOrEqual => " >= ",
            ExpressionType.LessThan => " < ",
            ExpressionType.LessThanOrEqual => " <= ",
            ExpressionType.AndAlso => " AND ",
            ExpressionType.OrElse => " OR ",
            _ => throw new NotSupportedException($"Operator {node.NodeType} is not supported")
        });

        Visit(node.Right);
        _sql.Append(")");
        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Expression != null && node.Expression.NodeType == ExpressionType.Parameter)
        {
            _sql.Append(node.Member.Name);
            return node;
        }

        var value = Expression.Lambda(node).Compile().DynamicInvoke();
        AddParameter(value);
        return node;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        AddParameter(node.Value);
        return node;
    }

    private void AddParameter(object value)
    {
        var paramName = "@p" + _paramIndex++;
        _sql.Append(paramName);
        _parameters.Add(paramName, value);
    }
}
