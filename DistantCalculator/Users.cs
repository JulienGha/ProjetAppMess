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
    class Users
    {
        private static List<Account> Accounts = new List<Account>();

        static SemaphoreSlim doorman = new SemaphoreSlim(1);


        public List<Account> getUsers()
        {
            return Accounts;
        }

        public static bool check (Account mine)
        {

            deserializeAccount();
            foreach (Account account in Accounts)
            {
                
                if (mine.Nom == account.Nom)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool checkPswd(Account mine)
        {
            
            foreach (Account account in Accounts)
            {
                if (mine.Pswd == account.Pswd)
                {
                    return true;
                }
            }
            return false;
        }

        public static void addAccount(Account account)
        {
            account.Nom.Trim();
            account.Pswd.Trim();
            Accounts.Add(account);
            serializeAccount();
        }


        public static void serializeAccount()
        {
            doorman.Wait();
            XmlSerializer xs = new XmlSerializer(typeof(List<Account>));
            using (StreamWriter writer = new StreamWriter("log_list.xml"))
            {
                xs.Serialize(writer, Accounts);
            }
            doorman.Release();
        }

        public static void deserializeAccount()
        {
            doorman.Wait();
            string path = "log_list.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<Account>));
            StreamReader reader = new StreamReader(path);
            Accounts = (List<Account>)serializer.Deserialize(reader);
            reader.Close();
            doorman.Release();
        }


    }
}
