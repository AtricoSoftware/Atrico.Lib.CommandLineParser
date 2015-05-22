using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = "123";
            Func1(ref str);
        }

        static void Func1(string str)
        {
            str = "";
        }
    }
}
