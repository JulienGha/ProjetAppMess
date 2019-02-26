using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Communication;

namespace ClientText
{
    class Client
    {
        private string hostname;
        private int port;
        private TcpClient comm;


        public Client(string h, int p)
        {
            hostname = h;
            port = p;
        }

        public void start()
        {
            info mess = new info("", false);
            Account account = new Account("","","");
            do
            {
                
                comm = new TcpClient(hostname, port);
                Console.WriteLine("Welcome");
                Console.WriteLine("What do you want to do ?");
                Console.WriteLine("1) Login");
                Console.WriteLine("2) Create an account");
                Console.WriteLine("3) Exit");
                string choice;
                
                do
                {
                    Console.WriteLine("Choose 1 or 2 please");
                    choice = Console.ReadLine();
                }
                while (choice != "1" && choice != "2" && choice != "3");

                if (choice == "1")
                {
                    account = CreateAccount("1");
                }

                else if (choice == "2")
                {
                    account = CreateAccount("2");

                }

                else Environment.Exit(0);
                Console.Clear();
                mess = (info)Net.rcvMsg(comm.GetStream());
                Console.WriteLine(mess.getA());
            } while (!mess.getSuccess());

            logged(account);


        }

        public Account CreateAccount(string n)
        {
            string nom;
            string pswd;
            string pswd2;
            Console.WriteLine("Enter your username");
            nom = Console.ReadLine();
            Console.WriteLine("Enter your password");
            pswd = Console.ReadLine();
            if (n == "2")
            {
                Console.WriteLine("Confirm your password");
                pswd2 = Console.ReadLine();
                while (pswd != pswd2)
                {
                    Console.WriteLine("Passwords don't match, please enter again");
                    pswd = Console.ReadLine();
                    Console.WriteLine("Confirm it");
                    pswd2 = Console.ReadLine();
                }

            }

            Account account = new Account(nom, pswd, n);

            Net.sendMsg(comm.GetStream(), account);

            return account;

        }

        public void logged (Account account)
        {

            bool sortie = false;
            string choix;
            listprivate list1 = new listprivate();
            Choix haha = new Choix();
            Console.WriteLine("Hello " + account.Nom);
            do
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("Where do you want to go ? ");
                    Console.WriteLine("1) Topic list");
                    Console.WriteLine("2) Message Box");
                    Console.WriteLine("3) Log out");
                    choix = Console.ReadLine();
                } while (choix != "1" && choix != "2" && choix != "3");

                if (choix == "1")

                {
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("What do you want to do");
                        Console.WriteLine("1) Join");
                        Console.WriteLine("2) Add a topic");
                        Console.WriteLine("3) Return");
                        choix = Console.ReadLine();
                        if (choix == "1")
                        {
                            topicmess hoho = new topicmess();
                            
                            Console.Clear();
                            haha.Read = true;
                            haha.Write = false;
                            Net.sendMsg(comm.GetStream(), haha);
                            listtopic print = new listtopic();
                            print = (listtopic)Net.rcvMsg(comm.GetStream());

                            foreach (Topic topic in print.getTopics())
                            {
                                Console.WriteLine(topic.Name);
                            }
                            Topic hahi = new Topic();
                            
                            Console.WriteLine("Which topic");
                            hahi.Name = Console.ReadLine();
                            Net.sendMsg(comm.GetStream(), hahi);
                            
                            
                            do
                            {
                                Console.Clear();
                                Console.WriteLine("type /quit to leave");
                                Console.WriteLine("press enter to actualise");
                                listtopicmess suite = (listtopicmess)Net.rcvMsg(comm.GetStream());
                            
                                                               
                            
                            suite.Afficher();
                                
                            hoho.From = account.Nom;
                            hoho.To = hahi.Name;
                            
                                Console.WriteLine(account.Nom + ":");
                                hoho.Content = Console.ReadLine();
                                Net.sendMsg(comm.GetStream(), hoho);
                            } while (hoho.Content != "/quit");
                        }


                        else if (choix == "2")
                        {
                            haha.Read = true;
                            haha.Write = true;
                            Net.sendMsg(comm.GetStream(), haha);
                            Console.WriteLine("How do you want to call it");
                            Topic added = new Topic();
                            added.Name = Console.ReadLine();
                            Net.sendMsg(comm.GetStream(), added);

                        }


                    } while (choix != "1" && choix != "2" && choix != "3");
                    


                }

                else if (choix == "2")
                {
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("What do you want to do");
                        Console.WriteLine("1) Write a message");
                        Console.WriteLine("2) Check your messages");
                        Console.WriteLine("3) Return");
                        choix = Console.ReadLine();
                        if (choix == "1")
                        {
                            haha.Read = false;
                            haha.Write = true;
                            Net.sendMsg(comm.GetStream(), haha);
                            privatemess oula = new privatemess();
                            Console.WriteLine("To who :");
                            oula.To = Console.ReadLine();
                            Console.WriteLine("Content : ");
                            oula.Content = Console.ReadLine();
                            oula.From = account.Nom;
                            Net.sendMsg(comm.GetStream(), oula);


                        }
                        else if (choix == "2")
                        {
                            haha.Read = false;
                            haha.Write = false;
                            string leave =  "";
                            Net.sendMsg(comm.GetStream(), haha);
                            Console.WriteLine("Type /quit to leave");
                            
                            list1 = (listprivate)Net.rcvMsg(comm.GetStream());
                            list1.Afficher();
                            do
                            {
                                leave = Console.ReadLine();
                            } while (leave != "/quit");
                        }


                    } while (choix != "1" && choix != "2" && choix != "3");


                }

                else if (choix == "3")
                {
                    sortie = true;
                    
                }
                


            } while (!sortie);

        }
      
    }
}
