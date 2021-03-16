using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sayro_Bypass
{
    class ap
    {
        public static void send(string message)
        {
            Console.WriteLine(message);
        }

        public static void sendline(string msg)
        {
            Console.Write(msg);
        }

        public static void clear()
        {
            Console.Clear();
        }

        public static void space(int k, bool debug)
        {
            int p;
            for (p = 0; p < k; p++)
            {
                Console.Write("\n");
            }
            if (debug)
                Console.Write("\n spaces did: " + p);
        }

        public static void stop()
        {
            System.Threading.Thread.Sleep(-1);
        }

    }
}
