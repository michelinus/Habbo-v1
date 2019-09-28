using System;
using System.IO;
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
            if (!File.Exists("Server.ini"))
            {
                Console.WriteLine("File di configurazione non esistente. Creazione in corso...");
                if (File.Exists("config.default"))
                {
                    File.Copy("config.default", "Server.ini");
                    Console.WriteLine("File di configurazione creato. Compilarlo con i propri valori.");
                    Console.WriteLine("Chiusura del programma automatica. Riavviare il server.");
                    Thread.Sleep(7000);
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("File di configurazione di backup non esistente. ");
                    Console.WriteLine("Rivolgersi alla pagina Github.");
                    Thread.Sleep(7000);
                    Environment.Exit(0);
                };

            }

            Console.Title = "Avvio Emulatore...";
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
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("");
            Console.WriteLine("                     ▄  █ ██   ███   ███   ████▄        ▄▄▄▄▄   ▄███▄   █▄▄▄▄    ▄   ▄███▄   █▄▄▄▄");
            Console.WriteLine("                    █   █ █ █  █  █  █  █  █   █       █     ▀▄ █▀   ▀  █  ▄▀     █  █▀   ▀  █  ▄▀");
            Console.WriteLine("                    ██▀▀█ █▄▄█ █ ▀ ▄ █ ▀ ▄ █   █     ▄  ▀▀▀▀▄   ██▄▄    █▀▀▌ █     █ ██▄▄    █▀▀▌");
            Console.WriteLine("                    █   █ █  █ █  ▄▀ █  ▄▀ ▀████      ▀▄▄▄▄▀    █▄   ▄▀ █  █  █    █ █▄   ▄▀ █  █  ");
            Console.WriteLine("                    █     █  ███   ███                          ▀███▀     █    █  █  ▀███▀     █   ");
            Console.WriteLine("                    ▀     █                                               ▀     █▐             ▀    ");
            Console.WriteLine("                          ▀                                                      ▐                  ");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("L'hotel avente nome " + "'" + hotel_name + "'" + " è stato avviato correttamente, buon divertimento!");
            Console.WriteLine("Indirizzo IP: " + ip + " | " + "Max Utenti: " + max_users + "!");
            Console.WriteLine("");

            Thread t = new Thread(console_listener);
            t.Start();
            while (true)
            {
                Process pmemory = Process.GetCurrentProcess();
                Console.Title = "| Memoria occupata dal server: " + pmemory.PrivateMemorySize64 / 1024 / 1024 +"Mb" + " | ";
                Thread.Sleep(100);
            }
        }
        private static void console_listener()
        {
            var config = new IniFile("Server.ini");
            String debug = config.Read("enabled", "Debug");
            bool enabledebug = bool.Parse(debug);
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
                    if (enabledebug == false)
                    {
                        switch (temp[0])
                        {
                            case "/clear":
                            case "/cls":
                                Console.Clear();
                                Console.WriteLine("Console cancellata!");
                                Console.WriteLine("");
                                break;

                            case "/help":
                                Console.WriteLine("");
                                Console.WriteLine("Comandi del server:");
                                Console.WriteLine("/help --- Mostra questo menù.");
                                Console.WriteLine("/clear(/cls) --- Cancella la console. ");
                                Console.WriteLine("/clearchatlogs --- Cancella i log dal database. ");
                                Console.WriteLine("/exit --- Chiude il server. ");
                                Console.WriteLine("");
                                break;

                            case "/exit":
                                Console.WriteLine("");
                                Console.WriteLine("Il server si chiuderà in 10 secondi... ");
                                Thread.Sleep(10000);
                                Environment.Exit(0);
                                break;

                            case "/clearchatlogs":
                                Console.WriteLine("");
                                DatabaseClient dbClient = Server.GetDatabase().GetClient();
                                dbClient.ExecuteQuery("TRUNCATE TABLE chatloggs");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Chatlog del database cancellata!");
                                Console.WriteLine("");
                                break;

                            default:
                                Console.WriteLine("--- ATTENZIONE: Comando non riconosciuto! (/help) ---");
                                break;
                        }
                    }
                    else Console.WriteLine("--- Modalità debug disattivata, impossibile inserire comandi. ---");
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