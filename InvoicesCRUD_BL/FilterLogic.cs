using DAL_EF_SQLite;
using Invoices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace InvoicesCRUD_BL
{
    internal static class QueryFilterProcesser
    {
        private static void Logic()
        {
            Type type = typeof(Invoice);
            var properties = type.GetProperties().ToDictionary(prop => prop.Name, prop => prop.PropertyType);

            var comparandParser = new Dictionary<Type, Func<string, (bool, object)>>();
            comparandParser.Add(typeof(int),
                comperand => { bool isParsed = int.TryParse(comperand, out int value); return (isParsed, value); });
            comparandParser.Add(typeof(double),
                comperand => { bool isParsed = double.TryParse(comperand, out double value); return (isParsed, value); });
            comparandParser.Add(typeof(DateTime),
                comperand => { bool isParsed = DateTime.TryParse(comperand, out DateTime value); return (isParsed, value); });
            comparandParser.Add(typeof(ProcessingStatuses),
                comperand => { bool isParsed = int.TryParse(comperand, out int value); return (isParsed, (ProcessingStatuses)value); });
            comparandParser.Add(typeof(PaymentMethods),
                comperand => { bool isParsed = int.TryParse(comperand, out int value); return (isParsed, (PaymentMethods)value); });

            var comparisonPrecicates = new Dictionary<(Type, string), Func<object, object, bool>>();

            //int
            comparisonPrecicates.Add((typeof(int), ">"), (value, comparand) => (int)value > (int)comparand);
            comparisonPrecicates.Add((typeof(int), ">="), (value, comparand) => (int)value >= (int)comparand);
            comparisonPrecicates.Add((typeof(int), "<"), (value, comparand) => (int)value < (int)comparand);
            comparisonPrecicates.Add((typeof(int), "<="), (value, comparand) => (int)value <= (int)comparand);
            comparisonPrecicates.Add((typeof(int), "=="), (value, comparand) => (int)value == (int)comparand);
            comparisonPrecicates.Add((typeof(int), "!="), (value, comparand) => (int)value != (int)comparand);

            //double
            comparisonPrecicates.Add((typeof(double), ">"), (value, comparand) => (double)value > (double)comparand);
            comparisonPrecicates.Add((typeof(double), ">="), (value, comparand) => (double)value >= (double)comparand);
            comparisonPrecicates.Add((typeof(double), "<"), (value, comparand) => (double)value < (double)comparand);
            comparisonPrecicates.Add((typeof(double), "<="), (value, comparand) => (double)value <= (double)comparand);
            comparisonPrecicates.Add((typeof(double), "=="), (value, comparand) => (double)value == (double)comparand);
            comparisonPrecicates.Add((typeof(double), "!="), (value, comparand) => (double)value != (double)comparand);

            //datetime
            comparisonPrecicates.Add((typeof(DateTime), ">"), (value, comparand) => (DateTime)value > (DateTime)comparand);
            comparisonPrecicates.Add((typeof(DateTime), ">="), (value, comparand) => (DateTime)value >= (DateTime)comparand);
            comparisonPrecicates.Add((typeof(DateTime), "<"), (value, comparand) => (DateTime)value < (DateTime)comparand);
            comparisonPrecicates.Add((typeof(DateTime), "<="), (value, comparand) => (DateTime)value <= (DateTime)comparand);
            comparisonPrecicates.Add((typeof(DateTime), "=="), (value, comparand) => (DateTime)value == (DateTime)comparand);
            comparisonPrecicates.Add((typeof(DateTime), "!="), (value, comparand) => (DateTime)value != (DateTime)comparand);

            //var propName = "creation";
            //var comparer = ">=";
            //var propType = properties[propName];
            //var func = precicates[(propType, comparer)];
        }

        private static Dictionary<Type, Dictionary<string, Type>> _properties = new Dictionary<Type, Dictionary<string, Type>>();
        private static MethodInfo _containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

        //https://stackoverflow.com/questions/24817283/reflection-in-entity-framework-c-sharp
        //https://stackoverflow.com/questions/8315819/expression-lambda-and-query-generation-at-runtime-simplest-where-example
        //https://stackoverflow.com/questions/278684/how-do-i-create-an-expression-tree-to-represent-string-containsterm-in-c
        //https://blog.marcgravell.com/2008/10/express-yourself.html
        private static Expression<Func<T, bool>> LambdaConstructor<T>(string propertyName, string comparer, string comparand)
        {
            var item = Expression.Parameter(typeof(T), "item");
            var prop = Expression.Property(item, propertyName);
            if (!_properties.ContainsKey(typeof(T)))
            {
                var properties = typeof(T).GetProperties().ToDictionary(prop => prop.Name.ToLowerInvariant(), prop => prop.PropertyType);
                _properties.TryAdd(typeof(T), properties);
            }
            var propertyType = _properties[typeof(T)][propertyName.ToLowerInvariant()];
            ConstantExpression value;
            try
            {
                if (propertyType.IsEnum)
                {
                    value = Expression.Constant(Enum.Parse(propertyType, comparand));
                }
                else
                {
                    value = Expression.Constant(Convert.ChangeType(comparand, propertyType));
                }
            }
            catch (InvalidCastException ex)
            {
                throw new NotSupportedException();
            }

            Expression predicateExp = default;
            if (typeof(T) == typeof(string))
            {
                switch (comparer)
                {
                    case "==":
                        predicateExp = Expression.Call(prop, _containsMethod, value);
                        break;
                    case "!=":
                        predicateExp = Expression.IsFalse(Expression.Call(prop, _containsMethod, value));
                        break;
                }
            }
            if (predicateExp == default)
            {
                switch (comparer)
                {
                    case "===":
                        predicateExp = Expression.Equal(prop, value);
                        break;
                    case "!==":
                        predicateExp = Expression.NotEqual(prop, value);
                        break;
                    case ">":
                        predicateExp = Expression.GreaterThan(prop, value);
                        break;
                    case ">=":
                        predicateExp = Expression.GreaterThanOrEqual(prop, value);
                        break;
                    case "<":
                        predicateExp = Expression.LessThan(prop, value);
                        break;
                    case "<=":
                        predicateExp = Expression.LessThanOrEqual(prop, value);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
            var lambda = Expression.Lambda<Func<T, bool>>(predicateExp, item);
            return lambda;
        }

        public static Expression<Func<T, bool>> ProcessFilter<T>(this string input)
        {
            //find input start index -- next char index
            //find field end index -- last char index
            //find comparer start index -- next char index
            //find comparer end index -- last char index
            //find comparand start index  -- next char index
            //find input end index -- last char index in string


            var fieldStart = input.FindWordStart(0);
            var fieldEnd = input.FindWordEnd(fieldStart + 1);
            var comparerStart = input.FindWordStart(fieldEnd + 2);
            var comparerEnd = input.FindWordEnd(comparerStart + 1);
            var comparandStart = input.FindWordStart(comparerEnd + 2);
            var comparandEnd = input.FindTextEnd(comparandStart);

            var fieldName = input.Substring(fieldStart, fieldEnd - fieldStart + 1);
            var comparerText = input.Substring(comparerStart, comparerEnd - comparerStart + 1);
            var comparandText = input.Substring(comparandStart, comparandEnd - comparandStart + 1);

            var predicate = LambdaConstructor<T>(fieldName, comparerText, comparandText);
            return predicate;
        }

        private static int FindWordStart(this string text, int fromIndex)
        {
            if (text?.Length > 0)
            {
                for (int i = fromIndex; i < text.Length; i++)
                {
                    if (text[i] != ' ')
                        return i;
                }
            }

            return -1;
        }
        private static int FindWordEnd(this string text, int fromIndex)
        {
            if (text?.Length > 0)
            {
                for (int i = fromIndex; i < text.Length; i++)
                {
                    if (text[i] == ' ')
                        return i - 1;
                }
            }

            return -1;
        }
        private static int FindTextEnd(this string text, int fromIndex = 0)
        {
            if (text?.Length > 0)
            {
                for (int i = text.Length - 1; i >= fromIndex; i--)
                {
                    if (text[i] != ' ')
                        return i;
                }
            }

            return -1;
        }
    }
}
