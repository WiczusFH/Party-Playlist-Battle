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
        Battle battle;

        public Active_Connection(TcpClient client, int id,Battle battle) {
            this.id = id;
            this.battle = battle;
            this.client = client;
            Task conn = connectionAsync();
            //connection();
        }

        public async Task connectionAsync() {
            await Task.Run(() => {
                byte[] buffer = new byte[4096];
                clistream = client.GetStream();
                int readsize;
                Request req;
                Response resp;
                Console.WriteLine($"Connection alive with id: {this.id}. ");

                while (!shutdown) {
                    readsize = clistream.Read(buffer, 0, buffer.Length);
                    req = new Request(ASCIIEncoding.ASCII.GetString(buffer, 0, readsize));
                    if (!req.keepalive) {
                        shutdown = true;
                    }
                    Console.WriteLine(ASCIIEncoding.ASCII.GetString(buffer, 0, readsize));
                    resp = REST_Tools.Handler(req,battle);
                    Console.WriteLine(resp.formulateResponse());
                    clistream.Write(ASCIIEncoding.ASCII.GetBytes(resp.formulateResponse(), 0, ASCIIEncoding.ASCII.GetBytes(resp.formulateResponse()).Length));
                    
                }
                clistream.Close();
                client.Close();
            });
        }

        public void connection()
        {
            
            byte[] buffer = new byte[4096];
            clistream = client.GetStream();
            int readsize;
            Request req;
            Response resp;
            Console.WriteLine($"Connection alive with id: {this.id}. ");

            while (!shutdown)
            {
                readsize = clistream.Read(buffer, 0, buffer.Length);
                req = new Request(ASCIIEncoding.ASCII.GetString(buffer, 0, readsize));
                if (!req.keepalive)
                {
                    shutdown = true;
                }
                Console.WriteLine(ASCIIEncoding.ASCII.GetString(buffer, 0, readsize));
                resp = REST_Tools.Handler(req,battle);
                Console.WriteLine(resp.formulateResponse());
                clistream.Write(ASCIIEncoding.ASCII.GetBytes(resp.formulateResponse(), 0, ASCIIEncoding.ASCII.GetBytes(resp.formulateResponse()).Length));

            }
            clistream.Close();
            client.Close();
            
        }


        public Socket user_socket {
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