using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
    class Logger
    {
        public static void Log(string message, [CallerMemberName]string callerMember = "")
        {
            try
            {
                using (StreamWriter file = new StreamWriter(Configuration.logPath, true))
                {
                    /* get caller class and method name */
                    StackFrame frame = new StackFrame(1);

                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + frame.GetMethod().DeclaringType.ToString() + frame.GetMethod().Name + ": " + message);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                /* can't log the log error.. */
            }
        }

        public static void Log(Exception ex, [CallerMemberName]string callerMember = "")
        {
            try
            {
                using (StreamWriter file = new StreamWriter(Configuration.logPath, true))
                {
                    /* get caller class and method name */
                    StackFrame frame = new StackFrame(1);

                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss: ") + frame.GetMethod().DeclaringType.ToString() + frame.GetMethod().Name + ": " + ex.Message);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                /* can't log the log error.. */
            }
        }
    }
}
