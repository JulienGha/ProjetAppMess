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
    class Topics
    {
        private static List<Topic> topics = new List<Topic>();

        static SemaphoreSlim doorman = new SemaphoreSlim(1);

        public static List<Topic> GetTopics()

        {
            return topics;
        }

        public static bool check(Topic topic)
        {
            deserializeTopic();
            foreach (Topic essai in topics)
            {  
                if (essai.Name == topic.Name)
                {
                    return true;
                }
            }
            return false;
        }
        

        public static void addTopic(Topic test)
        {
            check(test);
            test.Name.Trim();
            topics.Add(test);
            serializeTopic();
        }

        public static void serializeTopic()
        {
            doorman.Wait();
            XmlSerializer xs = new XmlSerializer(typeof(List<Topic>));
            using (StreamWriter writer = new StreamWriter("messTopics_list.xml"))
            {
                xs.Serialize(writer, topics);
            }
            doorman.Release();
        }

        public static void deserializeTopic()
        {
            doorman.Wait();
            string path = "messTopics_list.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<Topic>));
            StreamReader reader = new StreamReader(path);
            topics = (List<Topic>)serializer.Deserialize(reader);
            reader.Close();
            doorman.Release();
        }       
    }
}

