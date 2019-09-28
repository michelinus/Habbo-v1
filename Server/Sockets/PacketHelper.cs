namespace ServerEngine.Sockets
{
    static class PacketHelper
    {
        public static string[] Split_Paket(string Data)
        {
            string[] tmp;
            int len = int.Parse(Data.Substring(0, 4));
            string realdata = Data.Substring(4, len);
            if (realdata.Contains(" "))
            {
                // Valid Paket
                tmp = realdata.Split(' ');
            }
            else
            {
                tmp = new string[1];
                tmp[0] = "fail";
            }
            // tmp[0] = Header
            // tmp[1] = Paket Data

            return tmp;
        }
        public static string Build_Send(string Data)
        {
            return "# " + Data + ((char)13) + "##";
        }
    }
}
