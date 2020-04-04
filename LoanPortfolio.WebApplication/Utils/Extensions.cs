using System;

namespace LoanPortfolio.WebApplication
{
    public static class Extensions
    {
        public static DateTime SetTime(this DateTime value, int hour, int minutes)
        {
            if (hour >= 24) hour = 23;
            if (minutes >= 60) minutes = 59;
            value = value.AddSeconds(-value.Second).AddMinutes(-value.Minute).AddHours(-value.Hour);
            return value.AddHours(hour).AddMinutes(minutes);
        }
    }
}