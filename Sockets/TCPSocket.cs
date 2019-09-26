using System;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using Storage;
using System.Data;
using System.Threading;
using System.Net.Mail;
using System.Net;

namespace ServerEngine.Sockets
{
    class TCPSocket
    {
        public int ID;

        public TCPSocket(int id)
        {
            ID = id;
        }
        public void run()
        {
            try
            {
                //
                var config = new IniFile("Server.ini");
                DatabaseClient dbClient = Server.GetDatabase().GetClient();
                string svIP = config.Read("ip", "Server");
                string svPort = config.Read("port", "Server");
                string debug = config.Read("enabled", "Debug");
                bool enabledebug = bool.Parse(debug);
                //

                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                byte[] tmp_hello = enc.GetBytes(PacketHelper.Build_Send("HELLO" + (char)13).ToCharArray());
                Console.ForegroundColor = ConsoleColor.DarkYellow;

                if(enabledebug == true) Console.WriteLine("<-- HELLO {13} ");
                Console.ForegroundColor = ConsoleColor.Gray;

                Server.Users[ID].Sck.GetStream().Write(tmp_hello, 0, tmp_hello.Length);
                while (Server.Users[ID].Sck.Connected)
                {
                    if (Server.Users[ID].Sck.Available > 0)
                    {
                        byte[] len_info_byte = new byte[4];
                        Stream tmp = Server.Users[ID].Sck.GetStream();
                        tmp.Read(len_info_byte, 0, 4);
                        string len_info_string = Encoding.ASCII.GetString(len_info_byte);
                        int len_info_int = int.Parse(len_info_string);
                        byte[] PaketData_byte = new byte[len_info_int];
                        tmp.Read(PaketData_byte, 0, len_info_int);
                        string PaketData_string = Encoding.ASCII.GetString(PaketData_byte);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        string PaketData_string2 = PaketData_string;
                        if (enabledebug == true) Console.WriteLine("--> " + PaketData_string.Replace("\r", " {13} "));
                        Console.ForegroundColor = ConsoleColor.Gray;

                        if (Server.Users[ID].name != null && Server.Users[ID].inroom == 0)
                        {
                            Server.Users[ID].owner = 0;
                            try
                            {
                                Server.Users[ID].inroom = dbClient.ReadInt32("SELECT inroom FROM users WHERE name = '" + Server.Users[ID].name + "'");
                            }
                            catch
                            {
                                Server.Users[ID].inroom = 0;
                            }
                        }

                        string[] header = PaketData_string.Split(' ');
                        switch (header[0].ToUpper())
                        {
                            // SocketServer
                            #region VERSIONCHECK ##
                            case "VERSIONCHECK":
                                {
                                    if (header[1] == "client002" || header[1] == "client003")
                                    {
                                        Send_Data("ENCRYPTION_OFF");
                                        Send_Data("SECRET_KEY" + (char)13 + "1337");
                                    }
                                }
                                break;
                            #endregion

                            #region LOGIN *
                            case "LOGIN":
                                {
                                    try
                                    {
                                        string checkdata = dbClient.ReadString("SELECT * FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                        if (checkdata != null)
                                        {
                                            Server.Users[ID].name = header[1];
                                            Server.Users[ID].password = header[2];
                                            Server.Users[ID].credits = dbClient.ReadInt32("SELECT credits FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].email = dbClient.ReadString("SELECT email FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].figure = dbClient.ReadString("SELECT figure FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].birthday = dbClient.ReadString("SELECT birthday FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].phonenumber = dbClient.ReadString("SELECT phonenumber FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].customData = dbClient.ReadString("SELECT customData FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].had_read_agreement = dbClient.ReadInt32("SELECT had_read_agreement FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].sex = dbClient.ReadString("SELECT sex FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].country = dbClient.ReadString("SELECT country FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].has_special_rights = dbClient.ReadString("SELECT has_special_rights FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].badge_type = dbClient.ReadString("SELECT badge_type FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");

                                            try
                                            {
                                                if (header[3] == "0")
                                                {
                                                    Server.Users[ID].server = 1;

                                                    #region SHIT!
                                                    Send_Data("OBJECTS WORLD 0 lobby_a" + (char)13 +
                                                        "f90 flower1 9 0 7 0" + (char)13 +
                                                        "S110 chairf2b 11 0 7 4" + (char)13 +
                                                        "s120 chairf2 12 0 7 4" + (char)13 +
                                                        "t130 table1 13 0 7 2" + (char)13 +
                                                        "S140 chairf2b 14 0 7 4" + (char)13 +
                                                        "s150 chairf2 15 0 7 4" + (char)13 +
                                                        "w160 watermatic 16 0 7 4" + (char)13 +
                                                        "T92 telkka 9 2 7 2" + (char)13 +
                                                        "f93 flower1 9 3 7 0" + (char)13 +
                                                        "Z113 chairf2d 11 3 7 0" + (char)13 +
                                                        "s123 chairf2 12 3 7 0" + (char)13 +
                                                        "t133 table1 13 3 7 2" + (char)13 +
                                                        "Z143 chairf2d 14 3 7 0" + (char)13 +
                                                        "s153 chairf2 15 3 7 0" + (char)13 +
                                                        "f124 flower1 12 4 3 0" + (char)13 +
                                                        "f164 flower1 16 4 3 0" + (char)13 +
                                                        "S07 chairf2b 0 7 3 4" + (char)13 +
                                                        "s17 chairf2 1 7 3 4" + (char)13 +
                                                        "Z010 chairf2d 0 10 3 0" + (char)13 +
                                                        "s110 chairf2 1 10 3 0" + (char)13 +
                                                        "r2112 roommatic 21 12 1 4" + (char)13 +
                                                        "r2212 roommatic 22 12 1 4" + (char)13 +
                                                        "r2312 roommatic 23 12 1 4" + (char)13 +
                                                        "r2412 roommatic 24 12 1 4" + (char)13 +
                                                        "S014 chairf2b 0 14 3 4" + (char)13 +
                                                        "s114 chairf2 1 14 3 4" + (char)13 +
                                                        "w1314 watermatic 13 14 1 2" + (char)13 +
                                                        "w1215 watermatic 12 15 1 4" + (char)13 +
                                                        "c1916 chairf1 19 16 1 4" + (char)13 +
                                                        "C2116 table2c 21 16 1 2" + (char)13 +
                                                        "c2316 chairf1 23 16 1 4" + (char)13 +
                                                        "Z017 chairf2d 0 17 3 0" + (char)13 +
                                                        "s117 chairf2 1 17 3 0" + (char)13 +
                                                        "D2117 table2b 21 17 1 2" + (char)13 +
                                                        "c1918 chairf1 19 18 1 0" + (char)13 +
                                                        "d2118 table2 21 18 1 2" + (char)13 +
                                                        "c2318 chairf1 23 18 1 0" + (char)13 +
                                                        "S721 chairf2b 7 21 2 2" + (char)13 +
                                                        "z722 chairf2c 7 22 2 2" + (char)13 +
                                                        "z723 chairf2c 7 23 2 2" + (char)13 +
                                                        "z724 chairf2c 7 24 2 2" + (char)13 +
                                                        "s725 chairf2 7 25 2 2" + (char)13 +
                                                        "t726 table1 7 26 2 2" + (char)13 +
                                                        "e1026 flower2 10 26 1 2" + (char)13);
                                                    #endregion

                                                    string RoomMap = dbClient.ReadString("SELECT heightmap FROM heightmap WHERE model = 'lobby_a'");
                                                    Server.Users[ID].roomcache = RoomMap;

                                                    Send_Data("HEIGHTMAP" + RoomMap.Replace(" ", "\r"));
                                                    Send_Data("USERS" + (char)13 + " " + Server.Users[ID].name + " " + Server.Users[ID].figure.Replace("figure=", "") + " 3 5 0 " + Server.Users[ID].customData);
                                                    Server.Users[ID].userx = 12;
                                                    Server.Users[ID].usery = 27;
                                                    Send_Data("STATUS " + (char)13 + Server.Users[ID].name + " 12,27,1,0,0/mod 0/");
                                                }
                                            }
                                            catch (IndexOutOfRangeException)
                                            {
                                                // no header[3] exist -> private room loads
                                            }
                                        }
                                        else
                                        {
                                            Send_Data("ERROR: login incorrect");
                                        }
                                    }
                                    catch (NullReferenceException)
                                    {
                                        Send_Data("ERROR: login incorrect");
                                    }
                                }
                                break;
                            #endregion

                            #region INFORETRIEVE *
                            case "INFORETRIEVE":
                                {
                                    try
                                    {
                                        string checkdata = dbClient.ReadString("SELECT * FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                        if (checkdata != null)
                                        {
                                            Server.Users[ID].name = header[1];
                                            Server.Users[ID].password = header[2];
                                            Server.Users[ID].credits = dbClient.ReadInt32("SELECT credits FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].email = dbClient.ReadString("SELECT email FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].figure = dbClient.ReadString("SELECT figure FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].birthday = dbClient.ReadString("SELECT birthday FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].phonenumber = dbClient.ReadString("SELECT phonenumber FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].customData = dbClient.ReadString("SELECT customData FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].had_read_agreement = dbClient.ReadInt32("SELECT had_read_agreement FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].sex = dbClient.ReadString("SELECT sex FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].country = dbClient.ReadString("SELECT country FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].has_special_rights = dbClient.ReadString("SELECT has_special_rights FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");
                                            Server.Users[ID].badge_type = dbClient.ReadString("SELECT badge_type FROM users WHERE name = '" + header[1] + "' AND password = '" + header[2] + "'");

                                            Send_Data(
                                                "USEROBJECT" + (char)13 +
                                                "name=" + Server.Users[ID].name + (char)13 +
                                                "email=" + Server.Users[ID].email + (char)13 +
                                                "figure=" + Server.Users[ID].figure + (char)13 +
                                                "birthday=" + Server.Users[ID].birthday + (char)13 +
                                                "phonenumber=" + Server.Users[ID].phonenumber + (char)13 +
                                                "customData=" + Server.Users[ID].customData + (char)13 +
                                                "had_read_agreement=" + Server.Users[ID].had_read_agreement + (char)13 +
                                                "sex=" + Server.Users[ID].sex + (char)13 +
                                                "country=" + Server.Users[ID].country + (char)13 +
                                                "has_special_rights=" + Server.Users[ID].has_special_rights + (char)13 +
                                                "badge_type=" + Server.Users[ID].badge_type + (char)13);
                                        }
                                    }
                                    catch (NullReferenceException)
                                    {
                                        Send_Data("ERROR: login incorrect");
                                    }
                                }
                                break;
                            #endregion

                            #region INITUNITLISTENER ##
                            case "INITUNITLISTENER":
                                {
                                    string RoomData = "";
                                    DataTable Data = null;

                                    Data = dbClient.ReadDataTable("SELECT * FROM pubs");

                                    if (Data != null)
                                    {
                                        foreach (DataRow Row in Data.Rows)
                                        {
                                            RoomData = RoomData + (string)Row["name"] + "," + (int)Row["now_in"] + "," + (int)Row["max_in"] + "," + svIP + "/" + svIP + "," + svPort + "," + (string)Row["name"] + (char)9 + (string)Row["mapname"] + "," + (int)Row["now_in"] + "," + (int)Row["max_in"] + "," + (string)Row["heightmap"] + (char)13;
                                        }
                                    }

                                    Send_Data("ALLUNITS" + (char)13 + RoomData);
                                }
                                break;
                            #endregion

                            #region SEARCHBUSYFLATS *#
                            case "SEARCHBUSYFLATS":
                                {
                                    string RoomData = "";
                                    DataTable Data = null;

                                    Data = dbClient.ReadDataTable("SELECT * FROM rooms ORDER BY inroom DESC");

                                    if (Data != null)
                                    {
                                        foreach (DataRow Row in Data.Rows)
                                        {
                                            RoomData = RoomData + (char)13 + (int)Row["id"] + "/" + (string)Row["name"] + "/" + (string)Row["owner"] + "/" + (string)Row["door"] + "/" + (string)Row["pass"] + "/" + (string)Row["floor"] + "/" + svIP + "/" + svIP + "/" + svPort + "/" + (int)Row["inroom"] + "/null/" + (string)Row["desc"] + "";
                                        }
                                    }

                                    Send_Data("BUSY_FLAT_RESULTS 1" + RoomData);
                                }
                                break;
                            #endregion

                            #region GETCREDITS ##
                            case "GETCREDITS":
                                {
                                    Send_Data("WALLETBALANCE" + (char)13 + Server.Users[ID].credits);
                                    Send_Data("MESSENGERSMSACCOUNT" + (char)13 + "noaccount");
                                    Send_Data("MESSENGERREADY " + (char)13);
                                }
                                break;
                            #endregion

                            #region REGISTER *
                            case "REGISTER":
                                {
                                    string[] PacketData = PaketData_string.Split((char)13);
                                    string RegUserName = Regex.Split(PacketData[0], "=")[1];
                                    string RegUserPass = Regex.Split(PacketData[1], "=")[1];
                                    string RegUserMail = Regex.Split(PacketData[2], "=")[1];
                                    string RegUserFigure = PacketData[3].PadRight(PacketData[3].Length - 7).Replace("figure=", "");
                                    string RegUserDate = Regex.Split(PacketData[5], "=")[1];
                                    string RegUserPhone = Regex.Split(PacketData[6], "=")[1];
                                    string RegUserMission = Regex.Split(PacketData[7], "=")[1];
                                    string RegUserAgree = Regex.Split(PacketData[8], "=")[1];
                                    string RegUserSex = Regex.Split(PacketData[9], "=")[1];
                                    string RegUserCity = Regex.Split(PacketData[10], "=")[1];

                                    dbClient.ExecuteQuery("INSERT INTO users (`id`, `name`, `password`, `credits`, `email`, `figure`, `birthday`, `phonenumber`, `customData`, `had_read_agreement`, `sex`, `country`, `has_special_rights`, `badge_type`) VALUES (NULL, '" + RegUserName + "', '" + RegUserPass + "', '1337', '" + RegUserMail + "', '" + RegUserFigure + "', '" + RegUserDate + "', '" + RegUserPhone + "', '" + RegUserMission + "', '" + RegUserAgree + "', '" + RegUserSex + "', '" + RegUserCity + "', '0', '0');");
                                }
                                break;
                            #endregion

                            #region UPDATE #
                            case "UPDATE":
                                {
                                    string[] PacketData = PaketData_string.Split((char)13);
                                    string UpdUserName = Regex.Split(PacketData[0], "=")[1];
                                    string UpdUserPass = Regex.Split(PacketData[1], "=")[1];
                                    string UpdUserMail = Regex.Split(PacketData[2], "=")[1];
                                    string UpdUserFigure = PacketData[3].PadRight(PacketData[3].Length - 7).Replace("figure=", "");
                                    string UpdUserDate = Regex.Split(PacketData[5], "=")[1];
                                    string UpdUserPhone = Regex.Split(PacketData[6], "=")[1];
                                    string UpdUserMission = Regex.Split(PacketData[7], "=")[1];
                                    string UpdUserAgree = Regex.Split(PacketData[8], "=")[1];
                                    string UpdUserSex = Regex.Split(PacketData[9], "=")[1];
                                    string UpdUserCity = Regex.Split(PacketData[10], "=")[1];

                                    dbClient.ExecuteQuery("UPDATE users SET password =  '" + UpdUserPass + "', email = '" + UpdUserMail + "', figure = '" + UpdUserFigure + "', birthday = '" + UpdUserDate + "', phonenumber = '" + UpdUserPhone + "', had_read_agreement = '" + UpdUserAgree + "', country = '" + UpdUserCity + "', sex = '" + UpdUserSex + "', customData = '" + UpdUserMission + "' WHERE name = '" + UpdUserName + "' LIMIT 1");
                                }
                                break;
                            #endregion

                            #region SEARCHFLATFORUSER *
                            case "SEARCHFLATFORUSER":
                                {
                                    string GetFromUser = PaketData_string.Split('/')[1];
                                    string RoomData = "";
                                    DataTable Data = null;

                                    Data = dbClient.ReadDataTable("SELECT * FROM rooms WHERE owner = '" + GetFromUser + "'");

                                    if (Data != null)
                                    {
                                        foreach (DataRow Row in Data.Rows)
                                        {
                                            RoomData = RoomData + (char)13 + (int)Row["id"] + "/" + (string)Row["name"] + "/" + (string)Row["owner"] + "/" + (string)Row["door"] + "/" + (string)Row["pass"] + "/" + (string)Row["floor"] + "/83.117.80.215/83.117.80.215/37120/0/null/" + (string)Row["desc"] + "";
                                        }
                                    }

                                    Send_Data("BUSY_FLAT_RESULTS 1" + RoomData);
                                }
                                break;
                            #endregion

                            #region UINFO_MATCH *
                            case "UINFO_MATCH":
                                {
                                    string[] UInfo = PaketData_string.Split('/');

                                    DataTable Data = null;

                                    Data = dbClient.ReadDataTable("SELECT * FROM users WHERE name = '" + UInfo[1] + "'");

                                    if (Data == null)
                                    {
                                        Send_Data("NOSUCHUSER MESSENGER" + (char)13 + UInfo[1]);
                                    }
                                    else
                                    {
                                        foreach (DataRow Row in Data.Rows)
                                        {
                                            Send_Data("MEMBERINFO MESSENGER" + (char)13 + UInfo[1] + (char)13 + (string)Row["customData"] + (char)13 + "" + (char)13 + "0" + (char)13 + (string)Row["figure"].ToString().Replace("figure=", "") + (string)Row["sex"]);
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region GETFLATINFO *
                            case "GETFLATINFO":
                                {
                                    string[] GetFlatID = PaketData_string.Split('/');
                                    Send_Data("SETFLATINFO" + (char)13 + "/1/");
                                }
                                break;
                            #endregion

                            #region DELETEFLAT ##
                            case "DELETEFLAT":
                                {
                                    string[] GetFlatID = PaketData_string.Split('/');
                                    dbClient.ExecuteQuery("DELETE FROM rooms WHERE id = '" + GetFlatID[1] + "'");
                                }
                                break;
                            #endregion

                            #region GET_FAVORITE_ROOMS *
                            case "GET_FAVORITE_ROOMS":
                                {
                                    DataTable Data = null;
                                    string RoomData = "";

                                    Data = dbClient.ReadDataTable("SELECT * FROM favrooms WHERE user = '" + header[1] + "'");

                                    if (Data != null)
                                    {
                                        foreach (DataRow Row in Data.Rows)
                                        {
                                            DataTable Data2 = null;

                                            Data2 = dbClient.ReadDataTable("SELECT * FROM rooms WHERE id = '" + (int)Row["roomid"] + "' LIMIT 1");

                                            if (Data2 != null)
                                            {
                                                foreach (DataRow Row2 in Data2.Rows)
                                                {
                                                    RoomData = RoomData + (char)13 + (int)Row2["id"] + "/" + (string)Row2["name"] + "/" + (string)Row2["owner"] + "/" + (string)Row2["door"] + "/" + (string)Row2["pass"] + "/" + (string)Row2["floor"] + "/83.117.80.215/83.117.80.215/37120/" + (int)Row2["inroom"] + "/null/" + (string)Row2["desc"] + "";
                                                }
                                            }
                                        }
                                    }

                                    Send_Data("BUSY_FLAT_RESULTS 1" + RoomData);
                                }
                                break;
                            #endregion

                            #region ADD_FAVORITE_ROOM *
                            case "ADD_FAVORITE_ROOM":
                                {
                                    dbClient.ExecuteQuery("INSERT INTO favrooms VALUES (NULL, '" + Server.Users[ID].name + "', '" + header[1] + "')");
                                }
                                break;
                            #endregion

                            #region DEL_FAVORITE_ROOM #
                            case "DEL_FAVORITE_ROOM":
                                {
                                    dbClient.ExecuteQuery("DELETE FROM favrooms WHERE roomid = '" + header[1] + "' LIMIT 1");
                                }
                                break;
                            #endregion

                            #region SEARCHFLAT *
                            case "SEARCHFLAT":
                                {
                                    string[] SearchLike = PaketData_string.Split('/');

                                    DataTable Data = null;
                                    string RoomData = "";

                                    Data = dbClient.ReadDataTable("SELECT * FROM rooms WHERE owner LIKE '" + SearchLike[1] + "' OR name LIKE '" + SearchLike[1] + "'");

                                    if (Data != null)
                                    {
                                        foreach (DataRow Row in Data.Rows)
                                        {
                                            RoomData = RoomData + (char)13 + (int)Row["id"] + "/" + (string)Row["name"] + "/" + (string)Row["owner"] + "/" + (string)Row["door"] + "/" + (string)Row["pass"] + "/" + (string)Row["floor"] + "/83.117.80.215/83.117.80.215/37120/" + (int)Row["inroom"] + "/null/" + (string)Row["desc"] + "";
                                        }
                                    }

                                    Send_Data("BUSY_FLAT_RESULTS 1" + RoomData);
                                }
                                break;
                            #endregion

                            #region SEND_USERPASS_TO_EMAIL *
                            case "SEND_USERPASS_TO_EMAIL":
                                {
                                    string rlemail = dbClient.ReadString("SELECT email FROM users WHERE name = '" + header[1] + "' AND email = '" + header[2] + "'");
                                    string rlpasswd = dbClient.ReadString("SELECT password FROM users WHERE name = '" + header[1] + "' AND email = '" + header[2] + "'");

                                    if (rlemail != null)
                                    {
                                        var client = new SmtpClient("smtp.gmail.com", 587)
                                        {
                                            Credentials = new NetworkCredential("prjOwnage@gmail.com", "lolwutpwn"),
                                            EnableSsl = true
                                        };
                                        client.Send("prjOwnage@gmail.com", rlemail, "Habbo Hotel", "Hey, " + header[1] + "\nYou have clicked on the \"Forgotten your password??\" button,\n\nYour password is: " + rlpasswd);
                                    }
                                }
                                break;
                            #endregion

                            //P&P Server
                            #region TRYFLAT #
                            case "TRYFLAT":
                                {
                                    int gotoroom = int.Parse(PaketData_string.Split('/')[1]);
                                    if (Server.Users[ID].inroom != 0)
                                    {
                                        dbClient.ExecuteQuery("UPDATE rooms SET inroom = inroom - 1 WHERE id = '" + Server.Users[ID].inroom + "'");
                                        Server.Users[ID].owner = 0;
                                    }
                                    dbClient.ExecuteQuery("UPDATE users SET inroom = '" + gotoroom + "' WHERE name = '" + Server.Users[ID].name + "'");
                                    dbClient.ExecuteQuery("UPDATE rooms SET inroom = inroom + 1 WHERE id = '" + gotoroom + "'");
                                    Server.Users[ID].inroom = gotoroom;

                                    try
                                    {
                                        DataTable Data = null;
                                        bool RightPasswd = false;

                                        Data = dbClient.ReadDataTable("SELECT * FROM rooms WHERE id = '" + gotoroom + "'");

                                        if (Data != null)
                                        {
                                            foreach (DataRow Row in Data.Rows)
                                            {
                                                if ((string)Row["owner"] != Server.Users[ID].name && Server.Users[ID].has_special_rights != "1")
                                                {
                                                    if ((string)Row["pass"] == PaketData_string.Split('/')[2])
                                                    {
                                                        RightPasswd = true;
                                                    }
                                                }
                                                else
                                                {
                                                    RightPasswd = true;
                                                }
                                            }
                                        }

                                        if (RightPasswd == false)
                                        {
                                            Server.Users[ID].Sck.Close();
                                            break;
                                        }
                                    }
                                    catch
                                    {
                                        // No password on the room,...
                                    }

                                    Send_Data("FLAT_LETIN");
                                }
                                break;
                            #endregion

                            #region GOTOFLAT **
                            case "GOTOFLAT":
                                {
                                    int gotoroom = int.Parse(PaketData_string.Split('/')[1]);

                                    DataTable Data = null;

                                    Data = dbClient.ReadDataTable("SELECT * FROM rooms WHERE id = '" + gotoroom + "' LIMIT 1");

                                    if (Data == null)
                                    {
                                        return;
                                    }

                                    foreach (DataRow Row in Data.Rows)
                                    {
                                        Send_Data("ROOM_READY" + (char)13 + (string)Row["desc"]);
                                        Send_Data("FLATPROPERTY" + (char)13 + "wallpaper/" + (int)Row["space_w"] + (char)13);
                                        Send_Data("FLATPROPERTY" + (char)13 + "floor/" + (int)Row["space_f"] + (char)13);

                                        if ((string)Row["owner"].ToString().ToLower() == Server.Users[ID].name.ToLower())
                                        {
                                            Server.Users[ID].owner = 1;
                                            Send_Data("YOUAREOWNER");
                                        }

                                        DataTable Data02 = null;

                                        Data02 = dbClient.ReadDataTable("SELECT * FROM rights WHERE user = '" + Server.Users[ID].name + "' LIMIT 1");

                                        Server.Users[ID].rights = 0;

                                        if (Data02 != null)
                                        {
                                            foreach (DataRow Row02 in Data02.Rows)
                                            {
                                                Server.Users[ID].rights = 1;
                                                Send_Data("YOUARECONTROLLER");
                                            }
                                        }

                                        string RoomMap = dbClient.ReadString("SELECT heightmap FROM heightmap WHERE model = '" + (string)Row["model"] + "'");
                                        Server.Users[ID].roomcache = RoomMap;

                                        Send_Data("HEIGHTMAP" + (char)13 + RoomMap.Replace(" ", "\r"));
                                        Send_Data("OBJECTS WORLD 0 " + (string)Row["model"]);

                                        DataTable RoomItemData = null;
                                        string RoomTotalData = "";

                                        RoomItemData = dbClient.ReadDataTable("SELECT * FROM roomitems WHERE roomid = '" + gotoroom + "'");

                                        if (RoomItemData != null)
                                        {
                                            foreach (DataRow RowXX in RoomItemData.Rows)
                                            {
                                                int zero = (int)RowXX["id"];
                                                string zstring = "00000000000";
                                                int j = 0;
                                                while (j < zero.ToString().Length)
                                                {
                                                    zstring = zstring + "0";
                                                    j++;
                                                }

                                                RoomTotalData = RoomTotalData + (char)13 + zstring + (int)RowXX["id"] + "," + (string)RowXX["textname"] + " " + (int)RowXX["x"] + " " + (int)RowXX["y"] + " " + (string)RowXX["size"] + " " + (int)RowXX["rotate"] + " 0.0 " + (string)RowXX["rgb"] + " /" + (string)RowXX["itemname"] + "/" + (string)RowXX["itemdesc"] + "/";
                                            }
                                        }

                                        Send_Data("ACTIVE_OBJECTS " + RoomTotalData);
                                        Send_Data("ITEMS " + (char)13 + "6663" + (char)9 + "poster" + (char)9 + " " + (char)9 + "frontwall 6.9999,6.3500/21");

                                        DataTable Data2 = null;

                                        Data2 = dbClient.ReadDataTable("SELECT * FROM users WHERE inroom = '" + gotoroom + "' AND name != '" + Server.Users[ID].name + "'");

                                        if (Data == null)
                                        {
                                            return;
                                        }

                                        foreach (DataRow Row2 in Data2.Rows)
                                        {
                                            Send_Data("USERS" + (char)13 + " " + (string)Row2["name"] + " " + (string)Row2["figure"].ToString().Replace("figure=", "") + " 3 5 0 " + (string)Row2["customData"]);
                                        }

                                        if (Server.Users[ID].owner == 1 || Server.Users[ID].rights == 1)
                                        {
                                            Send_Data_All("STATUS " + (char)13 + Server.Users[ID].name + " 0,0,99,2,2/flatctrl useradmin/mod 0/");
                                            Send_Data_Room("USERS" + (char)13 + " " + Server.Users[ID].name + " " + Server.Users[ID].figure.Replace("figure=", "") + " 3 5 0 " + Server.Users[ID].customData);
                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " 3,5,0,2,2/flatctrl useradmin/mod 0/");
                                        }
                                        else
                                        {
                                            Send_Data_All("STATUS " + (char)13 + Server.Users[ID].name + " 0,0,99,2,2/mod 0/");
                                            Send_Data_Room("USERS" + (char)13 + " " + Server.Users[ID].name + " " + Server.Users[ID].figure.Replace("figure=", "") + " 3 5 0 " + Server.Users[ID].customData);
                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " 3,5,0,2,2/mod 0/");
                                        }
                                        Server.Users[ID].userx = 3;
                                        Server.Users[ID].usery = 5;
                                    }
                                }
                                break;
                            #endregion

                            #region SHOUT *
                            case "SHOUT":
                                {
                                    DataTable Data = null;
                                    string ChatMessage = PaketData_string.Replace("SHOUT ", "").Replace("'", "").Replace("\"", "").Replace("#", "");

                                    if (ChatMessage.StartsWith("/"))
                                    {
                                        if (Server.Users[ID].has_special_rights == "1")
                                        {
                                            #region /cleanhand
                                            if (ChatMessage.StartsWith("/cleanhand"))
                                            {
                                                dbClient.ExecuteQuery("DELETE FROM handitems WHERE user = '" + header[2] + "'");

                                                //Send_Data("SYSTEMBROADCAST" + (char)13 + "Hand successful cleaned!");
                                            }
                                            #endregion

                                            #region /teleport
                                            if (ChatMessage.StartsWith("/teleport"))
                                            {
                                                if (Server.Users[ID].teleport == 1)
                                                {
                                                    Server.Users[ID].teleport = 0;
                                                    //Send_Data("SYSTEMBROADCAST" + (char)13 + "TelePort disabled!");
                                                }
                                                else
                                                {
                                                    Server.Users[ID].moonwalk = 0;
                                                    Server.Users[ID].teleport = 1;
                                                    //Send_Data("SYSTEMBROADCAST" + (char)13 + "TelePort enabled!");
                                                }
                                            }
                                            #endregion

                                            #region /credits
                                            if (ChatMessage.StartsWith("/credits"))
                                            {
                                                dbClient.ExecuteQuery("UPDATE users SET credits = credits + " + header[3] + " WHERE name = '" + header[2] + "'");

                                                //Send_Data("SYSTEMBROADCAST" + (char)13 + "Gave successful credits!");
                                            }
                                            #endregion

                                            #region /changename
                                            if (ChatMessage.StartsWith("/changename"))
                                            {
                                                Server.Users[ID].name = header[2];
                                            }
                                            #endregion
                                        }

                                        #region /moonwalk
                                        if (ChatMessage.StartsWith("/moonwalk"))
                                        {
                                            if (Server.Users[ID].moonwalk == 1)
                                            {
                                                Server.Users[ID].moonwalk = 0;
                                                //Send_Data("SYSTEMBROADCAST" + (char)13 + "MoonWalk disabled!");
                                            }
                                            else
                                            {
                                                Server.Users[ID].teleport = 0;
                                                Server.Users[ID].moonwalk = 1;
                                                //Send_Data("SYSTEMBROADCAST" + (char)13 + "MoonWalk enabled!");
                                            }
                                        }
                                        #endregion
                                    }

                                    Data = dbClient.ReadDataTable("SELECT * FROM wordfilter");

                                    if (Data == null)
                                    {
                                        return;
                                    }

                                    foreach (DataRow Row in Data.Rows)
                                    {
                                        ChatMessage = ChatMessage.Replace((string)Row["word"], (string)Row["filter"]);
                                    }

                                    Send_Data_Room("SHOUT" + (char)13 + Server.Users[ID].name + " " + ChatMessage);

                                    dbClient.ExecuteQuery("INSERT INTO chatloggs VALUES (NULL, '" + Server.Users[ID].name + "',  '" + ChatMessage + "', '" + Server.Users[ID].inroom + "', 'SHOUT')");
                                }
                                break;
                            #endregion

                            #region CHAT *
                            case "CHAT":
                                {
                                    DataTable Data = null;
                                    string ChatMessage = PaketData_string.Replace("CHAT ", "").Replace("'", "").Replace("\"", "").Replace("#", "");

                                    if (ChatMessage.StartsWith("/"))
                                    {
                                        if (Server.Users[ID].has_special_rights == "1")
                                        {
                                            #region /cleanhand
                                            if (ChatMessage.StartsWith("/cleanhand"))
                                            {
                                                dbClient.ExecuteQuery("DELETE FROM handitems WHERE user = '" + header[2] + "'");
                                            }
                                            #endregion

                                            #region /teleport
                                            if (ChatMessage.StartsWith("/teleport"))
                                            {
                                                if (Server.Users[ID].teleport == 1)
                                                {
                                                    Server.Users[ID].teleport = 0;
                                                }
                                                else
                                                {
                                                    Server.Users[ID].moonwalk = 0;
                                                    Server.Users[ID].teleport = 1;
                                                }
                                            }
                                            #endregion

                                            #region /action
                                            if (ChatMessage.StartsWith("/action"))
                                            {
                                                Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + ",0,2,2/" + header[2] + "/");
                                            }
                                            #endregion

                                            #region /credits
                                            if (ChatMessage.StartsWith("/credits"))
                                            {
                                                dbClient.ExecuteQuery("UPDATE users SET credits = credits + " + header[3] + " WHERE name = '" + header[2] + "'");
                                            }
                                            #endregion

                                            #region /changename
                                            if (ChatMessage.StartsWith("/changename"))
                                            {
                                                Server.Users[ID].name = header[2];
                                            }
                                            #endregion
                                        }

                                        #region /moonwalk
                                        if (ChatMessage.StartsWith("/moonwalk"))
                                        {
                                            if (Server.Users[ID].moonwalk == 1)
                                            {
                                                Server.Users[ID].moonwalk = 0;
                                                //Send_Data("SYSTEMBROADCAST" + (char)13 + "MoonWalk disabled!");
                                            }
                                            else
                                            {
                                                Server.Users[ID].teleport = 0;
                                                Server.Users[ID].moonwalk = 1;
                                                //Send_Data("SYSTEMBROADCAST" + (char)13 + "MoonWalk enabled!");
                                            }
                                        }
                                        #endregion
                                    }

                                    Data = dbClient.ReadDataTable("SELECT * FROM wordfilter");

                                    if (Data == null)
                                    {
                                        return;
                                    }

                                    foreach (DataRow Row in Data.Rows)
                                    {
                                        ChatMessage = ChatMessage.Replace((string)Row["word"], (string)Row["filter"]);
                                    }

                                    Send_Data_Room("CHAT" + (char)13 + Server.Users[ID].name + " " + ChatMessage);

                                    dbClient.ExecuteQuery("INSERT INTO chatloggs VALUES (NULL, '" + Server.Users[ID].name + "',  '" + ChatMessage + "', '" + Server.Users[ID].inroom + "', 'CHAT')");
                                }
                                break;
                            #endregion

                            #region WHISPER *
                            case "WHISPER":
                                {
                                    DataTable Data = null;
                                    string ChatMessage = PaketData_string.Replace("WHISPER ", "").Replace("'", "").Replace("\"", "").Replace("#", "");

                                    if (ChatMessage.StartsWith("/"))
                                    {
                                        if (Server.Users[ID].has_special_rights == "1")
                                        {
                                            #region /cleanhand
                                            if (ChatMessage.StartsWith("/cleanhand"))
                                            {
                                                dbClient.ExecuteQuery("DELETE FROM handitems WHERE user = '" + header[2] + "'");
                                            }
                                            #endregion

                                            #region /teleport
                                            if (ChatMessage.StartsWith("/teleport"))
                                            {
                                                if (Server.Users[ID].teleport == 1)
                                                {
                                                    Server.Users[ID].teleport = 0;
                                                    //Send_Data("SYSTEMBROADCAST" + (char)13 + "TelePort disabled!");
                                                }
                                                else
                                                {
                                                    Server.Users[ID].moonwalk = 0;
                                                    Server.Users[ID].teleport = 1;
                                                    //Send_Data("SYSTEMBROADCAST" + (char)13 + "TelePort enabled!");
                                                }
                                            }
                                            #endregion

                                            #region /credits
                                            if (ChatMessage.StartsWith("/credits"))
                                            {
                                                dbClient.ExecuteQuery("UPDATE users SET credits = credits + " + header[3] + " WHERE name = '" + header[2] + "'");
                                            }
                                            #endregion

                                            #region /changename
                                            if (ChatMessage.StartsWith("/changename"))
                                            {
                                                Server.Users[ID].name = header[2];
                                            }
                                            #endregion
                                        }

                                        #region /moonwalk
                                        if (ChatMessage.StartsWith("/moonwalk"))
                                        {
                                            if (Server.Users[ID].moonwalk == 1)
                                            {
                                                Server.Users[ID].moonwalk = 0;
                                                //Send_Data("SYSTEMBROADCAST" + (char)13 + "MoonWalk disabled!");
                                            }
                                            else
                                            {
                                                Server.Users[ID].teleport = 0;
                                                Server.Users[ID].moonwalk = 1;
                                                //Send_Data("SYSTEMBROADCAST" + (char)13 + "MoonWalk enabled!");
                                            }
                                        }
                                        #endregion
                                    }

                                    Data = dbClient.ReadDataTable("SELECT * FROM wordfilter");


                                    if (Data == null)
                                    {
                                        return;
                                    }

                                    foreach (DataRow Row in Data.Rows)
                                    {
                                        ChatMessage = ChatMessage.Replace((string)Row["word"], (string)Row["filter"]);
                                    }

                                    Send_Data_Room("WHISPER" + (char)13 + Server.Users[ID].name + " " + ChatMessage);

                                    dbClient.ExecuteQuery("INSERT INTO chatloggs VALUES (NULL, '" + Server.Users[ID].name + "',  '" + ChatMessage + "', '" + Server.Users[ID].inroom + "', 'WHISPER')");

                                }
                                break;
                            #endregion

                            #region STATUSOK #
                            case "STATUSOK":
                                {
                                    Send_Data("OK");
                                }
                                break;
                            #endregion

                            #region MOVE *
                            case "MOVE":
                                {
                                    try
                                    {
                                        int ToX = int.Parse(header[1]);
                                        int ToY = int.Parse(header[2]);

                                        string OwnData = "";
                                        if (Server.Users[ID].owner == 1 || Server.Users[ID].rights == 1)
                                        {
                                            OwnData = "flatctrl useradmin/";
                                        }

                                        if (Server.Users[ID].teleport == 1)
                                        {
                                            string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[ToY].Substring(ToX, 1);
                                            string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[ToY].Substring(ToX, 1);

                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",0,0 /" + OwnData + "");
                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + ToX + "," + ToY + "," + RoomHeightNext + ",2,2/" + OwnData + "");
                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + ToX + "," + ToY + "," + RoomHeightNext + ",2,2/" + OwnData + "");
                                            Server.Users[ID].userx = ToX;
                                            Server.Users[ID].usery = ToY;
                                        }
                                        else
                                        {
                                            while (Server.Users[ID].userx != ToX || Server.Users[ID].usery != ToY)
                                            {
                                                if (Server.Users[ID].moonwalk == 1)
                                                {
                                                    #region > ToX > ToY
                                                    while (Server.Users[ID].userx > ToX && Server.Users[ID].usery > ToY)
                                                    {
                                                        int NewX = Server.Users[ID].userx - 1;
                                                        int NewY = Server.Users[ID].usery - 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[NewY].Substring(NewX, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",1,1/mv " + NewX + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",1,1/mv " + NewX + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].userx = NewX;
                                                        Server.Users[ID].usery = NewY;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 7;
                                                        Server.Users[ID].userry = 7;
                                                    }
                                                    #endregion

                                                    #region > ToX < ToY
                                                    while (Server.Users[ID].userx > ToX && Server.Users[ID].usery < ToY)
                                                    {
                                                        int NewX = Server.Users[ID].userx - 1;
                                                        int NewY = Server.Users[ID].usery + 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[NewY].Substring(NewX, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",3,3/mv " + NewX + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",3,3/mv " + NewX + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].userx = NewX;
                                                        Server.Users[ID].usery = NewY;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 5;
                                                        Server.Users[ID].userry = 5;
                                                    }
                                                    #endregion

                                                    #region < ToX > ToY
                                                    while (Server.Users[ID].userx < ToX && Server.Users[ID].usery > ToY)
                                                    {
                                                        int NewX = Server.Users[ID].userx + 1;
                                                        int NewY = Server.Users[ID].usery - 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[NewY].Substring(NewX, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",7,7/mv " + NewX + "," + NewY + "," + RoomHeightNext + "" + OwnData + "/dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",7,7/mv " + NewX + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].userx = NewX;
                                                        Server.Users[ID].usery = NewY;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 1;
                                                        Server.Users[ID].userry = 1;
                                                    }
                                                    #endregion

                                                    #region < ToX < ToY
                                                    while (Server.Users[ID].userx < ToX && Server.Users[ID].usery < ToY)
                                                    {
                                                        int NewX = Server.Users[ID].userx + 1;
                                                        int NewY = Server.Users[ID].usery + 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[NewY].Substring(NewX, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",5,5/mv " + NewX + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",5,5/mv " + NewX + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].userx = NewX;
                                                        Server.Users[ID].usery = NewY;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 3;
                                                        Server.Users[ID].userry = 3;
                                                    }
                                                    #endregion

                                                    #region > ToX
                                                    while (Server.Users[ID].userx > ToX)
                                                    {
                                                        int NewX = Server.Users[ID].userx - 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(NewX, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",2,2/mv " + NewX + "," + Server.Users[ID].usery + "," + RoomHeightNext + "/" + OwnData + "dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",2,2/mv " + NewX + "," + Server.Users[ID].usery + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].userx = NewX;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 6;
                                                        Server.Users[ID].userry = 6;
                                                    }
                                                    #endregion

                                                    #region < ToX
                                                    while (Server.Users[ID].userx < ToX)
                                                    {
                                                        int NewX = Server.Users[ID].userx + 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(NewX, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",6,6/mv " + NewX + "," + Server.Users[ID].usery + "," + RoomHeightNext + "/" + OwnData + "dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",6,6/mv " + NewX + "," + Server.Users[ID].usery + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].userx = NewX;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 2;
                                                        Server.Users[ID].userry = 2;
                                                    }
                                                    #endregion

                                                    #region > ToY
                                                    while (Server.Users[ID].usery > ToY)
                                                    {
                                                        int NewY = Server.Users[ID].usery - 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[NewY].Substring(Server.Users[ID].userx, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",4,4/mv " + Server.Users[ID].userx + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",4,4/mv " + Server.Users[ID].userx + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].usery = NewY;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 0;
                                                        Server.Users[ID].userry = 0;
                                                    }
                                                    #endregion

                                                    #region < ToY
                                                    while (Server.Users[ID].usery < ToY)
                                                    {
                                                        int NewY = Server.Users[ID].usery + 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[NewY].Substring(Server.Users[ID].userx, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",0,0/mv " + Server.Users[ID].userx + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",0,0/mv " + Server.Users[ID].userx + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].usery = NewY;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 4;
                                                        Server.Users[ID].userry = 4;
                                                    }
                                                    #endregion

                                                    #region RX / RY
                                                    int RX = 0;
                                                    int RY = 0;
                                                    if (Server.Users[ID].userrx == 0) { RX = 4; }
                                                    if (Server.Users[ID].userrx == 1) { RX = 7; }
                                                    if (Server.Users[ID].userrx == 2) { RX = 6; }
                                                    if (Server.Users[ID].userrx == 3) { RX = 5; }
                                                    if (Server.Users[ID].userrx == 5) { RX = 3; }
                                                    if (Server.Users[ID].userrx == 6) { RX = 2; }
                                                    if (Server.Users[ID].userrx == 7) { RX = 1; }
                                                    if (Server.Users[ID].userrx == 4) { RX = 0; }

                                                    if (Server.Users[ID].userry == 0) { RY = 4; }
                                                    if (Server.Users[ID].userry == 1) { RY = 7; }
                                                    if (Server.Users[ID].userry == 2) { RY = 6; }
                                                    if (Server.Users[ID].userry == 3) { RY = 5; }
                                                    if (Server.Users[ID].userry == 5) { RY = 3; }
                                                    if (Server.Users[ID].userry == 6) { RY = 2; }
                                                    if (Server.Users[ID].userry == 7) { RY = 1; }
                                                    if (Server.Users[ID].userry == 4) { RY = 0; }
                                                    #endregion

                                                    if (Server.Users[ID].dance == 1)
                                                    {
                                                        Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + "," + RX + "," + RY + "/" + OwnData + "dance/");
                                                    }
                                                    else
                                                    {
                                                        Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + "," + RX + "," + RY + "/" + OwnData + "");
                                                    }
                                                }
                                                else
                                                {
                                                    #region > ToX > ToY
                                                    while (Server.Users[ID].userx > ToX && Server.Users[ID].usery > ToY)
                                                    {
                                                        int NewX = Server.Users[ID].userx - 1;
                                                        int NewY = Server.Users[ID].usery - 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[NewY].Substring(NewX, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",7,7/mv " + NewX + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",7,7/mv " + NewX + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].userx = NewX;
                                                        Server.Users[ID].usery = NewY;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 7;
                                                        Server.Users[ID].userry = 7;
                                                    }
                                                    #endregion

                                                    #region > ToX < ToY
                                                    while (Server.Users[ID].userx > ToX && Server.Users[ID].usery < ToY)
                                                    {
                                                        int NewX = Server.Users[ID].userx - 1;
                                                        int NewY = Server.Users[ID].usery + 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[NewY].Substring(NewX, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",5,5/mv " + NewX + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",5,5/mv " + NewX + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].userx = NewX;
                                                        Server.Users[ID].usery = NewY;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 5;
                                                        Server.Users[ID].userry = 5;
                                                    }
                                                    #endregion

                                                    #region < ToX > ToY
                                                    while (Server.Users[ID].userx < ToX && Server.Users[ID].usery > ToY)
                                                    {
                                                        int NewX = Server.Users[ID].userx + 1;
                                                        int NewY = Server.Users[ID].usery - 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[NewY].Substring(NewX, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",1,1/mv " + NewX + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",1,1/mv " + NewX + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].userx = NewX;
                                                        Server.Users[ID].usery = NewY;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 1;
                                                        Server.Users[ID].userry = 1;
                                                    }
                                                    #endregion

                                                    #region < ToX < ToY
                                                    while (Server.Users[ID].userx < ToX && Server.Users[ID].usery < ToY)
                                                    {
                                                        int NewX = Server.Users[ID].userx + 1;
                                                        int NewY = Server.Users[ID].usery + 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[NewY].Substring(NewX, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",3,3/mv " + NewX + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",3,3/mv " + NewX + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].userx = NewX;
                                                        Server.Users[ID].usery = NewY;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 3;
                                                        Server.Users[ID].userry = 3;
                                                    }
                                                    #endregion

                                                    #region > ToX
                                                    while (Server.Users[ID].userx > ToX)
                                                    {
                                                        int NewX = Server.Users[ID].userx - 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(NewX, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",6,6/mv " + NewX + "," + Server.Users[ID].usery + "," + RoomHeightNext + "/" + OwnData + "dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",6,6/mv " + NewX + "," + Server.Users[ID].usery + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].userx = NewX;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 6;
                                                        Server.Users[ID].userry = 6;
                                                    }
                                                    #endregion

                                                    #region < ToX
                                                    while (Server.Users[ID].userx < ToX)
                                                    {
                                                        int NewX = Server.Users[ID].userx + 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(NewX, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",2,2/mv " + NewX + "," + Server.Users[ID].usery + "," + RoomHeightNext + "/" + OwnData + "dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",2,2/mv " + NewX + "," + Server.Users[ID].usery + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].userx = NewX;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 2;
                                                        Server.Users[ID].userry = 2;
                                                    }
                                                    #endregion

                                                    #region > ToY
                                                    while (Server.Users[ID].usery > ToY)
                                                    {
                                                        int NewY = Server.Users[ID].usery - 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[NewY].Substring(Server.Users[ID].userx, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",0,0/mv " + Server.Users[ID].userx + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",0,0/mv " + Server.Users[ID].userx + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].usery = NewY;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 0;
                                                        Server.Users[ID].userry = 0;
                                                    }
                                                    #endregion

                                                    #region < ToY
                                                    while (Server.Users[ID].usery < ToY)
                                                    {
                                                        int NewY = Server.Users[ID].usery + 1;

                                                        string RoomHeightNow = Server.Users[ID].roomcache.Split(' ')[Server.Users[ID].usery].Substring(Server.Users[ID].userx, 1);
                                                        string RoomHeightNext = Server.Users[ID].roomcache.Split(' ')[NewY].Substring(Server.Users[ID].userx, 1);

                                                        if (Server.Users[ID].dance == 1)
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",4,4/mv " + Server.Users[ID].userx + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "dance/");
                                                        }
                                                        else
                                                        {
                                                            Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + RoomHeightNow + ",4,4/mv " + Server.Users[ID].userx + "," + NewY + "," + RoomHeightNext + "/" + OwnData + "");
                                                        }
                                                        Thread.Sleep(500);

                                                        Server.Users[ID].usery = NewY;
                                                        Server.Users[ID].userz = RoomHeightNext;
                                                        Server.Users[ID].userrx = 4;
                                                        Server.Users[ID].userry = 4;
                                                    }
                                                    #endregion

                                                    if (Server.Users[ID].dance == 1)
                                                    {
                                                        Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + "," + Server.Users[ID].userrx + "," + Server.Users[ID].userry + "/" + OwnData + "dance/");
                                                    }
                                                    else
                                                    {
                                                        Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + "," + Server.Users[ID].userrx + "," + Server.Users[ID].userry + "/" + OwnData + "");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (NullReferenceException)
                                    {

                                    }
                                    catch (IOException)
                                    {
                                        //
                                    }
                                }
                                break;
                            #endregion

                            #region GOAWAY *
                            case "GOAWAY":
                                {
                                    Send_Data_Room("STATUS " + (char)13 + Server.Users[ID].name + " 0,0,99,2,2/mod 0/");
                                    Server.Users[ID].userx = 0;
                                    Server.Users[ID].usery = 0;
                                    Server.Users[ID].dance = 0;
                                    if (Server.Users[ID].inroom != 0)
                                    {
                                        dbClient.ExecuteQuery("UPDATE users SET inroom = 0 WHERE name = '" + Server.Users[ID].name + "'");
                                        dbClient.ExecuteQuery("UPDATE rooms SET inroom = inroom - 1 WHERE id = '" + Server.Users[ID].inroom + "'");
                                        Server.Users[ID].inroom = 0;
                                        Server.Users[ID].owner = 0;
                                    }
                                    Server.Users[ID].Sck.Close();
                                }
                                break;
                            #endregion

                            #region DANCE #
                            case "DANCE":
                                {
                                    Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + ",0,2,2/dance/");
                                    Server.Users[ID].dance = 1;
                                }
                                break;
                            #endregion

                            #region STOP *
                            case "STOP":
                                {
                                    if (header[1].ToUpper() == "DANCE")
                                    {
                                        Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + ",0,2,2/");
                                        Server.Users[ID].dance = 0;
                                    }
                                }
                                break;
                            #endregion

                            #region GETORDERINFO *
                            case "GETORDERINFO":
                                {
                                    DataTable Data = null;

                                    Data = dbClient.ReadDataTable("SELECT * FROM catalogue WHERE shortname = '" + header[2] + "' LIMIT 1");

                                    if (Data != null)
                                    {
                                        foreach (DataRow Row in Data.Rows)
                                        {
                                            Send_Data("ORDERINFO" + (char)13 + (string)Row["longname"] + "/" + (int)Row["price"] + "/" + (char)13 + (int)Row["price"]);
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region GETSTRIP *
                            case "GETSTRIP":
                                {
                                    #region new
                                    if (header[1] == "new")
                                    {
                                        DataTable Data = null;
                                        string ItemInfo = "";

                                        Data = dbClient.ReadDataTable("SELECT * FROM useritems WHERE user = '" + Server.Users[ID].name + "' LIMIT 0,11");

                                        if (Data != null)
                                        {
                                            foreach (DataRow Row in Data.Rows)
                                            {
                                                ItemInfo = ItemInfo + "BLSI;" + (int)Row["id"] + ";0;S;" + (int)Row["id"] + ";" + (string)Row["textname"] + ";" + (string)Row["itemname"] + ";" + (string)Row["itemdesc"] + ";1;1;0,0,0/" + (char)13;
                                            }
                                        }

                                        Server.Users[ID].handcache = 11;
                                        Send_Data("STRIPINFO" + (char)13 + ItemInfo);
                                    }
                                    #endregion

                                    #region next
                                    if (header[1] == "next")
                                    {
                                        DataTable Data = null;
                                        string ItemInfo = "";
                                        int HandCache2 = Server.Users[ID].handcache + 11;

                                        Data = dbClient.ReadDataTable("SELECT * FROM useritems WHERE user = '" + Server.Users[ID].name + "' LIMIT " + Server.Users[ID].handcache + "," + HandCache2);


                                        if (Data != null)
                                        {
                                            foreach (DataRow Row in Data.Rows)
                                            {
                                                ItemInfo = ItemInfo + "BLSI;" + (int)Row["id"] + ";0;S;" + (int)Row["id"] + ";" + (string)Row["textname"] + ";" + (string)Row["itemname"] + ";" + (string)Row["itemdesc"] + ";1;1;0,0,0/" + (char)13;
                                            }
                                        }

                                        Server.Users[ID].handcache = HandCache2;
                                        Send_Data("STRIPINFO" + (char)13 + ItemInfo);
                                    }
                                    #endregion
                                }
                                break;
                            #endregion

                            #region PURCHASE *
                            case "PURCHASE":
                                {
                                    DataTable Data = null;
                                    string[] DataSplit = PaketData_string.Split('/');

                                    Data = dbClient.ReadDataTable("SELECT * FROM catalogue WHERE longname = '" + DataSplit[1] + "' LIMIT 1");

                                    if (Data != null)
                                    {
                                        foreach (DataRow Row in Data.Rows)
                                        {
                                            dbClient.ExecuteQuery("INSERT INTO useritems VALUES (NULL, '" + Server.Users[ID].name + "', '" + (string)Row["longname"] + "', '" + (string)Row["itemname"] + "', '" + (string)Row["itemdesc"] + "', '" + (string)Row["rgb"] + "', '" + (string)Row["size"] + "', '" + (string)Row["shortname"] + "')");
                                            dbClient.ExecuteQuery("UPDATE users SET credits = credits - " + DataSplit[2] + " WHERE name = '" + Server.Users[ID].name + "'");
                                            Server.Users[ID].credits = Server.Users[ID].credits - (int)Row["price"];

                                            Send_Data("SYSTEMBROADCAST" + (char)13 + "Buying successful !");
                                        }
                                    }
                                }
                                break;
                            #endregion

                            #region CREATEFLAT *
                            case "CREATEFLAT":
                                {
                                    string[] SplitData = PaketData_string.Split('/');

                                    dbClient.ExecuteQuery("INSERT INTO rooms VALUES (NULL, '" + SplitData[2] + "', '', '" + Server.Users[ID].name + "', 'floor1', '0', '" + SplitData[3] + "', '" + SplitData[4] + "', '', '100', '100')");
                                }
                                break;
                            #endregion

                            #region MESSENGER_REQUESTBUDDY *
                            case "MESSENGER_REQUESTBUDDY":
                                {
                                    dbClient.ExecuteQuery("INSERT INTO buddy VALUES (NULL, '" + Server.Users[ID].name + "', '" + header[1].Split('\r')[0] + "', '0')");
                                }
                                break;
                            #endregion

                            #region MESSENGER_ACCEPTBUDDY *
                            case "MESSENGER_ACCEPTBUDDY":
                                {
                                    dbClient.ExecuteQuery("UPDATE buddy SET `accept` = '1' WHERE `from` LIKE '" + header[1] + "' AND `to` = '" + Server.Users[ID].name + "' AND `accept` = '0'");

                                    DataTable Data = null;
                                    string BuddyData = "";

                                    Data = dbClient.ReadDataTable("SELECT * FROM buddy WHERE `to` LIKE '" + Server.Users[ID].name + "' AND `accept` = '1' OR `from` LIKE '" + Server.Users[ID].name + "' AND `accept` = '1'");

                                    if (Data != null)
                                    {
                                        foreach (DataRow Row in Data.Rows)
                                        {
                                            string DataName = "";
                                            if ((string)Row["from"] != Server.Users[ID].name)
                                            {
                                                DataName = (string)Row["from"];
                                            }
                                            else
                                            {
                                                DataName = (string)Row["to"];
                                            }

                                            BuddyData = BuddyData + (char)13 + "1 " + DataName + " UNKNOW" + (char)13;
                                        }
                                    }

                                    Send_Data("BUDDYLIST" + BuddyData);
                                }
                                break;
                            #endregion

                            #region MESSENGER_DECLINEBUDDY *
                            case "MESSENGER_DECLINEBUDDY":
                                {
                                    dbClient.ExecuteQuery("DELETE FROM buddy WHERE `from` LIKE '" + header[1] + "' AND `to` = '" + Server.Users[ID].name + "' AND `accept` = '0'");
                                }
                                break;
                            #endregion

                            #region MESSENGERINIT *
                            case "MESSENGERINIT":
                                {
                                    DataTable Data = null;
                                    string BuddyData = "";

                                    Data = dbClient.ReadDataTable("SELECT * FROM buddy WHERE `to` LIKE '" + Server.Users[ID].name + "' AND `accept` = '1' OR `from` LIKE '" + Server.Users[ID].name + "' AND `accept` = '1'");

                                    if (Data != null)
                                    {
                                        foreach (DataRow Row in Data.Rows)
                                        {
                                            string DataName = "";
                                            if ((string)Row["from"] != Server.Users[ID].name)
                                            {
                                                DataName = (string)Row["from"];
                                            }
                                            else
                                            {
                                                DataName = (string)Row["to"];
                                            }
                                            BuddyData = BuddyData + (char)13 + "1 " + DataName + " UNKNOW" + (char)13;
                                        }
                                    }

                                    Send_Data("BUDDYLIST" + BuddyData);
                                }
                                break;
                            #endregion

                            #region MESSENGER_REMOVEBUDDY *
                            case "MESSENGER_REMOVEBUDDY":
                                {
                                    dbClient.ExecuteQuery("DELETE FROM buddy WHERE `from` LIKE '" + header[1] + "' AND `to` = '" + Server.Users[ID].name + "' AND `accept` = '1' LIMIT 1");
                                    dbClient.ExecuteQuery("DELETE FROM buddy WHERE `to` LIKE '" + header[1] + "' AND `from` = '" + Server.Users[ID].name + "' AND `accept` = '1' LIMIT 1");

                                    DataTable Data = null;
                                    string BuddyData = "";

                                    Data = dbClient.ReadDataTable("SELECT * FROM buddy WHERE `to` LIKE '" + Server.Users[ID].name + "' AND `accept` = '1' OR `from` LIKE '" + Server.Users[ID].name + "' AND `accept` = '1'");

                                    if (Data != null)
                                    {
                                        foreach (DataRow Row in Data.Rows)
                                        {
                                            string DataName = "";
                                            if ((string)Row["from"] != Server.Users[ID].name)
                                            {
                                                DataName = (string)Row["from"];
                                            }
                                            else
                                            {
                                                DataName = (string)Row["to"];
                                            }
                                            BuddyData = BuddyData + (char)13 + "1 " + DataName + " UNKNOW" + (char)13;
                                        }
                                    }

                                    Send_Data("BUDDYLIST" + BuddyData);
                                }
                                break;
                            #endregion

                            #region PLACESTUFFFROMSTRIP *
                            case "PLACESTUFFFROMSTRIP":
                                {
                                    DataTable Data = null;
                                    DataTable RoomItemData = null;
                                    string RoomTotalData = "";

                                    Data = dbClient.ReadDataTable("SELECT * FROM useritems WHERE id = '" + header[1] + "' LIMIT 1");

                                    if (Data != null)
                                    {
                                        foreach (DataRow Row in Data.Rows)
                                        {
                                            dbClient.ExecuteQuery("INSERT INTO roomitems VALUES (NULL, '" + Server.Users[ID].inroom + "', '" + (string)Row["textname"] + "', '" + (string)Row["itemname"] + "', '" + (string)Row["itemdesc"] + "', '" + (string)Row["rgb"] + "', '" + (string)Row["size"] + "', '" + (string)Row["shortname"] + "','0' , '" + header[2] + "', '" + header[3] + "')");

                                        }
                                    }

                                    dbClient.ExecuteQuery("DELETE FROM useritems WHERE id = '" + header[1] + "'");

                                    RoomItemData = dbClient.ReadDataTable("SELECT * FROM roomitems WHERE roomid = '" + Server.Users[ID].inroom + "' ORDER BY id DESC LIMIT 1");

                                    if (RoomItemData != null)
                                    {
                                        foreach (DataRow RowXX in RoomItemData.Rows)
                                        {
                                            int zero = (int)RowXX["id"];
                                            string zstring = "00000000000";
                                            int j = 0;
                                            while (j < zero.ToString().Length)
                                            {
                                                zstring = zstring + "0";
                                                j++;
                                            }

                                            RoomTotalData = RoomTotalData + (char)13 + zstring + (int)RowXX["id"] + "," + (string)RowXX["textname"] + " " + (int)RowXX["x"] + " " + (int)RowXX["y"] + " " + (string)RowXX["size"] + " " + (int)RowXX["rotate"] + " 0.0 " + (string)RowXX["rgb"] + " /" + (string)RowXX["itemname"] + "/" + (string)RowXX["itemdesc"] + "/";
                                        }
                                    }

                                    Send_Data_Room("ACTIVE_OBJECTS" + RoomTotalData);
                                }
                                break;
                            #endregion

                            #region MOVESTUFF *
                            case "MOVESTUFF":
                                {
                                    dbClient.ExecuteQuery("UPDATE roomitems SET x = '" + header[2] + "', y = '" + header[3] + "', rotate = '" + header[4] + "' WHERE id = '" + header[1] + "' LIMIT 1");

                                    DataTable RoomItemData = null;
                                    string RoomTotalData = "";

                                    RoomItemData = dbClient.ReadDataTable("SELECT * FROM roomitems WHERE roomid = '" + Server.Users[ID].inroom + "' AND id = '" + header[1] + "'");

                                    if (RoomItemData != null)
                                    {
                                        foreach (DataRow RowXX in RoomItemData.Rows)
                                        {
                                            int zero = (int)RowXX["id"];
                                            string zstring = "00000000000";
                                            int j = 0;
                                            while (j < zero.ToString().Length)
                                            {
                                                zstring = zstring + "0";
                                                j++;
                                            }

                                            RoomTotalData = RoomTotalData + (char)13 + zstring + (int)RowXX["id"] + "," + (string)RowXX["textname"] + " " + (int)RowXX["x"] + " " + (int)RowXX["y"] + " " + (string)RowXX["size"] + " " + (int)RowXX["rotate"] + " 0.0 " + (string)RowXX["rgb"] + " /" + (string)RowXX["itemname"] + "/" + (string)RowXX["itemdesc"] + "/";
                                        }
                                    }

                                    Send_Data_Room("ACTIVEOBJECT_UPDATE" + RoomTotalData);
                                }
                                break;
                            #endregion

                            #region REMOVESTUFF *
                            case "REMOVESTUFF":
                                {
                                    DataTable RoomItemData = null;
                                    string RoomTotalData = "";

                                    RoomItemData = dbClient.ReadDataTable("SELECT * FROM roomitems WHERE roomid = '" + Server.Users[ID].inroom + "' AND id = '" + header[1] + "' LIMIT 1");

                                    if (RoomItemData != null)
                                    {
                                        foreach (DataRow RowXX in RoomItemData.Rows)
                                        {
                                            int zero = (int)RowXX["id"];
                                            string zstring = "00000000000";
                                            int j = 0;
                                            while (j < zero.ToString().Length)
                                            {
                                                zstring = zstring + "0";
                                                j++;
                                            }

                                            RoomTotalData = RoomTotalData + (char)13 + zstring + (int)RowXX["id"] + "," + (string)RowXX["textname"] + " 99 99 " + (string)RowXX["size"] + " " + (int)RowXX["rotate"] + " 0.0 " + (string)RowXX["rgb"] + " /" + (string)RowXX["itemname"] + "/" + (string)RowXX["itemdesc"] + "/";
                                        }
                                    }

                                    Send_Data_Room("ACTIVEOBJECT_UPDATE" + RoomTotalData);
                                    dbClient.ExecuteQuery("DELETE FROM roomitems WHERE id = '" + header[1] + "' LIMIT 1");
                                }
                                break;
                            #endregion

                            #region ADDSTRIPITEM *
                            case "ADDSTRIPITEM":
                                {
                                    DataTable RoomItemData = null;
                                    string RoomTotalData = "";

                                    RoomItemData = dbClient.ReadDataTable("SELECT * FROM roomitems WHERE roomid = '" + Server.Users[ID].inroom + "' AND id = '" + header[3] + "' LIMIT 1");

                                    if (RoomItemData != null)
                                    {
                                        foreach (DataRow RowXX in RoomItemData.Rows)
                                        {
                                            int zero = (int)RowXX["id"];
                                            string zstring = "00000000000";
                                            int j = 0;
                                            while (j < zero.ToString().Length)
                                            {
                                                zstring = zstring + "0";
                                                j++;
                                            }

                                            RoomTotalData = RoomTotalData + (char)13 + zstring + (int)RowXX["id"] + "," + (string)RowXX["textname"] + " 99 99 " + (string)RowXX["size"] + " " + (int)RowXX["rotate"] + " 0.0 " + (string)RowXX["rgb"] + " /" + (string)RowXX["itemname"] + "/" + (string)RowXX["itemdesc"] + "/";

                                            dbClient.ExecuteQuery("INSERT INTO useritems VALUES (NULL, '" + Server.Users[ID].name + "', '" + (string)RowXX["textname"] + "', '" + (string)RowXX["itemname"] + "', '" + (string)RowXX["itemdesc"] + "', '" + (string)RowXX["rgb"] + "', '" + (string)RowXX["size"] + "', '" + (string)RowXX["shortname"] + "')");
                                            dbClient.ExecuteQuery("DELETE FROM roomitems WHERE id = '" + header[1] + "' LIMIT 1");
                                        }
                                    }

                                    Send_Data_Room("ACTIVEOBJECT_UPDATE" + RoomTotalData);
                                }
                                break;
                            #endregion

                            #region LOOKTO *
                            case "LOOKTO":
                                {
                                    int ToX = int.Parse(header[1]);
                                    int ToY = int.Parse(header[2]);

                                    string OwnData = "";
                                    if (Server.Users[ID].owner == 1)
                                    {
                                        OwnData = "flatctrl useradmin/";
                                    }

                                    #region > ToX > ToY
                                    if (Server.Users[ID].userx > ToX && Server.Users[ID].usery > ToY)
                                    {
                                        if (Server.Users[ID].dance == 1)
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",7,7/" + OwnData + "dance/");
                                        }
                                        else
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",7,7/" + OwnData + "");
                                        }

                                        Server.Users[ID].userrx = 7;
                                        Server.Users[ID].userry = 7;
                                    }
                                    #endregion

                                    #region > ToX < ToY
                                    else if (Server.Users[ID].userx > ToX && Server.Users[ID].usery < ToY)
                                    {
                                        if (Server.Users[ID].dance == 1)
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",5,5/" + OwnData + "dance/");
                                        }
                                        else
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",5,5/" + OwnData + "");
                                        }

                                        Server.Users[ID].userrx = 5;
                                        Server.Users[ID].userry = 5;
                                    }
                                    #endregion

                                    #region < ToX > ToY
                                    else if (Server.Users[ID].userx < ToX && Server.Users[ID].usery > ToY)
                                    {
                                        if (Server.Users[ID].dance == 1)
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",1,1/" + OwnData + "dance/");
                                        }
                                        else
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",1,1/" + OwnData + "");
                                        }

