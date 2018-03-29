using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    public static class DownloadInformation
    {
        // Parameters
        //   -a | --appname     ApplicationName with extension
        //   -v | --version     Version // Style "X.X.X.X" == "Major.Minor.Build.Revision"
        //   -d | --directlink  Direct download link
        //   -dx | --xmllink    Xml download link
        //   -tx | --xmltags    Xml tag names
        //   -xf | --xmlfirst   First TagName = Version (String) requires -tx
        //   -xs | --xmlsecond  Second TagName = DownloadLink (String) requires -tx
        //   -c | --comment     Description
        //   -f | --folder      DownloadFolder
        public static string applicationName { get; set; } = string.Empty;
        public static string oldVersion { get; set; } = string.Empty;
        public static string directLink { get; set; } = string.Empty;
        public static string xmlLink { get; set; } = string.Empty;
        public static string[] xmlTagNames { get; set; } = new string[2];
        public static string comment { get; set; } = string.Empty;
        public static string downloadFolder { get; set; } = string.Empty;
        public static string newVersion { get; set; } = string.Empty; // not read here, but read from Xml

        public static bool FillDownloadInformation(string[] args)
        {
            try
            {
                if (!CheckApplicationName(args)) { return false; }
                if (!CheckVersion(args)) { return false; }
                CheckDirectLink(args);
                if (!CheckXmlLink(args)) { return false; }
                if (!CheckXmlParameters(args)) { return false; } // fills xmltags
                if (!CheckComment(args)) { return false; }
                if (!CheckDownloadFolder(args)) { return false; }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);

                return false;
            }

            return true;
        }

        private static bool CheckApplicationName(string[] args)
        {
            string val = string.Empty;

            Utility.CheckArrayForStringAndApplyNext(args, ref val, "-a");
            if (val == string.Empty) // no short version, check for long one
            {
                Utility.CheckArrayForStringAndApplyNext(args, ref val, "--appname");
            }

            if (val != string.Empty)
            {
                DownloadInformation.applicationName = val;

                return true;
            }

            Logger.Log("Application name was empty");

            return false;
        }

        private static bool CheckVersion(string[] args)
        {
            string val = string.Empty;

            Utility.CheckArrayForStringAndApplyNext(args, ref val, "-v");
            if (val == string.Empty) // no short version, check for long one
            {
                Utility.CheckArrayForStringAndApplyNext(args, ref val, "--oldVersion");
            }

            if (val != string.Empty)
            {
                DownloadInformation.oldVersion = val;

                return true;
            }

            Logger.Log("Application version was empty");

            return false;
        }

        // can be empty
        private static void CheckDirectLink(string[] args)
        {
            string val = string.Empty;

            Utility.CheckArrayForStringAndApplyNext(args, ref val, "-d");
            if (val == string.Empty) // no short version, check for long one
            {
                Utility.CheckArrayForStringAndApplyNext(args, ref val, "--directlink");
            }

            if (val != string.Empty)
            {
                DownloadInformation.directLink = val;
            }
        }

        private static bool CheckXmlLink(string[] args)
        {
            string val = string.Empty;

            Utility.CheckArrayForStringAndApplyNext(args, ref val, "-dx");
            if (val == string.Empty) // no short version, check for long one
            {
                Utility.CheckArrayForStringAndApplyNext(args, ref val, "--xmllink");
            }

            if (val != string.Empty)
            {
                DownloadInformation.xmlLink = val;

                return true;
            }

            Logger.Log("Application xml download link was empty");

            return false;
        }

        private static bool CheckXmlParameters(string[] args)
        {
            string val = string.Empty;

            Utility.CheckArrayForStringAndApplyNext(args, ref val, "-tx");
            if (val == string.Empty) // no short version, check for long one
            {
                Utility.CheckArrayForStringAndApplyNext(args, ref val, "--xmltags");
            }

            if (val != string.Empty)
            {
                string[] pattern = new string[2];

                pattern[0] = "-xf";
                pattern[1] = "-xmlfirst";
                if (!CheckXmlTagNamesParameter(args, 0, pattern))
                {
                    Logger.Log("Application xml tag 1 was empty");

                    return false;
                }

                pattern[0] = "-xs";
                pattern[1] = "-xmlsecond";
                if (!CheckXmlTagNamesParameter(args, 1, pattern))
                {
                    Logger.Log("Application xml tag 2 was empty");

                    return false;
                }

                return true;
            }
            
            return true;
        }

        private static bool CheckXmlTagNamesParameter(string[] args, int index, string[] pattern)
        {
            string val = string.Empty;
            
            if (pattern.Length == 2)
            {
                if ((pattern[0] != string.Empty) && (pattern[1] != string.Empty))
                {
                    Utility.CheckArrayForStringAndApplyNext(args, ref val, pattern[0]);
                    if (val == string.Empty) // no short version, check for long one
                    {
                        Utility.CheckArrayForStringAndApplyNext(args, ref val, pattern[1]);
                    }

                    if (val != string.Empty)
                    {
                        DownloadInformation.xmlTagNames[index] = val;

                        return true;
                    }
                }
            }

            Logger.Log("Application xml tag name parameter was empty");
            
            return false;
        }

        private static bool CheckComment(string[] args)
        {
            string val = string.Empty;

            Utility.CheckArrayForStringAndApplyNext(args, ref val, "-c");
            if (val == string.Empty) // no short version, check for long one
            {
                Utility.CheckArrayForStringAndApplyNext(args, ref val, "--comment");
            }

            if (val != string.Empty)
            {
                DownloadInformation.comment = val;

                return true;
            }

            Logger.Log("Application comment was empty");

            return false;
        }

        private static bool CheckDownloadFolder(string[] args)
        {
            string val = string.Empty;

            Utility.CheckArrayForStringAndApplyNext(args, ref val, "-f");
            if (val == string.Empty) // no short version, check for long one
            {
                Utility.CheckArrayForStringAndApplyNext(args, ref val, "--folder");
            }

            if (val != string.Empty)
            {
                DownloadInformation.downloadFolder = val;

                return true;
            }

            Logger.Log("Application download folder was empty");

            return false;
        }
    }
}
