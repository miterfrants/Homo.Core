using System.Collections.Generic;
using System.Threading;
using System.IO;
using Newtonsoft.Json;

namespace Homo.Core.Helpers
{
    public class ErrorHelper
    {
        private static Dictionary<string, Dictionary<string, string>> errorMapping = new Dictionary<string, Dictionary<string, string>>();
        public static string GetErrorMessageByCode(string code, string path)
        {
            string languageCode = Thread.CurrentThread.CurrentCulture.Name;
            if (!Directory.Exists(path))
            {
                return code;
            }

            if (!errorMapping.ContainsKey(languageCode))
            {
                errorMapping.Add(languageCode, JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText($"{path}/{languageCode}.json")));
            }

            if (
                errorMapping.ContainsKey(languageCode)
                && errorMapping[languageCode] != null
                && errorMapping[languageCode].ContainsKey(code)
            )
            {
                return errorMapping[languageCode][code];
            }
            else
            {
                return code;
            }
        }
    }

}
