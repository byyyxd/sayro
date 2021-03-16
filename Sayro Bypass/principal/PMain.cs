using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace Sayro_Bypass
{
    class Program
    {
        static void useless()
        {
            if (utils.GetConsoleWindow() != IntPtr.Zero)
            {
                utils.DeleteMenu(utils.GetSystemMenu(utils.GetConsoleWindow(), false), 0xF000, 0x00000000);
                utils.DeleteMenu(utils.GetSystemMenu(utils.GetConsoleWindow(), false), 0xF030, 0x00000000);
            }
        }

        static void setup(string title)
        {
            Console.Title = title + " | daarkin.wtf";
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;
            string center_title = "                                            ";
            ap.send(center_title + "sayro.wtf - public bypass version");

            ap.send(center_title + "       windows ver: " + Environment.OSVersion.Platform);

            ap.space(3, false);

            ap.send(center_title + "checking useful information....");
            ap.send(center_title + "    - HWID: " + utils.check_hwid().ToString().Replace("-", ""));
            ap.send(center_title + "    - Date: " + DateTime.Now);
            ap.send(center_title + "    - Status: " + utils.get_status());

            ap.space(3, false);

            Thread.Sleep(1500);
            ap.send(center_title + "checking hash....");

            Thread.Sleep(500);
            ap.send(center_title + "    - hash: " + utils.get_hash());

            Thread.Sleep(2000);

            //removed hash check :)
            if (!utils.check_blacklist())
            {
                ap.clear();
                ap.space(2, false);

                ap.sendline(" - welcome back ");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                ap.sendline(Environment.UserName);
                Console.ResetColor();

                ap.space(5, false);
                ap.send("follow regedit paths were found:");
                Console.ForegroundColor = ConsoleColor.Cyan;
                ap.send("\n" + utils.regedit_paths());
                Console.ResetColor();

                ap.space(2, false);
                ap.send("want to start deleting them? [ Press any key ]");
                Console.ReadLine();
                utils.delete_regedits();
                Thread.Sleep(1500);
                ap.clear();
                ap.send("everything done, thank you. [ Press any key to exit ]");
                Console.ReadLine();
                utils.self_delete(1);
                Environment.Exit(0);
            }

            ap.stop();
        }

        static void Main(string[] args)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            useless(); // disable resize and maximize
            setup("s a y r o");
        }
    }
}
