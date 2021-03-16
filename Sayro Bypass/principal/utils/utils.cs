using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace Sayro_Bypass
{
    class utils
    {

        public const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_SIZE = 0xF000;

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();

        public static SecurityIdentifier check_hwid()
        {
            return new SecurityIdentifier((byte[])new DirectoryEntry(string.Format("WinNT://{0},Computer", Environment.MachineName)).Children.Cast<DirectoryEntry>().First().InvokeGet("objectSID"), 0).AccountDomainSid;
        }

        public static void rename()
        {
            Random r = new Random();
            string str = utils.random_string(r.Next(9, 16)).ToLower();
            string str1 = Process.GetCurrentProcess().MainModule.ModuleName;
            Process[] ps = Process.GetProcessesByName(str1);
            if (ps.Length <= 0)
            {
                string filePath = System.Environment.CurrentDirectory;
                string newFileName = filePath + "\\" + str + ".exe";
                File.Move(str1, newFileName);
            }
        }
        public static bool check_acess()
        {
            if(get_status().Contains("Online"))
            {
                using (var wc = new WebClient())
                {
                    wc.Proxy = null;
                    byte[] bytes = wc.DownloadData("");
                    string list = Encoding.UTF8.GetString(bytes);
                    if (list.Contains(check_hwid().ToString().Replace("-", "")))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            } else
            {
                return false;
            }
        }

        private static Random random = new Random();
        public static string random_string(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool check_blacklist()
        {
            return false;
        }

        public static string get_status()
        {
            using(var wc = new WebClient())
            {
                wc.Proxy = null;
                string p = wc.DownloadString("https://google.com.br");
                if (string.IsNullOrEmpty(p))
                    return "Offline";
                else
                    return "Online";
            }
        }

        public static string get_hash()
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            System.IO.FileStream stream = new System.IO.FileStream(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

            md5.ComputeHash(stream);

            stream.Close();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < md5.Hash.Length; i++)
                sb.Append(md5.Hash[i].ToString("x2"));

            return sb.ToString().ToUpperInvariant();
        }

        public static void self_delete(int Timeout)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe")
            {
                Arguments = string.Concat(new string[] {
                    "/c ping 1.1.1.1 -n 1 -w ",
                    Timeout.ToString(),
                    " > Nul & Del \"",
                    Application.ExecutablePath,
                    "\""
                }),
                CreateNoWindow = true,
                ErrorDialog = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process.Start(startInfo);
            Application.ExitThread();
            Environment.Exit(0);
        }

        public static void delete_regedits()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\MICROSOFT\\WINDOWS\\CURRENTVERSION\\Explorer\\USERASSIST", true))
            {
                foreach (string class_guid in key.GetSubKeyNames())
                {
                    using (RegistryKey productKey = key.OpenSubKey(class_guid))
                        key.DeleteSubKeyTree(class_guid);
                }
            }

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SYSTEM\\ControlSet001\\Services\bam\\State\\", true))
            {
                if (key == null)
                {
                    return;
                }
                else
                {
                    foreach (string class_guid in key.GetSubKeyNames())
                    {
                        using (RegistryKey productKey = key.OpenSubKey(class_guid))
                            key.DeleteSubKeyTree(class_guid);
                    }
                }
            }

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\CLASSES\\LOCAL SETTINGS\\SOFTWARE\\MICROSOFT\\WINDOWS\\SHELL\\", true))
            {
                foreach (string class_guid in key.GetSubKeyNames())
                {
                    using (RegistryKey productKey = key.OpenSubKey(class_guid))
                        key.DeleteSubKeyTree(class_guid);
                }
            }

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\MICROSOFT\\WINDOWS\\SHELLNOROAM\\", true))
            {
                if (key == null)
                {
                    return;
                }
                else
                {
                    foreach (string class_guid in key.GetSubKeyNames())
                    {
                        using (RegistryKey productKey = key.OpenSubKey(class_guid))
                            key.DeleteSubKeyTree(class_guid);
                    }
                }
            }

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\MICROSOFT\\WINDOWS NT\\CURRENTVERSION\\APPCOMPATFLAGS\\COMPATIBILITY ASSISTANT\\", true))
            {
                if (key == null)
                {
                    return;
                }
                else
                {
                    foreach (string class_guid in key.GetSubKeyNames())
                    {
                        using (RegistryKey productKey = key.OpenSubKey(class_guid))
                            key.DeleteSubKeyTree(class_guid);
                    }
                }
            }

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\MICROSOFT\\WINDOWS\\SHELL\\", true))
            {
                if (key == null)
                {
                    return;
                }
                else
                {
                    foreach (string class_guid in key.GetSubKeyNames())
                    {
                        using (RegistryKey productKey = key.OpenSubKey(class_guid))
                            key.DeleteSubKeyTree(class_guid);
                    }
                }
            }

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\MICROSOFT\\WINDOWS\\CURRENTVERSION\\Explorer\\COMDLG32\\", true))
            {
                if (key == null)
                {
                    return;
                }
                else
                {
                    foreach (string class_guid in key.GetSubKeyNames())
                    {
                        using (RegistryKey productKey = key.OpenSubKey(class_guid))
                            key.DeleteSubKeyTree(class_guid);
                    }
                }
            }

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\CurrentVersion\\AppContainer\\Storage\\", true))
            {
                if (key == null)
                {
                    return;
                }
                else
                {
                    foreach (string class_guid in key.GetSubKeyNames())
                    {
                        using (RegistryKey productKey = key.OpenSubKey(class_guid))
                            key.DeleteSubKeyTree(class_guid);
                    }
                }
            }

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\5.0\\Cache\\History\\", true))
            {
                if (key == null)
                {
                    return;
                }
                else
                {
                    foreach (string class_guid in key.GetSubKeyNames())
                    {
                        using (RegistryKey productKey = key.OpenSubKey(class_guid))
                            key.DeleteSubKeyTree(class_guid);
                    }
                }
            }
        }

        public static string regedit_paths()
        {
            try
            {
                Registry.CurrentUser.OpenSubKey("SOFTWARE\\MICROSOFT\\WINDOWS\\CURRENTVERSION\\Explorer\\USERASSIST", true);
                Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\AppCompatFlags\\Compatibility Assistant\\", true);
                Registry.CurrentUser.OpenSubKey("SYSTEM\\ControlSet001\\Services\bam\\State\\", true);
                Registry.CurrentUser.OpenSubKey("SOFTWARE\\MICROSOFT\\WINDOWS\\SHELL\\", true);
                Registry.CurrentUser.OpenSubKey("SOFTWARE\\MICROSOFT\\WINDOWS\\CURRENTVERSION\\Explorer\\COMDLG32\\", true);
                Registry.CurrentUser.OpenSubKey("SOFTWARE\\MICROSOFT\\WINDOWS\\SHELLNOROAM\\", true);
                Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\CurrentVersion\\AppContainer\\Storage\\", true);
                Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Internet Settings\\5.0\\Cache\\History\\", true);

                return "UserAssistView\nCompatibility Assistant\nSystem State\nWindows Shell\nExplorer\nShell No Roam\nApplication Storage\nBrowser Cache";
            } catch(Exception)
            {
                return "Some of the regedit paths couldn't be find due to alredy deleted.";
            }
        }


        public static bool check_hash()
        {
            using(var wc = new WebClient())
            {
                wc.Proxy = null;
                byte[] bytes = wc.DownloadData("https://raw.githubusercontent.com/byyyxd/sayro/main/current_hash");
                string hash = Encoding.UTF8.GetString(bytes);
                if(hash.Contains(get_hash()))
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }

    }
}
