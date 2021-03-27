using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard
{
    class Consts
    {
        private string getStatusScanFiles = "{\"scan\" : {\"info\" : 2}}";
        string payloadThreads = "{\"threads\" : {\"flag\" : 1}}";
        private const string dbExtension = ".amdb";

        public string GetConsts(int flag)
        {
            switch(flag)
            {
                case 0x1:
                    return getStatusScanFiles;

                case 0x2:
                    return dbExtension;

                case 0x3:
                    return payloadThreads;
            }

            return null;
        }
    }
}
