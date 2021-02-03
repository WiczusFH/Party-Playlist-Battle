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
        List<Active_Connection> connections = new List<Active_Connection>();
        TcpListener listener = new TcpListener(IPAddress.Loopback, 6000);

        public Connection_Listener() {
        }
        public async void start() {
            await Task.Run(() => {
                listener.Start();
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

        public string generate_token() {
            Random rand=new Random();
            int number = rand.Next(0,65535);
            string result = ComputeSha256Hash(number.ToString());
            return result;
        }

        //Source: https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
        static string ComputeSha256Hash(string rawData) {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create()) {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++) {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}