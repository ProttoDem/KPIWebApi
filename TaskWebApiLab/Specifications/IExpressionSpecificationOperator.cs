using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specifications
{
    internal interface IExpressionSpecificationOperator
    {
        ExpressionSpecification<TModel> Combine<TModel>(ExpressionSpecification<TModel> left, ExpressionSpecification<TModel> right);
    }
}
