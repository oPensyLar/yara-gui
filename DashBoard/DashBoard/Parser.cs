using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;

namespace DashBoard
{

    class Parser
    {

        public JsonCurrentScan cs = new JsonCurrentScan();
        public JsonCurrentFile cf = new JsonCurrentFile();

        public JsonThreads ThreadsResponse(string jsonData)
        {
            if(jsonData != null)
            {
                JObject jObj = JObject.Parse(jsonData);
                string strThreads = jObj.SelectToken("threads").ToString();
                JsonThreads jThreads = JsonConvert.DeserializeObject<JsonThreads>(strThreads);
                return jThreads;
            }

            return null;
        }
      

        public void FileScanned(string raw_data)
        {

            JObject jObj = new JObject();
            try
            {
                jObj = JObject.Parse(raw_data);
            }

            // Exception Newtonsoft.Json.JsonReaderException
            catch
            {
                return;
            }

            foreach (KeyValuePair<string, JToken> sub_obj in (JObject)jObj["current_file"])
            {
                if (sub_obj.Key.CompareTo("file_path") == 0x0)
                    cf.filePath = sub_obj.Value.ToString();
            }


            foreach (KeyValuePair<string, JToken> sub_obj in (JObject)jObj["current_scan"])
            {
                if(sub_obj.Key.CompareTo("scans")==0x0)
                    cs.scans = (int) sub_obj.Value;

                if (sub_obj.Key.CompareTo("omitted") == 0x0)
                    cs.omitted = (int) sub_obj.Value;

                if (sub_obj.Key.CompareTo("infected") == 0x0)
                    cs.infected = (int) sub_obj.Value;
            }
        }
    }
}
