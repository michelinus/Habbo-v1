using System.Net.Sockets;

namespace ServerEngine.Manager
{
    class Usermanager
    {
        public TcpClient Sck;
        public bool used = false;

        public int credits;
        public int had_read_agreement;

        public int inroom;
        public int owner;
        public int rights;

        public int userx;
        public int usery;
        public string userz;
        public int userrx;
        public int userry;

        public int dance;

        public int handcache;
        public string roomcache;

        public int moonwalk;
        public int teleport;

        public int server;

        public string name;
        public string password;
        public string email;
        public string figure;
        public string birthday;
        public string phonenumber;
        public string customData;
        public string sex;
        public string country;
        public string has_special_rights;
        public string badge_type;
    }
}
