using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Specifications
{
    internal class ExpressionSpecificationAndOperator : IExpressionSpecificationOperator
    {
        public ExpressionSpecification<TModel> Combine<TModel>(ExpressionSpecification<TModel> left, ExpressionSpecification<TModel> right)
        {
            Expression<Func<TModel, bool>> resultExpression;
            ParameterExpression param = left.Expression.Parameters.First();
            if (ReferenceEquals(param, right.Expression.Parameters.First()))
            {
                resultExpression = Expression.Lambda<Func<TModel, bool>>(
                    Expression.AndAlso(left.Expression.Body, right.Expression.Body), param);
            }
            else
            {
                resultExpression = Expression.Lambda<Func<TModel, bool>>(
                    Expression.AndAlso(
                        left.Expression.Body,
                        Expression.Invoke(right.Expression, param)), param);
            }

            var combinedSpecification = new DynamicExpressionSpecification<TModel>(resultExpression);
            return combinedSpecification;
        }
    }

    public static class ExpressionSpecificationAndOperatorExtension
    {
        public static ExpressionSpecification<T> And<T>(this ExpressionSpecification<T> specificationLeft, ExpressionSpecification<T> specificationRight)
        {
            var specificationAndOperator = new ExpressionSpecificationAndOperator();
            ExpressionSpecification<T> expressionSpecification = specificationAndOperator.Combine(specificationLeft, specificationRight);
            return expressionSpecification;
        }
    }
}
