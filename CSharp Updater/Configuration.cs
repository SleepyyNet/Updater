using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Updater
{
    public static class Configuration
    {
        // Parameters
        //   -l | --logpath     Change the default logpath and creates the directory
        //   -s | --silent      Activate the silent-mode
        public static string logPath { set; get; } = ".\\Updater.log";
        public static bool isSilent { get; set; } = false;

        public static void FillConfiguration(string[] args)
        {
            try
            {
                CheckSilentMode(args);
                CheckLogPath(args);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public static void CheckSilentMode(string[] args)
        {
            if ((Utility.CheckArrayForString(args, "-s")) || (Utility.CheckArrayForString(args, "--silent")))
            {
                Configuration.isSilent = true;
            }
        }

        public static void CheckLogPath(string[] args)
        {
            string val = string.Empty;

            Utility.CheckArrayForStringAndApplyNext(args, ref val, "-l");
            if (val == string.Empty) // no short version, check for long one
            {
                Utility.CheckArrayForStringAndApplyNext(args, ref val, "--log");
            }

            if (val != string.Empty)
            {
                Configuration.logPath = val;

                Directory.CreateDirectory(Path.GetDirectoryName(Configuration.logPath));
            }
        }
    }
}
