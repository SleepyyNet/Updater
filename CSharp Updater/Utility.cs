using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    class Utility
    {
        public static bool DoChangeLabel(System.Windows.Forms.Label label, string content)
        {
            try
            {
                // set label text
                label.Text = content;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);

                return false;
            }

            return true;
        }

        public static bool DoChangeProgress(System.Windows.Forms.ProgressBar progress, int percentage)
        {
            try
            {
                // set progressbar value
                progress.Value = percentage;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);

                return false;
            }

            return true;
        }

        public static bool CheckArrayForStringAndApplyNext(string[] args, ref string val, string pattern)
        {
            if (CheckArrayForString(args, pattern))
            {
                int index = Array.IndexOf(args, pattern);

                if (index >= 0)
                {
                    val = args[index + 1];

                    return true;
                }
            }

            return false;
        }

        public static bool CheckArrayForString(string[] args, string pattern)
        {
            return Array.Exists<string>(args, element => element == pattern);
        }
    }
}
