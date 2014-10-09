using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Irony.Compiler;

namespace ExpressionGrammer.Nodes
{
    public class BlockNode: AstNode , IExpressionGenerator
    {
        public BlockNode(AstNodeArgs args) : base(args)
        {}

        public Expression GenerateExpression(Expression tree)
        {
            var _2 = Expression.Constant((decimal?)2, typeof(decimal?));
            var _3 = Expression.Constant((decimal?)3, typeof(decimal?));
            var _4 = Expression.Constant((decimal?)4, typeof(decimal?));

            var conditionResult = Expression.Condition(Expression.Equal(_2, _2), _3, _4);

            var add = Expression.Add(_2, conditionResult);
            var mult = Expression.Multiply(add, _4);

            var lambda1 = Expression.Lambda<Func<decimal?>>(
                    mult,
                    new ParameterExpression[] { });
            return lambda1;
        }
    }
}
