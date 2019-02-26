using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Communication
{

    public interface Message
    {
        string ToString();
    }


    [Serializable]
    public class Choix : Message
    {
        private bool write;
        private bool read;
        public bool Write
        {
            get
            {
                return write;
            }
            set
            {
                this.write = value;
            }
        }
        public bool Read
        {
            get
            {
                return read;
            }
            set
            {
                this.read = value;
            }
        }
    }

    [Serializable]
    public class Account : Message
    {
        private string nom;
        public string Nom
        {
            get
            {
                return nom;
            }
            set
            {
                this.nom = value;
            }
        }
        private string pswd;
        public string Pswd
        {
            get
            {
                return pswd;
            }
            set
            {
                this.pswd = value;
            }
        }
        private string cho;
        

        public Account ()
        {

        }

        public Account (string nom, string pswd, string cho)
        {
            this.nom = nom;
            this.pswd = pswd;
            this.cho = cho;
        }


        public string Cho
        {
            get
            {
                return cho;
            }
            set
            {
                this.cho = value;
            }
        }

        public override string ToString()
        {
            return "your username" + nom + "your password" + pswd;
        }

    }

    [Serializable]
    public class Topic : Message
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                this.name = value;
            }
        }
        private bool priority = false;
        public bool Priority
        {
            get
            {
                return priority;
            }

            set
            {
                priority = value;
            }
        }

        public Topic()
        {

        }

    }


    [Serializable]
    public class listtopic : Message
    {
        private List<Topic> topics;
        public List<Topic> Topics
        {
            get
            {
                return topics;
            }
            set
            {
                this.topics = value;
            }
        }
        public List<Topic> getTopics()
        {
            return topics;
        }

        public listtopic()
            {

            }

    }

    [Serializable]
    public class listprivate : Message
    {
        private List<privatemess> mess;
        public List<privatemess> Mess
        {
            get
            {
                return mess;
            }
            set
            {
                this.mess = value;
            }
        }
        public void Afficher()
        {
            foreach (privatemess hehe in mess)
            {
                hehe.Affichage();

            }
                
        }
        public listprivate()
        {

        }
    }


    [Serializable]
    public class privatemess : Message
    {
        private string from;
        public string From
        {
            get
            {
                return from;
            }
            set
            {
                this.from = value;
            }
        }
        private string to;
        public string To
        {
            get
            {
                return to;
            }
            set
            {
                this.to = value;
            }
        }
        private string content;
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                this.content = value;
            }
        }

        public privatemess()
        {

        }

        public void Affichage ()
        {
            Console.WriteLine("Message from : " + from);
            Console.WriteLine("Content : " + content);
        }
    }


    [Serializable]
    public class topicmess : Message
    {

        private string from;
        public string From
        {
            get
            {
                return from;
            }
            set
            {
                this.from = value;
            }
        }
        private string to;
        public string To
        {
            get
            {
                return to;
            }
            set
            {
                this.to = value;
            }
        }
        private string content;
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                this.content = value;
            }
        }

        public topicmess()
        {

        }
        public void Affichage()
        {
            Console.WriteLine(from + ":");
            Console.WriteLine(content);
        }
    }

    [Serializable]
    public class listtopicmess : Message
    {
        public bool empty
        {
            get;
            set;
        }
        private List<topicmess> mess;
        public List<topicmess> Mess
        {
            get
            {
                return mess;
            }
            set
            {
                this.mess = value;
            }
        }
        public void Afficher()
        {
            foreach (topicmess hehe in mess)
            {
                hehe.Affichage();

            }

        }
        public listtopicmess()
        {

        }
        public listtopicmess(bool exist)
        {
            empty = !exist;
        }
    }


    [Serializable]
    public class info : Message
    {
        private string A;
        private bool Success;

        public info()
        {

        }

        public info(string a, bool success)
        {
            A = a;
            Success = success;
        }

        public string getA()
        {
            return A;
        }

        public bool getSuccess()
        {
            return Success;
        }

        public void setSuccess(bool value)
        {
            Success = value;
        }
    }

}
