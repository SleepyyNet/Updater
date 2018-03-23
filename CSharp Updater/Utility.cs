using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Updater
{
    class Utility
    {
        public static bool DoChangeLabel(System.Windows.Forms.Label label, string content, string logPath)
        {
            try
            {
                // set label text
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
                // set progressbar value
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
