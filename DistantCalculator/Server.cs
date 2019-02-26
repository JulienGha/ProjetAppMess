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
using DistantCalculator;

namespace Distant
{
    public class Server
    {
        private int port;

        public Server(int port)
        {
            this.port = port; 
        } 

        public void start()
        {
            TcpListener l = new TcpListener(new IPAddress(new byte[] { 127, 0, 0, 1 }), port);
            l.Start();

            while (true)
            {
                TcpClient comm = l.AcceptTcpClient();
                Console.WriteLine("Connection established @" + comm);
                new Thread(new Receiver(comm).doOperation).Start();

            }


             
        }

        class Receiver
        {
            private TcpClient comm;

            info sucl = new info("Login sucessfull", true);
            info succ = new info("Your account have been created", true);
            info erl = new info("Login failed", false);
            info erc = new info("This account name already exist", false);

            public Receiver(TcpClient s)
            {
                comm = s;
            }

            public void doOperation()
            {


                Account account = (Account) Net.rcvMsg(comm.GetStream());
                

                if (account.Cho == "1")  
                { 
                    if (Users.check(account))
                    {
                        if(Users.checkPswd(account))
                        {
                            Net.sendMsg(comm.GetStream(), sucl);
                        }
                        else
                        {
                            Net.sendMsg(comm.GetStream(), erl);
                        }
                    }
                    else Net.sendMsg(comm.GetStream(), erl);

                }
                else if (account.Cho == "2")
                {
                    
                    if (!Users.check(account))
                    {
                        Users.addAccount(account);
                        Net.sendMsg(comm.GetStream(), succ);
                    }
                    else Net.sendMsg(comm.GetStream(), erc);


                }
                Choix choix = new Choix();
                privatemess message1 = new privatemess();
                info write = new info();
                info read = new info();
                Topic topa = new Topic();
                do
                {
                    choix = (Choix)Net.rcvMsg(comm.GetStream());
                    if (choix.Read == true && choix.Write == false)

                    {
                        topicmess newmess = new topicmess();
                        

                        listtopic sended = new listtopic(); 

                        Topics.deserializeTopic();
                        
                        sended.Topics = Topics.GetTopics();
                        Net.sendMsg(comm.GetStream(), sended);
                        Topic topi = new Topic();
                        topi = (Topic)Net.rcvMsg(comm.GetStream());                        
                        listtopicmess end = new listtopicmess();
                        do
                        {
                            end.Mess = MessageTopic.getlistmess(topi);
                        
                        if (end.Mess != null)
                        {
                            Net.sendMsg(comm.GetStream(), end);
                            MessageTopic.deserializeTopicMess();
                        }
                        else
                        {

                            Net.sendMsg(comm.GetStream(), new listtopicmess(false));
                        }
                            
                        
                            newmess = (topicmess)Net.rcvMsg(comm.GetStream());
                            topi.Name = newmess.To;
                        
                            if ((newmess.Content != "/quit") && newmess.Content != "")
                            {
                                MessageTopic.writemess(newmess, topi);
                            }


                        } while (newmess.Content != "/quit") ;

                    }

                    else if (choix.Read == true && choix.Write == true)
                    {
                        
                        Topics.deserializeTopic();
                        Topic adding = (Topic)Net.rcvMsg(comm.GetStream());
                        adding.Priority = false;
                        Topics.addTopic(adding);
                        MessageTopic.deserializeTopicMess();
                        topicmess alpha = new topicmess();
                        alpha.Content = "Welcome";
                        alpha.To = adding.Name;
                        alpha.From = "admin";
                        MessageTopic.addTopicMess(alpha);


                    }

                    else if (choix.Read == false && choix.Write == true)
                    {
                        message1 = (privatemess)Net.rcvMsg(comm.GetStream());
                        PrivateMess.writemess(message1, account);
                    }

                    else if (choix.Read == false && choix.Write == false)
                    {
                        listprivate themessage = new listprivate();
                        themessage.Mess = PrivateMess.check(account);
                        Net.sendMsg(comm.GetStream(), themessage);
                    }

                } while (true);
            }
        }


        



    }


}

