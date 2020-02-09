using System;

namespace CurrLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            //log result. Use Task Scheduler to plan daily usage
            CurrLoader cl = new CurrLoader();
            var result = cl.GetRates();
            if (result == 0)
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Error");
            }
            Console.ReadKey();
        }
    }
}
