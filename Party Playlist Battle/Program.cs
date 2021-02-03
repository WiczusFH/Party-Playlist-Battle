using System.Collections.Generic;
using System.Linq;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace Party_Playlist_Battle
{
    class Program
    {

        public static void Main(string[] args) {
            //Server serv = new Server();
            //serv.run();
            Connection_Listener listener = new Connection_Listener();
            Console.WriteLine(listener.generate_token());
        }
    }
}
