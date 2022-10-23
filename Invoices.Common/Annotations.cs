using System.ComponentModel.DataAnnotations;

namespace Invoices.Common
{
    public class RequiredEnumFieldAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            var type = value.GetType();
            return type.IsEnum && Enum.IsDefined(type, value);
        }
    }
    public class OptionalEnumFieldAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var type = value.GetType();
            return type.IsEnum && Enum.IsDefined(type, value);
        }
    }
    public class RequiredDoubleFieldAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }

            var type = value.GetType();
            return type == typeof(double) && (double)value != 0;
        }
    }
    public class OptionalDoubleFieldAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var type = value.GetType();
            return type == typeof(double) && (double)value != 0;
        }
    }
}
