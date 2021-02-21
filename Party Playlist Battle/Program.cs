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
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using Npgsql;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Party_Playlist_Battle;
using System.Security.Cryptography;

namespace Party_Playlist_Battle
{
    class Program
    {
        public static void Main(string[] args) {
            Server serv = new Server();
            serv.run();
        }
    }
}
