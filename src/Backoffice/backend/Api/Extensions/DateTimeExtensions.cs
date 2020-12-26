using System;
using System.Globalization;

namespace CanaryDeliveries.Backoffice.Api.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToISO8601(this DateTime datetime)
        {
            return datetime.ToString("o", CultureInfo.InvariantCulture);
        }
    }
}