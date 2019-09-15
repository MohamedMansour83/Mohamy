using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElMaitre.Web.Extensions
{
    public static class Extensions
    {

        public static IEnumerable<String> GetErrors(this ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(v => v.Errors)
                                    .Select(v => v.ErrorMessage + " " + v.Exception).ToList();
        }


        public static int CalculateAge(this DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }

        public static DateTime ToEgyptTimezone(this DateTime dateOfBirth)
        {
            var info = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

            DateTimeOffset localServerTime = DateTimeOffset.Now;
            DateTimeOffset usersTime = TimeZoneInfo.ConvertTime(localServerTime, info);
            return usersTime.DateTime;
        }


        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }
    }

    
}
