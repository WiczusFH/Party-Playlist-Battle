using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Party_Playlist_Battle
{
    public class Active_Connection {
        public bool shutdown { get; set; } = false;
        public int id { get; }
        public TcpClient client;
        public Stream clistream;
        

        public Active_Connection(TcpClient client, int id) {
            this.id = id;
            this.client = client;
            Task conn = connection();
        }

        public async Task connection() {
            await Task.Run(() => {
                byte[] buffer = new byte[4096];
                clistream = client.GetStream();
                int readsize;
                Request req;
                Console.WriteLine($"Connection alive with id: {this.id}. ");

                while (!shutdown) {
                    readsize = clistream.Read(buffer, 0, buffer.Length);
                    Console.WriteLine(ASCIIEncoding.ASCII.GetString(buffer, 0, readsize));
                    req = new Request("");
                    if (!req.keepalive) {
                        shutdown = true;
                    }
                    clistream.Write(ASCIIEncoding.ASCII.GetBytes(Response.formulateResponse(req,Status_Code.OK)), 0, ASCIIEncoding.ASCII.GetBytes(Response.formulateResponse(req,Status_Code.OK)).Length);
                    
                }
                clistream.Close();
                client.Close();
            });
        }


        public Socket user_socket {
            get => default;
            set {
            }
        }

        public int token {
            get => default;
            set {
            }
        }

        public Active_User Active_User {
            get => default;
            set {
            }
        }

        public Battle Battle {
            get => default;
            set {
            }
        }

        /// <summary>
        /// create instance of login manager, until a login is completed, create active user when logged in.
        /// </summary>
        public void run() {
            throw new System.NotImplementedException();
        }

        public void ip_block() {
            throw new System.NotImplementedException();
        }

        public void receive_message() {
            throw new System.NotImplementedException();
        }

        public void join_battle() {
            throw new System.NotImplementedException();
        }

        public void display_scoreboard() {
            throw new System.NotImplementedException();
        }
    }
}