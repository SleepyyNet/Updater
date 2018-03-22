using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Updater
{
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

        public static bool FillDownloadInformation(ref DownloadInformation download, string logPath, ref Version oldVersion, string[] args)
        {
            if (args.Length >= 7)
            {
                // set parameters
                try
                {
                    // Set Parameters
                    //   1. ApplicationName with extension
                    download.applicationName = args[0];
                    //   2. Version
                    download.version = args[1];
                    //   3. DownloadLinkUpdate
                    download.downloadLinkUpdate = args[2];
                    //   4. DownloadLinkUpdateXML
                    download.downloadLinkUpdateXML = args[3];
                    //   5. DownloadLinkUpdateXMLSchema
                    download.XMLTagNames = args[4].Split('|');
                    //   6. Description
                    download.description = args[5];
                    //   7. DownloadFolder
                    download.downloadFolder = args[6];
                }
                catch (Exception ex)
                {
                    Logger.Log(logPath, ex);

                    return false;
                }

                //test parameters
                //   1. ApplicationName with extension
                if (download.applicationName == "")
                {
                    Logger.Log(logPath, "Application name was empty");

                    return false;
                }
                //   2. Version
                if (download.version == "")
                {
                    Logger.Log(logPath, "Application version was empty");

                    return false;
                }
                else
                {
                    try
                    {
                        oldVersion = new Version(download.version);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(logPath, ex);

                        return false;
                    }
                }
                //   3. DownloadLinkUpdate
                if (download.downloadLinkUpdate == "")
                {
                    Logger.Log(logPath, "Application download link was empty");

                    return false;
                }
                //   4. DownloadLinkUpdateXML
                if (download.downloadLinkUpdateXML == "")
                {
                    Logger.Log(logPath, "Application xml download link was empty");

                    return false;
                }
                //   5. DownloadLinkUpdateXMLSchema
                if (download.XMLTagNames.Length >= 1)
                {
                    if (download.XMLTagNames[0] == "")
                    {
                        Logger.Log(logPath, "Application xml tag name one was empty");

                        return false;
                    }
                    if (download.XMLTagNames[1] == "")
                    {
                        Logger.Log(logPath, "Application xml tag name two was empty");

                        return false;
                    }
                }
                //   6. Description
                if (download.description == "")
                {
                    Logger.Log(logPath, "Application description was empty");

                    return false;
                }
                //   7. DownloadFolder
                if (download.downloadFolder == "")
                {
                    Logger.Log(logPath, "Application download folder was empty");

                    return false;
                }
            }
            else
            {
                Logger.Log(logPath, "Application parameters were invalid ( "+ args.Length + " )");

                return false;
            }

            return true;
        }
    }
}
