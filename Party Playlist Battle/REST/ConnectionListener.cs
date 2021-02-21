using System.Collections.Generic;
using System.Linq;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Party_Playlist_Battle
{
    public class Connection_Listener
    {
        Battle battle = new Battle();
        List<Active_Connection> connections = new List<Active_Connection>();
        TcpListener listener = new TcpListener(IPAddress.Loopback, 10001);

        public async void startAsync() {
            await Task.Run(() => {
                listener.Start();
                for (int i = 0; i < 25; i++) {
                    Console.WriteLine("Looking for cliens. ");
                    connections.Add(new Active_Connection(listener.AcceptTcpClient(), i,battle));
                }
            });
        }

        public void start()
        {
            listener.Start();
            for (int i = 0; i < 2500; i++)
            {
                Console.WriteLine("Looking for cliens. ");
                connections.Add(new Active_Connection(listener.AcceptTcpClient(), i, battle)); ;
            }
        }

        public void list_active_connections() {
            foreach (Active_Connection conn in connections) {
                Console.WriteLine(conn.id);
            }
        }
        public void kill_active_connection(int id) {
            Console.WriteLine("Close Attempt");
            int id_to_remove=-1;
            foreach (Active_Connection conn in connections) {
                if (conn.id == id) {
                    Console.WriteLine($"Closing connection{id}");
                    //close connection, remove from list;
                    conn.clistream.Close();
                    conn.client.Close();
                    id_to_remove = id;
                }
            }
            if (id_to_remove != -1) {
                connections.RemoveAt(id_to_remove);
            }
        }
        public List<Active_Connection> active_connection_list {
            get => default;
            set {
            }
        }

        public Active_Connection Active_Connection {
            get => default;
            set {
            }
        }

    }
}