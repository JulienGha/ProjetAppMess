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
    class MessageTopic
    {
        private static List<topicmess> messT = new List<topicmess>();

        static SemaphoreSlim doorman = new SemaphoreSlim(1);

        public List<topicmess> getPrivateMess()
        {
            return messT;
        }

        public static bool check(Topic topic)
        {
            
            deserializeTopicMess();
            foreach (topicmess privatemessage in messT)
            {

                if (topic.Name == privatemessage.To)
                {
                    return true;
                }
            }
            return false;
        }
        public static List<topicmess> getlistmess(Topic topic)
        {
            List<topicmess> result = new List<topicmess>();
            deserializeTopicMess();

            foreach (topicmess topicmessage in messT)
            {

                if (topic.Name == topicmessage.To)
                {
                    result.Add(topicmessage);
                }
            }
            return result;
        }

        public static void writemess(topicmess message1, Topic topic)
        {
            if (Topics.check(topic))
            {
                if (topic.Priority == true)
                {
                    topicmess oul = new topicmess();
                    oul.To = message1.To;
                    oul.From = topic.Name;
                    oul.Content = message1.Content;
                    foreach (Topic topi in Topics.GetTopics())

                    {
                        oul.To = topi.Name;
                        addTopicMess(oul);
                    }
                }
                else
                {
                    topicmess oula = new topicmess();
                    oula.To = message1.To;
                    oula.From = message1.From;
                    oula.Content = message1.Content;
                    addTopicMess(oula);
                    
                }
            }
        }


        public static void addTopicMess(topicmess message)
        {
            
            message.To.Trim();
            message.From.Trim();
            messT.Add(message);
            serializeTopicMess();
        }


        public static void serializeTopicMess()
        {
            doorman.Wait();
            XmlSerializer xs = new XmlSerializer(typeof(List<topicmess>));
            using (StreamWriter writer = new StreamWriter("messTop_list.xml"))
            {
                xs.Serialize(writer, messT);
            }
            doorman.Release();

        }

        public static void deserializeTopicMess()
        {
            doorman.Wait();
            string path = "messTop_list.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<topicmess>));
            StreamReader reader = new StreamReader(path);
            messT = (List<topicmess>)serializer.Deserialize(reader);
            reader.Close();
            doorman.Release();
        }


    }
}