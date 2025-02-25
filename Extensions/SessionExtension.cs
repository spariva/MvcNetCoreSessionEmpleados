using Newtonsoft.Json;

namespace MvcNetCoreSessionEmpleados.Extensions
{
    public static class SessionExtension
    {

        public static void setObject(this ISession session, string key, object value)
        {
            string data = JsonConvert.SerializeObject(value);
            session.SetString(key, data);
        }

        public static T getObject<T>(this ISession session, string key)
        {
            string data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }

            T value = JsonConvert.DeserializeObject<T>(data);
            return value;
        }
    }
}
