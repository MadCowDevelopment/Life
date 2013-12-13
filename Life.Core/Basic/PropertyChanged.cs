using System;
using System.Linq.Expressions;

namespace Life.Core.Basic
{
    public class PropertyChanged<TSender, TValue>
    {
        #region Constructors

        public PropertyChanged(TSender sender, Expression<Func<TValue>> func)
        {
            Sender = sender;
            Value = func.Compile()();
            PropertyName = GetMemberName(func.Body);
        }

        #endregion Constructors

        #region Public Properties

        public string PropertyName
        {
            get; private set;
        }

        public TSender Sender
        {
            get; private set;
        }

        public TValue Value
        {
            get; private set;
        }

        #endregion Public Properties

        #region Public Methods

        public string GetMemberName(Expression expression)
        {
            var memberExpression = expression as MemberExpression;
            if (memberExpression != null)
            {
                return memberExpression.Member.Name;
            }

            throw new InvalidOperationException();
        }

        #endregion Public Methods
    }
}