using System;
using Newtonsoft.Json;

namespace MyDemo.Core
{
    public class GeneralHelper
    {
        /// <summary>
        /// Deserializes the json response string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString">The json string.</param>
        /// <returns></returns>
        public static T DeserializeJsonResponseString<T>(string jsonString)
        {
            var result = JsonConvert.DeserializeObject<T>(jsonString);
            return result;
        }

        /// <summary>
        /// Serialzes to json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeObj">The type object.</param>
        /// <returns></returns>
        public static string SerialzeToJson<T>(T typeObj)
        {
            string result = JsonConvert.SerializeObject(typeObj);
            return result;
        }
    }
}

