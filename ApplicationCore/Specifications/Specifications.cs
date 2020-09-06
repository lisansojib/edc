using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ApplicationCore.Specifications
{
    public abstract class Specification<T> where T : BaseEntity
    {
        #region Public Fields

        public static readonly Specification<T> All = new IdentitySpecification<T>();

        #endregion Public Fields

        #region Public Properties

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        #endregion

        #region Public Methods

        /// <summary>
        /// "Ands" specifications together to meet multiple criteria
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        public Specification<T> And(Specification<T> specification)
        {
            if (this == All)
            {
                return specification;
            }

            if (specification == All)
            {
                return this;
            }

            return new AndSpecification<T>(this, specification);
        }

        /// <summary>
        /// Determines if an entity meets a specification criteria
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool IsSatisfiedBy(T entity)
        {
            Func<T, bool> predicate = ToExpression().Compile();
            return predicate(entity);
        }

        public Specification<T> Not()
        {
            return new NotSpecification<T>(this);
        }

        public Specification<T> Or(Specification<T> specification)
        {
            if (this == All || specification == All)
            { return All; }

            return new OrSpecification<T>(this, specification);
        }

        public abstract Expression<Func<T, bool>> ToExpression();

        #endregion Public Methods

        #region Protected Methods

        public void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        public void AddIncludes(List<Expression<Func<T, object>>> includeExpressions)
        {
            foreach (var exp in includeExpressions)
            {
                Includes.Add(exp);
            }
        }

        #endregion
    }
    internal sealed class AndSpecification<T> : Specification<T> where T : BaseEntity
    {
        #region Public Constructors

        public AndSpecification(Specification<T> left, Specification<T> right)
        {
            _right = right;
            _left = left;
        }

        #endregion Public Constructors

        #region Public Methods

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> leftExpression = _left.ToExpression();
            Expression<Func<T, bool>> rightExpression = _right.ToExpression();

            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.AndAlso(leftExpression.Body, rightExpression.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

            return finalExpr;
        }

        #endregion Public Methods

        #region Private Fields

        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        #endregion Private Fields
    }

    internal sealed class IdentitySpecification<T> : Specification<T> where T : BaseEntity
    {
        #region Public Methods

        public override Expression<Func<T, bool>> ToExpression()
        {
            return x => true;
        }

        #endregion Public Methods
    }

    internal sealed class NotSpecification<T> : Specification<T> where T : BaseEntity
    {
        #region Private Fields

        private readonly Specification<T> _specification;

        #endregion Private Fields

        #region Public Constructors

        public NotSpecification(Specification<T> specification)
        {
            _specification = specification;
        }

        #endregion Public Constructors

        #region Public Methods

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> expression = _specification.ToExpression();
            UnaryExpression notExpression = Expression.Not(expression.Body);

            return Expression.Lambda<Func<T, bool>>(notExpression, expression.Parameters.Single());
        }

        #endregion Public Methods
    }

    internal sealed class OrSpecification<T> : Specification<T> where T : BaseEntity
    {
        #region Public Constructors

        public OrSpecification(Specification<T> left, Specification<T> right)
        {
            _right = right;
            _left = left;
        }

        #endregion Public Constructors

        #region Public Methods

        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = _left.ToExpression();
            var rightExpression = _right.ToExpression();
            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.OrElse(leftExpression.Body, rightExpression.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

            return finalExpr;
        }

        #endregion Public Methods

        #region Private Fields

        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        #endregion Private Fields
    }

    internal class ParameterReplacer : ExpressionVisitor
    {
        #region Internal Constructors

        internal ParameterReplacer(ParameterExpression parameter)
        {
            _parameter = parameter;
        }

        #endregion Internal Constructors

        #region Protected Methods

        protected override Expression VisitParameter(ParameterExpression node)
            => base.VisitParameter(_parameter);

        #endregion Protected Methods

        #region Private Fields

        private readonly ParameterExpression _parameter;

        #endregion Private Fields
    }
}
