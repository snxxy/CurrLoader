using System;
using System.Collections.Generic;

namespace CurrLoader
{
    class DbManipulations
    {
        public List<Currency> GetAllCurrencies()
        {
            var currencies = new List<Currency>();
            using (DatabaseEntities context = new DatabaseEntities())
            {
                foreach (var item in context.Currencies)
                {
                    currencies.Add(item);
                }
            }
            return currencies;
        }

        /// <summary>
        /// Adds passed list to database.
        /// </summary>
        /// <returns>
        /// 0 for success
        /// 1 for error
        /// </returns>
        /// <param name="rates">List of rates to add to DB</param>
        public int AddRates(List<Rate> rates)
        {
            using (DatabaseEntities context = new DatabaseEntities())
            {
                //log Database.Log. CW used for demonstration
                context.Database.Log += Console.WriteLine;
                foreach (var item in rates)
                {
                    context.Rates.Add(item);
                }
                try
                {
                    context.SaveChanges();
                    return 0;
                }
                //Throws exception with duplicate key data, but input check is not required for once/day usage case
                catch (Exception e)
                {
                    //log e
                    return 1;          
                }
            }
        }

        //For testing/demo
        /* public List<Rate> GetAllRates()
        {
            var rates = new List<Rate>();
            using (DatabaseEntities context = new DatabaseEntities())
            {
                foreach (var item in context.Rates)
                {
                    rates.Add(item);
                }
            }
            return rates;
        }*/
    }
}
