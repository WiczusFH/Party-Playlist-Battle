using System.Collections.Generic;
using System.Linq;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Party_Playlist_Battle
{
    public class Connection_Listener
    {
        List<Active_Connection> connections = new List<Active_Connection>();
        TcpListener listener = new TcpListener(IPAddress.Loopback, 6000);

        public Connection_Listener() {
            listener.Start();
            start();
        }
        public async void start() {
            await Task.Run(() => {
                for (int i = 0; i < 5; i++) {
                    Console.WriteLine("Looking for cliens. ");
                    connections.Add(new Active_Connection(listener.AcceptTcpClient(), i));
                }
            });
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

        /// <summary>
        /// creates instance of active_connection class
        /// </summary>
        public int create_active_connection() {
            throw new System.NotImplementedException();
        }

        public void start_listening() {
            throw new System.NotImplementedException();
        }

        public void terminate_active_connection() {
            throw new System.NotImplementedException();
        }

        public void generate_token() {
            throw new System.NotImplementedException();
        }
    }
}