using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGrammer.Nodes
{
    public interface IExpressionGenerator
    {
        Expression GenerateExpression(Expression tree);
    }
}
