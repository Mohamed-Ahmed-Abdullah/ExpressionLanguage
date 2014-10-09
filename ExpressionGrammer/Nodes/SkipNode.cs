using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Irony.Compiler;

namespace ExpressionGrammer.Nodes
{
    public class SkipNode : AstNode , IExpressionGenerator
    {
        public SkipNode(AstNodeArgs args) : base(args)
        {
        }

        public Expression GenerateExpression(Expression tree)
        {
            return tree;
        }
    }
}