using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace WebClient.Models.ViewModels
{
    public class Errors
    {
        public static async Task<string> getErrors(HttpResponseMessage result)
        {
            string errors = "";
            var errorResponse = ((JObject)JsonConvert.DeserializeObject(await result.Content.ReadAsStringAsync()))["errors"];
            foreach (var err in errorResponse)
            {
                var jvalue = ((JProperty)(err)).Value;
                foreach (var ev in jvalue)
                {
                    errors += (ev.ToString() + "<br>");
                }
            }
            return errors;
        }
    }
}