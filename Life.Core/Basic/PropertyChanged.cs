using System;
using System.Linq.Expressions;

namespace Life.Core.Basic
{
    public class PropertyChanged
    {
        #region Constructors

        public PropertyChanged(object sender, Expression<Func<object>> func)
        {
            Sender = sender;
            PropertyName = GetName(func);
            Value = func.Compile()();
        }

        #endregion Constructors

        #region Public Properties

        public string PropertyName
        {
            get; private set;
        }

        public object Sender
        {
            get; private set;
        }

        public object Value
        {
            get; private set;
        }

        #endregion Public Properties

        #region Public Methods

        private static string GetName(Expression<Func<object>> exp)
        {
            var body = exp.Body as MemberExpression;
            if (body == null)
            {
                var ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member.Name;
        }

        #endregion Public Methods
    }
}