using System;
using System.Net;
using System.Net.Sockets;
using ServerEngine.Manager;
using System.Threading;

namespace ServerEngine.Sockets
{
    class ServerSocket
    {
        public int port;
        public int max_users_on;
        TcpListener Listener;
        public Thread ServerThread;
        public int users_on;
        public ServerSocket(int Port, int Max_users_on)
        {
            port = Port;
            max_users_on = Max_users_on;
            Server.Users = new Usermanager[max_users_on];
            users_on = 0;
            Listener = new TcpListener(IPAddress.Any, port);
            Listener.Start();
            iRun();
        }
        public void iRun()
        {
            ServerThread = new Thread(run);
            ServerThread.Start();
        }
        public void run()
        {
            try
            {
                while (true)
                {
                    int uo = 0;
                    for (int i = 0; i < Server.Users.Length; i++)
                    {
                        if (Server.Users[i] != null)
                        {
                            if (Server.Users[i].used)
                            {
                                uo++;
                            }
                        }
                    }
                    users_on = uo;
                    TcpClient tmp = Listener.AcceptTcpClient();
                    if (tmp != null && max_users_on > users_on)
                    {
                        int res = -1;
                        for (int i = 0; i < Server.Users.Length; i++)
                        {
                            if (Server.Users[i] != null)
                            {
                                if (Server.Users[i].used == false)
                                {
                                    res = i;
                                    break;
                                }
                            }
                            else
                            {
                                res = i;
                                break;
                            }
                        }
                        if (res != -1)
                        {
                            //Free Socket Found!!!
                            Server.Users[res] = new Usermanager();
                            Server.Users[res].Sck = tmp;
                            Server.Users[res].used = true;
                            Server.CTCP[res] = new TCPSocket(res);
                            Thread CWThread = new Thread(Server.CTCP[res].run);
                            CWThread.Start();
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error Starting Server Thread :(");
            }
        }
    }
}
