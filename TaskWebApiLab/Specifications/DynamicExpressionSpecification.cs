using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Specifications
{
    public class DynamicExpressionSpecification<T> : ExpressionSpecification<T>
    {
        public DynamicExpressionSpecification(Expression<Func<T, bool>> expression) : base(expression)
        {
        }
    }
}
