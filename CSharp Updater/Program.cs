using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Updater
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // check for needed .NET Framework
            if(IsNet45OrNewer())
            {
                // Enable visuals
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                try
                {
                    // run application
                    Application.Run(new Download(args));
                }
                catch (System.Reflection.TargetInvocationException ex)
                {
                    // specific error occured
                    MessageBox.Show("Das Update wurde aufgrund eines TargetInvocationException Fehlers (" + ex.ToString() + ") abgebrochen!", "Update abgebrochen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    // error occured
                    MessageBox.Show("Das Update wurde aufgrund eines unbekannten Fehlers (" + ex.ToString() + ") abgebrochen!", "Update abgebrochen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                }
            }
            else
            {
                // .NET version too old, download new one?
                if(MessageBox.Show("Sie benötigen mindestens die .NET Version 4.5.1 - Möchten Sie diese Version herunterladen?", ".NET Version veraltet", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                {
                    // start browser for downloading
                    Process.Start("https://www.microsoft.com/de-de/download/confirmation.aspx?id=40779");
                }

                Application.Exit();
            }
        }

        public static bool IsNet45OrNewer()
        {
            // registry key for .NET version
            const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

            try
            {
                // open and read registry
                using (RegistryKey regKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
                {
                    // check registry key for "Release"
                    if (regKey != null && regKey.GetValue("Release") != null)
                    {
                        // test NET version
                        switch (CheckNetVersion((int)regKey.GetValue("Release")))
                        {
                            case "4.7.1+":
                                return true;
                            case "4.7":
                                return true;
                            case "4.6.2":
                                return true;
                            case "4.6.1":
                                return true;
                            case "4.6":
                                return true;
                            case "4.5.2":
                                return true;
                            case "4.5.1":
                                return true;
                            case "4.5":
                                return false;
                            default:
                                return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(Download.logPath, ex);

                return false;
            }

            // no registry value found or registry could not be opened
            return false;
        }

        public static string CheckNetVersion(int version)
        {
            // check for microsoft NET versioning
            if (version >= 461308)
            {
                return "4.7.1+";
            }
            if (version >= 460798)
            {
                return "4.7";
            }
            if (version >= 394802)
            {
                return "4.6.2";
            }
            if (version >= 394254)
            {
                return "4.6.1";
            }
            if (version >= 393295)
            {
                return "4.6";
            }
            if (version >= 379893)
            {
                return "4.5.2";
            }
            if (version >= 378675)
            {
                return "4.5.1";
            }
            if (version >= 378389)
            {
                return "4.5";
            }

            // default
            return "";
        }
    }
}
