using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Hardcodet.Wpf.TaskbarNotification.Interop;
using System.Net;

namespace DashBoard
{

    /// <summary>
    /// Interaction logic for Main2.xaml
    /// </summary>
    public partial class Main2 : Window
    {
        Ipc ipc = new Ipc();
        string appName = "AntiMalwarev1";

        /**
         * Modify lbl_remove_object
         * @content string to lbl_remove_object
        */
        void ModRemoveObjects(int content)
        {
            lbl_remove_objects.Content = content + " ficheros";
        }


        /**
        * Modify lbl_omitidos_objects
        * @content string to lbl_omitidos_objects
        */
        void ModOmitidosObjects(int content)
        {
            lbl_omitidos_objects.Content = content + " ficheros";
        }


        /**
        * Modify lbl_threats_found
        * @content string to lbl_threats_found
        */
        void ModThreatsFound(int content)
        {
            lbl_threats_found.Content = content + " ficheros";
        }

        /**
         * Modify lbl_total_scan_objects.
         * @content string to lbl_file_scanned
        */
        public void ModTotalScanObjects(int content)
        {
            lbl_total_scan_objects.Content = content.ToString() +  " ficheros";
        }



        /**
         * Modify lbl_engine_version
         * @content string to lbl_engine_version
        */
        void ModEngineVersion(string content)
        {
            lbl_engine_version.Content = content;
        }

        /**
         * Modify lbl_db_version
         * @content string to lbl_db_version
        */
        void ModDbVersion(string content)
        {
            lbl_db_version.Content = content;
        }

        /**
         * Modify lbl_file_scanned.
         * @content string to lbl_file_scanned
        */
        public int ModFileScanned(string content)
        {

            if (content.Length > 25)
            {
                content = content.Remove(20, content.Length-20);
                content += " ...";
            }
                
            lbl_file_scanned.Content = content;
            return 0x0;
        }

        private void IpcConnect()
        {
            ipc.Connect();
            // IpcWriteThreads();

            // Lee();
        }


        private void IpcGetFileScanned()
        {
            Parser p = new Parser();            

            while(true)
            {

                Consts c = new Consts();
                ipc.Connect();                
                ipc.Write(c.GetConsts(0x1));
                p.FileScanned(ipc.Read());

                this.Dispatcher.Invoke(() =>
                {

                    if(btn_log_extend.IsVisible == false)
                    {
                        btn_log_extend.Visibility = Visibility.Visible;
                    }

                    ModFileScanned(p.cf.filePath);
                    ModFileScanned(p.cf.filePath);
                    ModTotalScanObjects(p.cs.scans);
                    ModOmitidosObjects(p.cs.omitted);
                });

                ipc.Close();

                Thread.Sleep(100);

            }            
        }

        /**
         * Modify lbl_file_scanned.
         * @param (Solo se chequea el index 0x0) que indica el flag que debe hacer
         * cuando existe scan threads en el engine, 0x2 para llamar a IpcGetFileScanned()
         * 0x1 para llamar a IpcKillScanThreards()
        */
        private void IpcGetScanThreads(object flagDoing)
        {
            Consts c = new Consts();

            if(ipc.IsConnected() == false)
                ipc.Connect();            

            ipc.Write(c.GetConsts(0x3));
            string ipcJson = ipc.Read();
            ipc.Close();

            if(ipcJson != null)
            {
                Array a = new object[0x1];
                a = (Array)flagDoing;
                int oneParm = (int)a.GetValue(0);

                Parser p = new Parser();
                JsonThreads jThreads = p.ThreadsResponse(ipcJson);

                // Flag what do?
                if (jThreads.master_thread_counter > 0x0)
                {
                    switch (oneParm)
                    {
                        case 0x1:
                            // IpcKillScanThreards();
                        break;

                        case 0x2:
                            IpcGetFileScanned();
                        break;
                    }
                }                

            }


        }

        private void IpcWriteScan()
        {
            ipc.Connect();
            string payloadScan = "{\"scan\" : {\"path\" : [\"D:\", \"P:\"], \"recursive\" : 1, \"threads\" : 1, \"rules\" : \"P:\\\\movibles\\\\yara-rules\\\\rules\\\\antidebug_antivm_index.yar\"}}";
            ipc.Write(payloadScan);
        }

