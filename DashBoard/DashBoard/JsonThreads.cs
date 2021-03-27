using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard
{
    class JsonThreads
    {
        public int native_thread_counter { get; set; }
        public int master_thread_counter { get; set; }
    }

    class JsonCurrentScan
    {
        public int scans { get; set; }
        public int omitted { get; set; }
        public int infected { get; set; }
    }

    class JsonCurrentFile
    {
        public string filePath { get; set; }
    }
}
