using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharp_Updater
{
    class Logger
    {
        public static void Log(string pathToLogFile, string message, [CallerMemberName]string callerMember = "")
        {
            try
            {
                using (StreamWriter file = new StreamWriter(pathToLogFile, true))
                {
                    /* get caller class and method name */
                    StackFrame frame = GetLastStackFrame();

                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss: ") + frame.GetMethod().DeclaringType.ToString() + frame.GetMethod().Name + ": " + message);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                /* can't log the log error.. */
            }
        }

        public static void Log(string pathToLogFile, Exception ex, [CallerMemberName]string callerMember = "")
        {
            try
            {
                using (StreamWriter file = new StreamWriter(pathToLogFile, true))
                {
                    /* get caller class and method name */
                    StackFrame frame = GetLastStackFrame();

                    file.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss: ") + frame.GetMethod().DeclaringType.ToString() + frame.GetMethod().Name + ": " + ex.Message);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);

                /* can't log the log error.. */
            }
        }

        public static StackFrame GetLastStackFrame()
        {
            return new StackFrame(2); // get on top of this and caller function
        }
    }
}