        public Main2()
        {
            InitializeComponent();

            if(ipc.IsConnected() == false)
            {
                Utils u = new Utils();
                u.CheckEnviroment();

                ModEngineVersion("Desconocido");
                ModDbVersion("Nunca");
                ModFileScanned("Ninguno");
                ModTotalScanObjects(0);
                ModRemoveObjects(0);
                ModThreatsFound(0);
                ModOmitidosObjects(0);

            }


            // 0x2 flag para mostrar los ficheros scaneados
            object prmFlag = new object[0x1] { 0x2 };
            var t1 = new Thread(new ParameterizedThreadStart(IpcGetScanThreads));
            t1.Start(prmFlag);        
            
            // this.WindowState = System.Windows.WindowState.Minimized;

            /* Hardcodet.Wpf.TaskbarNotification.TaskbarIcon tbi = new Hardcodet.Wpf.TaskbarNotification.TaskbarIcon();
            System.Drawing.Icon u = new System.Drawing.Icon(@"C:\Users\opensylar\Downloads\icon-dashboard-csharp.ico");
            tbi.Icon = u;
            tbi.ToolTipText = appName;*/

        }

        private void btn_log_extend_Btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow mWin2 = new MainWindow();
            mWin2.Show();
        }

        private void back_Btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void close_Btn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        public async Task<int> Lee()
        {
           
            string json_ipc;

            while(true)
            {

                json_ipc = ipc.Read();

                if (json_ipc != null)
                {
                    Parser p = new Parser();
                    p.FileScanned(json_ipc);
                    ModFileScanned(p.cf.filePath);
                    ModTotalScanObjects(p.cs.scans);
                    ModOmitidosObjects(p.cs.omitted);
                }

                await Task.Delay(100);
            }

            return 0x0;
        }


        public async Task<int> Escribe()
        {
            IpcWriteScan();
            return 0x0;
        }

        public async Task<int> ScanFile()
        {
            int counter = 0x0;
            int remov_obj = 0x0;
            int threats_obj = 0x0;
            int omitidos_obj = 0x0;

            while (counter != 100000000)
            {
                await Task.Delay(100);

                if(counter % 50 == 0x0)
                {
                    threats_obj++;
                    lbl_threats_found.Content = threats_obj + "/" + threats_obj + " ficheros";
                }

                if (counter % 70 == 0x0)
                {
                    omitidos_obj++;
                    lbl_omitidos_objects.Content = omitidos_obj + "/" + omitidos_obj + " ficheros";
                }
               
                if(counter % 20 == 0x0)
                {
                    remov_obj++;
                    lbl_remove_objects.Content = remov_obj + "/" + remov_obj + " ficheros";
                }                

                lbl_total_scan_objects.Content = counter + " ficheros";

                counter++;
            }

            return 0x0;
        }

        
        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            lbl_db_version.Content = e.ProgressPercentage + " % descargada";
        }


        public void download()
        {
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;

                wc.DownloadFileAsync(
                    new System.Uri("http://192.168.1.218/large-db.amdb"), 
                    "P:\\movibles\\large-db.amdb");
            }
        }

        public async Task<int> UpdateDbEngine()
        {

            download();

            /* int status = 0x0;

             while(status != 100)
            {
                await Task.Delay(1000);
                lbl_db_version.Content = status + " % descargada";
                lbl_engine_version.Content = status + " % descargada";
                status++;                
            } */

            return 0x0;
        }

        private async void btn_update_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                await UpdateDbEngine();
            }

            catch (Exception)
            {
                // Process the exception if one occurs.
            }
        }

        private void btn_imgSettings_Click(object sender, RoutedEventArgs e)
        {
            /*ScanOptions scnOptions = new ScanOptions();
            scnOptions.Show();*/
        }

        private async void btn_analisis_Click(object sender, RoutedEventArgs e)
        {

            ScanOptions scnOptions = new ScanOptions();
            scnOptions.Show();

        }

        private void fn2(object sender, RoutedEventArgs e)
        {
            MyAccount accountWin = new MyAccount();
            accountWin.Show();

        }

        private void fn1(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("fn1");
        }
    }
}
