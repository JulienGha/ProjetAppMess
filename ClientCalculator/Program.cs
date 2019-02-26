using System;
using System.Collections.Generic;
using Communication;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientText


{
    class Program
    {
        static void Main(string[] args)
        {
            Client c1 = new Client("127.0.0.1", 8976);
            c1.start();
        }


    }
}