                                        Server.Users[ID].userrx = 1;
                                        Server.Users[ID].userry = 1;
                                    }
                                    #endregion

                                    #region < ToX < ToY
                                    else if (Server.Users[ID].userx < ToX && Server.Users[ID].usery < ToY)
                                    {
                                        if (Server.Users[ID].dance == 1)
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",3,3/" + OwnData + "dance/");
                                        }
                                        else
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",3,3/" + OwnData + "");
                                        }

                                        Server.Users[ID].userrx = 3;
                                        Server.Users[ID].userry = 3;
                                    }
                                    #endregion

                                    #region > ToX
                                    else if (Server.Users[ID].userx > ToX)
                                    {
                                        if (Server.Users[ID].dance == 1)
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",6,6/" + OwnData + "dance/");
                                        }
                                        else
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",6,6/" + OwnData + "");
                                        }

                                        Server.Users[ID].userrx = 6;
                                        Server.Users[ID].userry = 6;
                                    }
                                    #endregion

                                    #region < ToX
                                    else if (Server.Users[ID].userx < ToX)
                                    {
                                        if (Server.Users[ID].dance == 1)
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",2,2/" + OwnData + "dance/");
                                        }
                                        else
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",2,2/" + OwnData + "");
                                        }

                                        Server.Users[ID].userrx = 2;
                                        Server.Users[ID].userry = 2;
                                    }
                                    #endregion

                                    #region > ToY
                                    else if (Server.Users[ID].usery > ToY)
                                    {
                                        if (Server.Users[ID].dance == 1)
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",0,0/" + OwnData + "dance/");
                                        }
                                        else
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",0,0/" + OwnData + "");
                                        }

                                        Server.Users[ID].userrx = 0;
                                        Server.Users[ID].userry = 0;
                                    }
                                    #endregion

                                    #region < ToY
                                    else if (Server.Users[ID].usery < ToY)
                                    {
                                        if (Server.Users[ID].dance == 1)
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",4,4/" + OwnData + "dance/");
                                        }
                                        else
                                        {
                                            Send_Data_Room("STATUS" + (char)13 + Server.Users[ID].name + " " + Server.Users[ID].userx + "," + Server.Users[ID].usery + "," + Server.Users[ID].userz + ",4,4/" + OwnData + "");
                                        }

                                        Server.Users[ID].userrx = 4;
                                        Server.Users[ID].userry = 4;
                                    }
                                    #endregion
                                }
                                break;
                            #endregion

                            #region KILLUSER *
                            case "KILLUSER":
                                {
                                    int i = 0;
                                    while (i < 150)
                                    {
                                        if (Server.Users[i] != null)
                                        {
                                            if (Server.Users[i].used != false)
                                            {
                                                if (Server.Users[i].name == header[1] && Server.Users[i].inroom == Server.Users[ID].inroom)
                                                {
                                                    System.Text.ASCIIEncoding enc2 = new System.Text.ASCIIEncoding();
                                                    byte[] tmp2 = enc2.GetBytes(PacketHelper.Build_Send("SYSTEMBROADCAST" + (char)13 + "You are kicked out of the room!").ToCharArray());
                                                    Server.Users[i].Sck.GetStream().Write(tmp2, 0, tmp2.Length);

                                                    Server.Users[i].Sck.Close();
                                                }
                                            }
                                        }
                                        i++;
                                    }
                                }
                                break;
                            #endregion

                            #region ASSIGNRIGHTS *
                            case "ASSIGNRIGHTS":
                                {
                                    dbClient.ExecuteQuery("INSERT INTO rights VALUES (NULL, '" + Server.Users[ID].inroom + "', '" + header[1] + "')");

                                    int i = 0;
                                    while (i < 150)
                                    {
                                        if (Server.Users[i] != null)
                                        {
                                            if (Server.Users[i].used != false)
                                            {
                                                if (Server.Users[i].name == header[1] && Server.Users[i].inroom == Server.Users[ID].inroom)
                                                {
                                                    System.Text.ASCIIEncoding enc2 = new System.Text.ASCIIEncoding();
                                                    byte[] tmp2 = enc2.GetBytes(PacketHelper.Build_Send("YOUARECONTROLLER").ToCharArray());
                                                    Server.Users[i].Sck.GetStream().Write(tmp2, 0, tmp2.Length);
                                                    Server.Users[i].rights = 1;
                                                }
                                            }
                                        }
                                        i++;
                                    }
                                }
                                break;
                            #endregion

                            #region REMOVERIGHTS *
                            case "REMOVERIGHTS":
                                {
                                    dbClient.ExecuteQuery("DELETE FROM rights WHERE room = '" + Server.Users[ID].inroom + "' AND user = '" + header[1] + "'");
                                }
                                break;
                            #endregion

                            #region UPDATEFLAT *
                            case "UPDATEFLAT":
                                {
                                    string[] SplitData = PaketData_string.Split('/');
                                    //UPDATE  `v1_habbo`.`rooms` SET  `name` =  'NewName', `desc` =  'NewDesc' WHERE  `rooms`.`id` =1;
                                    dbClient.ExecuteQuery("UPDATE rooms SET `name` = '" + SplitData[2] + "', `door` = '" + SplitData[3] + "' WHERE id = '" + SplitData[1] + "' LIMIT 1");
                                }
                                break;
                            #endregion

                            #region SETFLATINFO *
                            case "SETFLATINFO":
                                {
                                    string[] SplitData = PaketData_string.Split('/');
                                    string[] SplitData2 = SplitData[2].Split((char)13);
                                    dbClient.ExecuteQuery("UPDATE rooms SET `desc` = '" + SplitData2[0].Split('=')[1] + "', `pass` = '" + SplitData2[1].Split('=')[1] + "' WHERE id = '" + SplitData[1] + "'");
                                }
                                break;
                            #endregion
                        }

                        if (Server.Users[ID].name != null)
                        {
                            DataTable Data01 = null;

                            Data01 = dbClient.ReadDataTable("SELECT * FROM buddy WHERE `to` LIKE '" + Server.Users[ID].name + "' AND `accept` = '0'");

                            if (Data01 != null)
                            {
                                foreach (DataRow Row01 in Data01.Rows)
                                {
                                    Send_Data("BUDDYADDREQUESTS" + (char)13 + "/" + (string)Row01["from"]);
                                }
                            }
                        }
                        Thread.Sleep(20);
                    }
                    Thread.Sleep(20);
                }
                if (enabledebug == true) Console.WriteLine("Test, Close: " + ID);
                Send_Data_All("STATUS " + (char)13 + Server.Users[ID].name + " 0,0,99,2,2/mod 0/");
                if (Server.Users[ID].inroom != 0 && Server.Users[ID].name != null)
                {
                    dbClient.ExecuteQuery("UPDATE users SET inroom = 0 WHERE name = '" + Server.Users[ID].name + "'");
                    dbClient.ExecuteQuery("UPDATE rooms SET inroom = inroom - 1 WHERE id = '" + Server.Users[ID].inroom + "'");
                    Server.Users[ID].inroom = 0;
                    Server.Users[ID].owner = 0;
                }
                Server.Users[ID].used = false;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        
        private void Send_Data(string data)
        {
            var config = new IniFile("Server.ini");
            String debug = config.Read("enabled", "Debug");
            bool enabledebug = bool.Parse(debug);
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            byte[] tmp = enc.GetBytes(PacketHelper.Build_Send(data).ToCharArray());
            Server.Users[ID].Sck.GetStream().Write(tmp, 0, tmp.Length);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (enabledebug == true) { Console.WriteLine("<-- " + data.Replace("\r", " {13} ")); };
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void Send_Data_All(string data)
        {
            var config = new IniFile("Server.ini");
            String debug = config.Read("enabled", "Debug");
            bool enabledebug = bool.Parse(debug);
            int i = 0;
            while (i < 150)
            {
                if (Server.Users[i] != null)
                {
                    if (Server.Users[i].used != false)
                    {
                        try
                        {
                            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                            byte[] tmp = enc.GetBytes(PacketHelper.Build_Send(data).ToCharArray());
                            Server.Users[i].Sck.GetStream().Write(tmp, 0, tmp.Length);
                        }
                        catch (Exception)
                        {
                            //
                        }
                    }
                }
                i++;
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (enabledebug == true) Console.WriteLine("<-- " + data.Replace("\r", " {13} "));
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private void Send_Data_Room(string data)
        {
            var config = new IniFile("Server.ini");
            String debug = config.Read("enabled", "Debug");
            bool enabledebug = bool.Parse(debug);
            int i = 0;
            while (i < 150)
            {
                if (Server.Users[i] != null)
                {
                    if (Server.Users[i].used != false)
                    {
                        if (Server.Users[i].inroom == Server.Users[ID].inroom)
                        {
                            try
                            {
                                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                                byte[] tmp = enc.GetBytes(PacketHelper.Build_Send(data).ToCharArray());
                                Server.Users[i].Sck.GetStream().Write(tmp, 0, tmp.Length);
                            }
                            catch (Exception)
                            {
                                //
                            }
                        }
                    }
                }
                i++;
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (enabledebug == true) Console.WriteLine("<-- " + data.Replace("\r", " {13} "));
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}