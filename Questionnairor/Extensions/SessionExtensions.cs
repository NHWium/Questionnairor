using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Questionnairor.Models;

// Inspired by https://docs.microsoft.com/en-gb/aspnet/core/fundamentals/app-state?view=aspnetcore-2.0
// Changed to be specific for this project
namespace Questionnairor.Extensions
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static void Set(this ISession session, string key, Questionnaire value)
        {
            session.SetString(key, value.ToJson(Formatting.None));
        }
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value);
        }
        public static Questionnaire Get(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(Questionnaire) :
                                  Questionnaire.FromJson(value);
        }
    }
}
