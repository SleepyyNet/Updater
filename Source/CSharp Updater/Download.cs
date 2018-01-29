using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CSharp_Updater
{
    public partial class Download : Form
    {
        public string[] args = new string[] { };
        public string logPath { set; get; } = ".\\downloader.log";
        private Utils util = null;

        Version oldVersion = null;
        Version newVersion = null;

        public Download(string[] args)
        {
            InitializeComponent();
            this.args = args;
            util = new Utils();
            // wait for rendering to complete and start update check in event function
            this.Shown += new System.EventHandler(this.Download_Shown);
        }

        private void Download_Shown(object sender, EventArgs e)
        {
            DoUpdate(args);
        }

        public class DownloadInformation
        {
            // Parameters
            //   1. ApplicationName with extension
            //   2. Version // Style "X.X.X.X" == "Major.Minor.Build.Revision"
            //   3. DownloadLinkUpdate
            //   4. DownloadLinkUpdateXML
            //   5. XMLTagNames
            //      XMLTagNames have to be seperated by "|"
            //      First TagName = Version (String)
            //      Second TagName = DownloadLink (String)
            //   6. Description
            //   7. DownloadFolder
            public string applicationName { get; set; }
            public string version { get; set; }
            public string downloadLinkUpdate { get; set; }
            public string downloadLinkUpdateXML { get; set; }
            public string[] XMLTagNames { get; set; }
            public string description { get; set; }
            public string downloadFolder { get; set; }
        }

        public DownloadInformation downloadInformation = new DownloadInformation { };
        
        public async void DoUpdate(object data)
        {
            this.Invoke((Func<System.Windows.Forms.Label, string, bool>)DoChangeLabel, label_Version, Application.ProductVersion);
            this.Invoke((Func<System.Windows.Forms.Label, string, bool>)DoChangeLabel, label_Content, "");
            this.Invoke((Func<System.Windows.Forms.Label, string, bool>)DoChangeLabel, label_Speed, "");
            this.Invoke((Func<System.Windows.Forms.Label, string, bool>)DoChangeLabel, label_Status, "");

            // test and set parameters --> parameters valid after this function! --> use downloadInformation from here on
            if (FillDownloadInformation(args))
            {
                // new Version is set here
                if (CheckForUpdate())
                {
                    if (IsNewVersionHigher())
                    {
                        if (MessageBox.Show("Es ist eine neue Version für " + Path.GetFileNameWithoutExtension(downloadInformation.applicationName) + "(" + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + "." + newVersion.Revision + ") verfügbar - Möchten Sie diese Version herunterladen?", "Download verfügbar", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            // start the task and instantly wait for it..
                            if (await Task.Run(() => DoDownload(downloadInformation.applicationName, downloadInformation.downloadLinkUpdate, downloadInformation.description, downloadInformation.downloadFolder)))
                            {
                                MessageBox.Show("Die neue Version des Programms " + Path.GetFileNameWithoutExtension(downloadInformation.applicationName) + " (" + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + "." + newVersion.Revision + ") wurde heruntergeladen.", "Download verarbeitet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                // update had an error
                                MessageBox.Show("Das Update wird aufgrund eines Fehlers abgebrochen!", "Update abgebrochen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            // update was denied
                            MessageBox.Show("Das Update wurde abgelehnt!", "Update abgebrochen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        // new version is not higher than current
                        MessageBox.Show("Es ist kein Update verfügbar!", "Update abgebrochen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    // new version is not higher than current
                    MessageBox.Show("Es ist konnten keine Updateinformationen heruntergeladen werden!", "Update abgebrochen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // not all parameters valid
                MessageBox.Show("Das Update wird aufgrund fehlerhafter Informationen abgebrochen!", "Update abgebrochen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            Application.Exit();
        }

        public bool FillDownloadInformation(string[] args)
        {
            if (args.Length >= 7)
            {
                // set parameters
                try
                {
                    // Set Parameters
                    //   1. ApplicationName with extension
                    downloadInformation.applicationName = args[0];
                    //   2. Version
                    downloadInformation.version = args[1];
                    //   3. DownloadLinkUpdate
                    downloadInformation.downloadLinkUpdate = args[2];
                    //   4. DownloadLinkUpdateXML
                    downloadInformation.downloadLinkUpdateXML = args[3];
                    //   5. DownloadLinkUpdateXMLSchema
                    downloadInformation.XMLTagNames = args[4].Split('|');
                    //   6. Description
                    downloadInformation.description = args[5];
                    //   7. DownloadFolder
                    downloadInformation.downloadFolder = args[6];
                }
                catch (System.IndexOutOfRangeException ex)
                {
                    if (ex.InnerException != null)
                    {
                        util.log(ex.InnerException.ToString(), logPath);
                    }
                    else
                    {
                        util.log(ex.ToString(), logPath);
                    }
                    return false;
                }

                //test parameters
                //   1. ApplicationName with extension
                if (downloadInformation.applicationName == "")
                {
                    return false;
                }
                //   2. Version
                if (downloadInformation.version == "")
                {
                    return false;
                }
                else
                {
                    try
                    {
                        oldVersion = new Version(downloadInformation.version);
                    }
                    catch (System.FormatException ex)
                    {
                        if(ex.InnerException != null)
                        {
                            util.log(ex.InnerException.ToString(), logPath);
                        }
                        else
                        {
                            util.log(ex.ToString(), logPath);
                        }

                        return false;
                    }
                }
                //   3. DownloadLinkUpdate
                if (downloadInformation.downloadLinkUpdate == "")
                {
                    return false;
                }
                //   4. DownloadLinkUpdateXML
                if (downloadInformation.downloadLinkUpdateXML == "")
                {
                    return false;
                }
                //   5. DownloadLinkUpdateXMLSchema
                if(downloadInformation.XMLTagNames.Length >= 1)
                {
                    if (downloadInformation.XMLTagNames[0] == "")
                    {
                        return false;
                    }
                    if (downloadInformation.XMLTagNames[1] == "")
                    {
                        return false;
                    }
                }
                //   6. Description
                if (downloadInformation.description == "")
                {
                    return false;
                }
                //   7. DownloadFolder
                if (downloadInformation.downloadFolder == "")
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool IsNewVersionHigher()
        {
            if(oldVersion.CompareTo(newVersion) < 0)
            {
                // negative means newVersion is higher
                return true;
            }
            else
            {
                // equal and higher than 0 means own version is at least same
                return false;
            }
        }

        public bool CheckForUpdate()
        {
            // ToDo: Download XML
            if (DoDownload("update.xml", downloadInformation.downloadLinkUpdateXML, "Download der Update-Informationen", "."))
            {
                try
                {
                    System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
                    xml.Load("update.xml");

                    if (downloadInformation.XMLTagNames.Length >= 1)
                    {
                        newVersion = new Version(xml.SelectSingleNode("*/" + downloadInformation.XMLTagNames[0]).InnerText);
                    }
                    if (downloadInformation.XMLTagNames.Length >= 2)
                    {
                        downloadInformation.downloadLinkUpdate = xml.SelectSingleNode("*/" + downloadInformation.XMLTagNames[1]).InnerText;
                    }

                    xml = null;

                    if (File.Exists("update.xml"))
                    {
                        System.IO.File.Delete("update.xml");
                    }
                }
                catch (System.FormatException ex)
                {
                    if (ex.InnerException != null)
                    {
                        util.log(ex.InnerException.ToString(), logPath);
                    }
                    else
                    {
                        util.log(ex.ToString(), logPath);
                    }
                    
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
        
        public bool DoDownload(string applicationName, string downloadLink, string description, string downloadFolder)
        {
            // thread safe control modifying
            this.Invoke((Func<System.Windows.Forms.Label, string, bool>)DoChangeLabel, label_Content, description);

            // thread safe control modifying
            this.Invoke((Func<System.Windows.Forms.Label, string, bool>)DoChangeLabel, label_Status, "Datei anfordern");

            // check file size
            System.Net.HttpWebResponse response = null;
            try
            {
                Uri uri = new Uri(downloadLink);
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(uri);
                response = (System.Net.HttpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (System.Net.WebException ex)
            {
                if (ex.InnerException != null)
                {
                    util.log(ex.InnerException.ToString(), logPath);
                }
                else
                {
                    util.log(ex.ToString(), logPath);
                }

                return false;
            }

            if (response == null)
            {
                util.log("Invalid response", logPath);
                return false;
            }

            // complete size of the content that has to be downloaded
            Int64 completeSize = response.ContentLength;
            
            if (completeSize == 0)
            {
                util.log("Requested file size == 0", logPath);
                return false;
            }

            // downloaded size of the content that has to be downloaded to update progressbar
            Int64 downloadedSize = 0;

            // Destination
            string downloadName = downloadFolder + "\\" + applicationName;
            try
            {
                // delete last update file
                if(File.Exists(downloadName + ".bak"))
                {
                    System.IO.File.Delete(downloadName + ".bak");
                }

                // move now running exe to backup file (to be deleted at the next update)
                if (File.Exists(downloadName))
                {
                    System.IO.File.Move(downloadName, downloadName + ".bak");
                }
            }
            catch (System.IO.IOException ex)
            {
                if (ex.InnerException != null)
                {
                    util.log(ex.InnerException.ToString(), logPath);
                }
                else
                {
                    util.log(ex.ToString(), logPath);
                }

                return false;
            }

            try
            {
                // thread safe control modifying
                this.Invoke((Func<System.Windows.Forms.Label, string, bool>)DoChangeLabel, label_Status, "Verbindung aufbauen");

                // use webclient object to download the file
                using (System.Net.WebClient webClient = new System.Net.WebClient())
                {
                    // thread safe control modifying
                    this.Invoke((Func<System.Windows.Forms.Label, string, bool>)DoChangeLabel, label_Status, "Verbindung öffnen");

                    // open file at remote url for reading
                    using (System.IO.Stream remoteStream = webClient.OpenRead(new Uri(downloadLink)))
                    {
                        // thread safe control modifying
                        this.Invoke((Func<System.Windows.Forms.Label, string, bool>)DoChangeLabel, label_Status, "Lokale Vorbereitung");

                        // use FileStream to write downloaded files to system
                        using (System.IO.Stream localStream = new FileStream(downloadName, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            // thread safe control modifying
                            this.Invoke((Func<System.Windows.Forms.Label, string, bool>)DoChangeLabel, label_Status, "Download");

                            // loop stream and get file into a file buffer
                            int byteSize = 0;
                            byte[] byteBuffer = new byte[completeSize];

                            // create last update time for download speed calculation
                            DateTime lastUpdateTime = DateTime.Now;

                            // read from remote stream and download into byteBuffer
                            while ((byteSize = remoteStream.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                            {
                                // write to local Stream and file
                                localStream.Write(byteBuffer, 0, byteSize);

                                // update local byte size
                                Int64 lastDownloadedSize = downloadedSize;
                                downloadedSize += byteSize;

                                // calculate progress for progressbar (base 100)
                                double step = (double)downloadedSize;
                                double total = (double)byteBuffer.Length;
                                double fraction = (double)(step / total);
                                int percentage = (int)(fraction * 100);

                                // calculate download speed
                                DateTime now = DateTime.Now;
                                TimeSpan interval = now - lastUpdateTime;
                                double timeDiff = interval.TotalSeconds;
                                double sizeDiff = downloadedSize - lastDownloadedSize; // byte
                                double speed = (double)Math.Floor((double)(sizeDiff * 8) / timeDiff); // bit
                                // bit / second
                                if (speed < 0)
                                {
                                    speed = 0;
                                }
                                string speedString = " Bit/s";
                                // kilobit / second
                                if (speed > 999)
                                {
                                    speed /= 1000; // kilobit
                                    speedString = " kBit/s";

                                    // megabit / second
                                    if (speed > 999)
                                    {
                                        speed /= 1000; // megabit
                                        speedString = " MBit/s";

                                        // gigabit / second
                                        if (speed > 999) // gigabit
                                        {
                                            speed /= 1000;
                                            speedString = " GBit/s";
                                        }
                                    }
                                }
                                this.Invoke((Func<System.Windows.Forms.Label, string, bool>)DoChangeLabel, label_Speed, speed.ToString() + speedString);
                                lastUpdateTime = now;

                                // update progress bar
                                // thread safe control modifying
                                this.Invoke((Func<int, bool>)DoChangeProgress, percentage);
                            }

                            // clean up local stream
                            localStream.Close();
                        }

                        // clean up and close remote stream
                        remoteStream.Close();
                    }
                }

                // thread safe control modifying
                this.Invoke((Func<System.Windows.Forms.Label, string, bool>)DoChangeLabel, label_Status, "Download beendet");
                this.Invoke((Func<System.Windows.Forms.Label, string, bool>)DoChangeLabel, label_Speed, "");
            }
            catch (System.IO.IOException ex)
            {
                if (ex.InnerException != null)
                {
                    util.log(ex.InnerException.ToString(), logPath);
                }
                else
                {
                    util.log(ex.ToString(), logPath);
                }

                return false;
            }

            return true;
        }
        
        public bool DoChangeLabel(System.Windows.Forms.Label label, string content)
        {
            try
            {
                label.Text = content;
            }
            catch (System.InvalidOperationException ex)
            {
                if (ex.InnerException != null)
                {
                    util.log(ex.InnerException.ToString(), logPath);
                }
                else
                {
                    util.log(ex.ToString(), logPath);
                }

                return false;
            }
            catch (System.ComponentModel.InvalidAsynchronousStateException ex)
            {
                if (ex.InnerException != null)
                {
                    util.log(ex.InnerException.ToString(), logPath);
                }
                else
                {
                    util.log(ex.ToString(), logPath);
                }

                return false;
            }
            catch (System.NullReferenceException ex)
            {
                if (ex.InnerException != null)
                {
                    util.log(ex.InnerException.ToString(), logPath);
                }
                else
                {
                    util.log(ex.ToString(), logPath);
                }

                return false;
            }

            return true;
        }

        public bool DoChangeSpeed(string speed)
        {
            try
            {
                label_Speed.Text = speed;
            }
            catch (System.InvalidOperationException ex)
            {
                if (ex.InnerException != null)
                {
                    util.log(ex.InnerException.ToString(), logPath);
                }
                else
                {
                    util.log(ex.ToString(), logPath);
                }

                return false;
            }
            catch (System.ComponentModel.InvalidAsynchronousStateException ex)
            {
                if (ex.InnerException != null)
                {
                    util.log(ex.InnerException.ToString(), logPath);
                }
                else
                {
                    util.log(ex.ToString(), logPath);
                }

                return false;
            }
            catch (System.NullReferenceException ex)
            {
                if (ex.InnerException != null)
                {
                    util.log(ex.InnerException.ToString(), logPath);
                }
                else
                {
                    util.log(ex.ToString(), logPath);
                }

                return false;
            }

            return true;
        }

        public bool DoChangeProgress(int percentage)
        {
            try
            {
                progressBar_DownloadProgess.Value = percentage;
            }
            catch (System.InvalidOperationException ex)
            {
                if (ex.InnerException != null)
                {
                    util.log(ex.InnerException.ToString(), logPath);
                }
                else
                {
                    util.log(ex.ToString(), logPath);
                }

                return false;
            }
            catch (System.ComponentModel.InvalidAsynchronousStateException ex)
            {
                if (ex.InnerException != null)
                {
                    util.log(ex.InnerException.ToString(), logPath);
                }
                else
                {
                    util.log(ex.ToString(), logPath);
                }

                return false;
            }
            catch (System.NullReferenceException ex)
            {
                if (ex.InnerException != null)
                {
                    util.log(ex.InnerException.ToString(), logPath);
                }
                else
                {
                    util.log(ex.ToString(), logPath);
                }

                return false;
            }

            return true;
        }
    }

    partial class Utils
    {
        public Utils()
        {
        }

        public void log(string message, string path)
        {
            try
            { 
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss: ") + message);
                }
            }
            catch (System.NullReferenceException)
            {
                // yeah..can't log the log error..
                return;
            }
        }
    }
}
