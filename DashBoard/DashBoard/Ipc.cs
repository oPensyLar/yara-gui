using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;

namespace DashBoard
{
    class Ipc
    {

        public static int threadcounter = 1;
        public static NamedPipeClientStream client;

        public bool IsConnected()
        {
            if (client == null)
                return false;

            return client.IsConnected;
        }

        public string Read()
        {
            int a = 0x0;
            string ret = null;

            do
            {

                if (client.IsConnected)
                {
                    // {System.ObjectDisposedException: Safe handle has been closed

                    try
                    {
                        a = client.ReadByte();
                    }

                    catch (ObjectDisposedException e)
                    {
                        Close();
                        return null;
                    }

                    if (a == -1)
                        break;

                    ret += Convert.ToChar(a).ToString();

                }

                else
                    a = 0x0;

            } while (a != 0x0);

            return ret;
        }


        public void Close()
        {
            client.Close();
        }


        public int Write(string payload)
        {
            payload += "\0";

            byte[] a = Encoding.ASCII.GetBytes(payload);

            if(client.IsConnected)
            {
                if (client.IsConnected)
                {

                    try
                    {
                        client.Write(a, 0, a.Length);
                        return 0x0;
                    }

                    catch(System.IO.IOException e)
                    {
                        client.Close();
                        return 0x1;
                    }

                }
            }
            
            return 0x1;
        }


        public int Connect()
        {
            client = new NamedPipeClientStream(".", "Scanner", PipeDirection.InOut, PipeOptions.Asynchronous);
            client.Connect();

            // var t1 = new System.Threading.Thread(StartSend);
            // t1.Start();

            // var t2 = new System.Threading.Thread(StartSend);            
            // t2.Start(); 

            return 0x0;
        }

        public static void StartSend()
        {
            int thisThread = threadcounter;
            threadcounter++;

            StartReadingAsync(client);

            for (int i = 0; i < 10000; i++)
            {
                var buf = new byte[1];
                buf[0] = (byte)i;
                client.WriteAsync(buf, 0, 1);

                // Console.WriteLine($@"Thread{thisThread} Wrote: {buf[0]}");
            }
        }

        public static async Task StartReadingAsync(NamedPipeClientStream pipe)
        {
            var bufferLength = 1; 
            byte[] pBuffer = new byte[bufferLength];

            await pipe.ReadAsync(pBuffer, 0, bufferLength).ContinueWith(async c =>
            {
                // Console.WriteLine($@"read data {pBuffer[0]}");
                await StartReadingAsync(pipe); // read the next data <-- 
            });
        }
    }
}
