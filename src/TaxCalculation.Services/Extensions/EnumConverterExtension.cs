using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxCalculation.Persistent.Exceptions;

namespace TaxCalculation.Services.Extensions
{
    public static class EnumConverterExtension
    {
        public static T ToEnum<T>(this string value) where T : struct
        {
            var result = Enum.TryParse(value, true, out T parsedValue);

            if (!result)
            {
                throw new BadRequestException($"Invalid enum value of type '{typeof(T)}'");
            }
            return parsedValue;

        }
    }
}
