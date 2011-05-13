using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Linq.Expressions;


namespace TSDTReports.Models
{
    public static class PredicateExtensions
    {



        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        public static Expression<Func<T, bool>> False<T>() { return f => false; }


        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression1,

                                                            Expression<Func<T, bool>> expression2)
        {

            var invokedExpression = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, bool>>

                  (Expression.Or(expression1.Body, invokedExpression), expression1.Parameters);

        }



        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression1,

                                                            Expression<Func<T, bool>> expression2)
        {

            var invokedExpression = Expression.Invoke(expression2, expression1.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, bool>>

                  (Expression.And(expression1.Body, invokedExpression), expression1.Parameters);

        }



    }
}
