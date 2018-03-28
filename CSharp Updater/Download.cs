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

namespace Updater
{
    public partial class Download : Form
    {
        public string[] args = new string[] { }; // to have class wide access to the parameters
        
        public Download(string[] args)
        {
            InitializeComponent();
            this.args = args;
            // wait for rendering to complete and start update check in event function
            this.Shown += new System.EventHandler(this.Download_Shown);
        }

        private void Download_Shown(object sender, EventArgs e)
        {
            DoUpdate(args);
        }
        
        public async void DoUpdate(object data)
        {
            // reset labels
            try { this.Invoke((Func<System.Windows.Forms.Label, string, bool>)Utility.DoChangeLabel, label_Version, Application.ProductVersion); } catch (Exception ex) { Logger.Log(ex); }
            try { this.Invoke((Func<System.Windows.Forms.Label, string, bool>)Utility.DoChangeLabel, label_Content, ""); } catch (Exception ex) { Logger.Log(ex); }
            try { this.Invoke((Func<System.Windows.Forms.Label, string, bool>)Utility.DoChangeLabel, label_Speed, ""); } catch (Exception ex) { Logger.Log(ex); }
            try { this.Invoke((Func<System.Windows.Forms.Label, string, bool>)Utility.DoChangeLabel, label_Status, ""); } catch (Exception ex) { Logger.Log(ex); }

            // test and set parameters --> parameters valid after this function! --> use downloadInformation from here on
            if (DownloadInformation.FillDownloadInformation(args))
            {
                // new Version is set here
                if (DownloadXmlAndCheckForUpdateInformation())
                {
                    if (IsNewVersionHigher())
                    {
                        Version newVersion = new Version();
                        try
                        {
                            newVersion = new Version(DownloadInformation.newVersion);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex);

                            Environment.Exit(1);
                        }

                        if (MessageBox.Show("Es ist eine neue Version für " + Path.GetFileNameWithoutExtension(DownloadInformation.applicationName) + " (" + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + "." + newVersion.Revision + ") verfügbar - Möchten Sie diese Version herunterladen und installieren?", "Download verfügbar", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            // start the task and instantly wait for it..
                            if (await Task.Run(() => DoDownload(DownloadInformation.applicationName, DownloadInformation.directLink, DownloadInformation.comment, DownloadInformation.downloadFolder)))
                            {
                                MessageBox.Show("Die neue Version des Programms " + Path.GetFileNameWithoutExtension(DownloadInformation.applicationName) + " (" + newVersion.Major + "." + newVersion.Minor + "." + newVersion.Build + "." + newVersion.Revision + ") wurde heruntergeladen. Bitte starten Sie " + Path.GetFileNameWithoutExtension(DownloadInformation.applicationName) + " neu.", "Download verarbeitet", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                Environment.Exit(0);
                            }
                            else
                            {
                                // update had an error
                                if (!Configuration.isSilent)
                                {
                                    MessageBox.Show("Das Update wird aufgrund eines Fehlers abgebrochen!", "Update abgebrochen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }

                                Environment.Exit(1);
                            }
                        }
                        else
                        {
                            // update was denied
                            if (!Configuration.isSilent)
                            {
                                MessageBox.Show("Das Update wurde abgelehnt!", "Update abgebrochen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            Environment.Exit(1);
                        }
                    }
                    else
                    {
                        // new version is not higher than current
                        if (!Configuration.isSilent)
                        {
                            MessageBox.Show("Es ist kein Update verfügbar!", "Update abgebrochen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        Environment.Exit(1);
                    }
                }
                else
                {
                    // xml error
                    if (!Configuration.isSilent)
                    {
                        MessageBox.Show("Es ist konnten keine Updateinformationen heruntergeladen werden!", "Update abgebrochen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    Environment.Exit(1);
                }
            }
            else
            {
                // not all parameters valid
                if (!Configuration.isSilent)
                {
                    MessageBox.Show("Das Update wird aufgrund fehlerhafter Informationen abgebrochen!", "Update abgebrochen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                Environment.Exit(1);
            }
        }

        public bool IsNewVersionHigher()
        {
            try
            {
                Version oldVersion = new Version(DownloadInformation.oldVersion);
                Version newVersion = new Version(DownloadInformation.newVersion);

                if (oldVersion.CompareTo(newVersion) < 0)
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
            catch (Exception ex)
            {
                Logger.Log(ex);

                return false;
            }
        }

        public bool DownloadXmlAndCheckForUpdateInformation()
        {
            // download xml
            if (DoDownload("update.xml", DownloadInformation.xmlLink, "Download der Update-Informationen", "."))
            {
                try
                {
                    // read xml
                    System.Xml.XmlDocument xml = new System.Xml.XmlDocument();
                    xml.Load("update.xml");

                    // read tagname 1
                    if (DownloadInformation.xmlTagNames.Length >= 1)
                    {
                        if ((xml.SelectSingleNode("*/" + DownloadInformation.xmlTagNames[0]).InnerText != null) && (xml.SelectSingleNode("*/" + DownloadInformation.xmlTagNames[0]).InnerText != ""))
                        DownloadInformation.newVersion = xml.SelectSingleNode("*/" + DownloadInformation.xmlTagNames[0]).InnerText;
                    }

                    // read tagname 2
                    if (DownloadInformation.xmlTagNames.Length >= 2)
                    {
                        if ((xml.SelectSingleNode("*/" + DownloadInformation.xmlTagNames[1]).InnerText != null) && (xml.SelectSingleNode("*/" + DownloadInformation.xmlTagNames[1]).InnerText != ""))
                        {
                            DownloadInformation.directLink = xml.SelectSingleNode("*/" + DownloadInformation.xmlTagNames[1]).InnerText;
                        }
                    }

                    // close xml
                    xml = null;

                    // delete after reading
                    if (File.Exists("update.xml"))
                    {
                        System.IO.File.Delete("update.xml");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);

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
            try { this.Invoke((Func<System.Windows.Forms.Label, string, bool>)Utility.DoChangeLabel, label_Content, description); } catch (Exception ex) { Logger.Log(ex); }

            // thread safe control modifying
            try { this.Invoke((Func<System.Windows.Forms.Label, string, bool>)Utility.DoChangeLabel, label_Status, "Datei anfordern"); } catch (Exception ex) { Logger.Log(ex); }

            // check file size
            System.Net.HttpWebResponse response = null;
            try
            {
                // get http request and store size
                Uri uri = new Uri(downloadLink);
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(uri);
                response = (System.Net.HttpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);

                return false;
            }

            if (response == null)
            {
                Logger.Log("Invalid response");

                return false;
            }

            // complete size of the content that has to be downloaded
            Int64 completeSize = response.ContentLength;
            
            if (completeSize == 0)
            {
                Logger.Log("Requested file size is zero");

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
            catch (Exception ex)
            {
                Logger.Log(ex);

                return false;
            }

            try
            {
                // thread safe control modifying
                try { this.Invoke((Func<System.Windows.Forms.Label, string, bool>)Utility.DoChangeLabel, label_Status, "Verbindung aufbauen"); } catch (Exception ex) { Logger.Log(ex); }

                // use webclient object to download the file
                using (System.Net.WebClient webClient = new System.Net.WebClient())
                {
                    // thread safe control modifying
                    try { this.Invoke((Func<System.Windows.Forms.Label, string, bool>)Utility.DoChangeLabel, label_Status, "Verbindung öffnen"); } catch (Exception ex) { Logger.Log(ex); }

                    // open file at remote url for reading
                    using (System.IO.Stream remoteStream = webClient.OpenRead(new Uri(downloadLink)))
                    {
                        // thread safe control modifying
                        try { this.Invoke((Func<System.Windows.Forms.Label, string, bool>)Utility.DoChangeLabel, label_Status, "Lokale Vorbereitung"); } catch (Exception ex) { Logger.Log(ex); }

                        // use FileStream to write downloaded files to system
                        using (System.IO.Stream localStream = new FileStream(downloadName, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            // thread safe control modifying
                            try { this.Invoke((Func<System.Windows.Forms.Label, string, bool>)Utility.DoChangeLabel, label_Status, "Download"); } catch (Exception ex) { Logger.Log(ex); }

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

                                // thread safe control modifying
                                try { this.Invoke((Func<System.Windows.Forms.Label, string, bool>)Utility.DoChangeLabel, label_Speed, speed.ToString() + speedString); } catch (Exception ex) { Logger.Log(ex); }
                                lastUpdateTime = now;

                                // update progress bar
                                // thread safe control modifying
                                try { this.Invoke((Func<System.Windows.Forms.ProgressBar, int, bool>)Utility.DoChangeProgress, progressBar_DownloadProgess, percentage); } catch (Exception ex) { Logger.Log(ex); }
                            }

                            // clean up local stream
                            localStream.Close();
                        }

                        // clean up and close remote stream
                        remoteStream.Close();
                    }
                }

                // thread safe control modifying
                try { this.Invoke((Func<System.Windows.Forms.Label, string, bool>)Utility.DoChangeLabel, label_Status, "Download beendet"); } catch (Exception ex) { Logger.Log(ex); }
                try { this.Invoke((Func<System.Windows.Forms.Label, string, bool>)Utility.DoChangeLabel, label_Speed, ""); } catch (Exception ex) { Logger.Log(ex); }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);

                return false;
            }

            return true;
        }
    }
}
