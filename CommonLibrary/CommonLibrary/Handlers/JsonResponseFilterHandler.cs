using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Handlers
{
    public static class JsonResponseFilterHandler
    {
        public static string Filter(string jsonString)
        {
            // Use JsonDeserialize<T> to check if string is of type T
            /*if (JsonDeserialize<StrataReportResponse>(jsonString, out StrataReportResponse obj))
            {
                if (obj.image != null && obj.image != "" && obj.image.Length > 250)
                {
                    obj.image = "base64 image not stored";
                    return JsonConvert.SerializeObject(obj);
                }
            }*/

            return jsonString;
        }

        private static bool JsonDeserialize<T>(string json, out T obj)
        {
            try
            {
                obj = JsonConvert.DeserializeObject<T>(json);
                return true;
            }
            catch (Exception ex)
            {
                obj = default(T);
                return false;
            }
        }
    }
}
