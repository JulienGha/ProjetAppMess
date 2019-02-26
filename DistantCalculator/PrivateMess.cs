using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Communication;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DistantCalculator
{

    [Serializable]
    class PrivateMess
    {
        private static List<privatemess> mess = new List<privatemess>();

        static SemaphoreSlim doorman = new SemaphoreSlim(1);

        public List<privatemess> getPrivateMess()
        {
            return mess;
        }

        public static List<privatemess> check(Account mine)
        {
            List<privatemess> result = new List<privatemess>();
            deserializePrivateMess();

            foreach (privatemess privatemessage in mess)
            {

                if (mine.Nom == privatemessage.To)
                {
                    result.Add(privatemessage);
                }
            }
            return result;
        }

        public static bool writemess(privatemess test, Account mine)
        {
            Account account = new Account(test.To, "", "");
            if (Users.check(account))
            {
                privatemess oula = new privatemess();
                oula.To = test.To;
                oula.From = mine.Nom;
                oula.Content = test.Content;
                addPrivateMess(oula);
                return true;
            }
            return false;
        }


        public static void addPrivateMess(privatemess message)
        {
            deserializePrivateMess();
            message.To.Trim();
            message.From.Trim();
            mess.Add(message);
            serializePrivateMess();
        }


        public static void serializePrivateMess()
        {
            doorman.Wait();
            XmlSerializer xs = new XmlSerializer(typeof(List<privatemess>));
            using (StreamWriter writer = new StreamWriter("mess_list.xml"))
            {
                xs.Serialize(writer, mess);
            }
            doorman.Release();
        }

        public static void deserializePrivateMess()
        {
            doorman.Wait();
            string path = "mess_list.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<privatemess>));
            StreamReader reader = new StreamReader(path);
            mess = (List<privatemess>)serializer.Deserialize(reader);
            reader.Close();
            doorman.Release();
        }


    }
}
