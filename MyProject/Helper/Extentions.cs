using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Project.Helper
{
    public static class Extentions
    {
        public static string Jsonify<T>(this T obj)
        {
            DataContractJsonSerializer s = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings
            {
                DateTimeFormat = new DateTimeFormat("yyyy-MM-dd")
            });
            using (MemoryStream stream = new MemoryStream())
            {
                s.WriteObject(stream, obj);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    return json;
                }
            }
        }
    }
}
