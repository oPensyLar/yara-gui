using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard
{
    class Utils
    {
        public int CheckDbExists()
        {

            Consts c = new Consts();
            string rootPath = Environment.CurrentDirectory;
            string dbPath = rootPath + "\\db\\full." + c.GetConsts(0x2);

            // long length = new System.IO.FileInfo(dbPath).Length;

            return 0x0;
        }

        public int CheckEnviroment()
        {
            CheckDbExists();

            return 0x0;
        }

    }
}
