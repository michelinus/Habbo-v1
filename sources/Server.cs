using System;
using ServerEngine.Manager;
using System.Threading;
using System.Diagnostics;
using Storage;

namespace ServerEngine
{
    class Server
    {
        public static Usermanager[] Users;
        public static ServerEngine.Sockets.ServerSocket SS;
        public static ServerEngine.Sockets.TCPSocket[] CTCP;
        private static DatabaseManager DatabaseManager;

        static void Main(string[] args)
        {
            var config = new IniFile("Server.ini");
            Console.Title = "Starting emulator...";
            DatabaseServer dbServer = new DatabaseServer(config.Read("hostname", "MySQL"), uint.Parse(config.Read("port","MySQL")), config.Read("username", "MySQL"), config.Read("password", "MySQL"));
            Database db = new Database(config.Read("database", "MySQL"), uint.Parse("5"), uint.Parse("30"));
            DatabaseManager = new DatabaseManager(dbServer, db);

            DatabaseClient dbClient = Server.GetDatabase().GetClient();
            CTCP = new ServerEngine.Sockets.TCPSocket[10];
            String hotel_name = dbClient.ReadString("SELECT hotel_name FROM settings LIMIT  1");
            String ip = config.Read("ip", "Server");
            int port = int.Parse(config.Read("port", "Server"));
            int max_users = dbClient.ReadInt32("SELECT max_users FROM settings LIMIT 1");
            dbClient.ExecuteQuery("UPDATE users SET inroom = '0'");
            dbClient.ExecuteQuery("UPDATE rooms SET inroom = '0'");
            SS = new ServerEngine.Sockets.ServerSocket(port, max_users);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("");
            Console.WriteLine("#  #  #   ##  ##   ##       ## ### ##  #  # ### ##     v1");
            Console.WriteLine("#  # #  # # # # # #  #     #   #   # # #  # #   # # ");
            Console.WriteLine("#### #### ##  ##  #  #      #  ##  ##  #  # ##  ##  ");
            Console.WriteLine("#  # #  # # # # # #  #       # ##  ##  #  # ##  ##  ");
            Console.WriteLine("#  # #  # ##  ##   ##      ##  ### # #  ##  ### # # ");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("                    © Michelinus ");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("'" + hotel_name + "'" + " e' stato avviato correttamente, buon divertimento!");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("IP: " + ip + " | " + "Client Port: " + port + " | " + "Max Users: " + max_users + "!");
            Console.WriteLine("");
            Thread t = new Thread(console_listener);
            t.Start();
            while (true)
            {
                Process pmemory = Process.GetCurrentProcess();
                Console.Title = "| Emulator started! - Server Name: " + hotel_name + " | " + "Client Port: 50172" + " | " + "RAM: " + pmemory.PrivateMemorySize64 / 1024 / 1024 +"mb" + " | ";
                Thread.Sleep(100);
            }
        }
        private static void console_listener()
        {
            while (true)
            {
                string tmp = Console.ReadLine();
                try
                {
                    string[] temp = tmp.Split(' ');
                    string data = "";
                    if (temp.Length > 1)
                    {
                        for (int i = 1; i < temp.Length; i++)
                        {
                            data = data + temp[i];
                        }
                    }
                    else
                    {
                        data = temp[0];
                    }
                    switch (temp[0])
                    {
                        case "/clear": case "clear": case "/cls": case "cls":
                        Console.Clear();               
                        Console.WriteLine("Console cancellata!");
                        Console.WriteLine("");
                        break;

                        case "/help": case "help":
                        Console.WriteLine("");                       
                        Console.WriteLine("Comandi implementati:");
                        Console.WriteLine("/help --- Mostra questo menù.");
                        Console.WriteLine("/clear(/cls) --- Cancella la console. ");
                        Console.WriteLine("/clearlogs --- Cancella i log dal database. ");
                        Console.WriteLine("/exit --- Chiude il server. ");
                        Console.WriteLine("");
                        break;

                        case "/exit": case "exit":
                        Console.WriteLine("");
                        Console.WriteLine("Il server si chiuderà in 10 secondi... ");
                        Thread.Sleep(10000);
                        Environment.Exit(0);
                        break;

                        case "/clearlogs": case "clearlogs":
                        Console.WriteLine("");
                        DatabaseClient dbClient = Server.GetDatabase().GetClient();
                        dbClient.ExecuteQuery("TRUNCATE TABLE chatloggs");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Chatlog del database cancellata!");
                        Console.WriteLine("");
                        break;

                        default: Console.WriteLine("--- ATTENZIONE: Comando non riconosciuto! (/help) ---");
                        break;
                    }
                }
                catch
                {

                }
            }
        }

        public static DatabaseManager GetDatabase()
        {
            return DatabaseManager;
        }
    }
}