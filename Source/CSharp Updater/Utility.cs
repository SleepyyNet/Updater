using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_Updater
{
    class Utility
    {
        public static bool DoChangeLabel(System.Windows.Forms.Label label, string content, string logPath)
        {
            try
            {
                label.Text = content;
            }
            catch (Exception ex)
            {
                Logger.Log(logPath, ex);

                return false;
            }

            return true;
        }

        public static bool DoChangeProgress(System.Windows.Forms.ProgressBar progress, int percentage, string logPath)
        {
            try
            {
                progress.Value = percentage;
            }
            catch (Exception ex)
            {
                Logger.Log(logPath, ex);

                return false;
            }

            return true;
        }
    }
}
