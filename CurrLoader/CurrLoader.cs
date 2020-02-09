using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace CurrLoader
{
    class CurrLoader
    {
        /// <summary>
        /// 1) Gets the list of valid currencies
        /// 2) Gets XML for DateTime.Now
        /// 3) Gets XML elements by ID attribute and saves data to Rate object
        /// 4) Forms and adds list of rates to localDb
        /// </summary>
        /// <returns>
        /// 0 for success
        /// 1 for error
        /// </returns>
        /// <param name="addedDays">Optional parameter for testing. Gets rates for addedDays days before DateTime.Now</param>
        public int GetRates(int addedDays = 0)
        {
            var currentDate = DateTime.Now.AddDays(-addedDays).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            Console.WriteLine(currentDate);
            var xml = GetXml(currentDate);
            if (xml == null)
            {
                //log null xml
                return 1;
            }

            var toSave = new List<Rate>();
            var dbMan = new DbManipulations();
            var currs = dbMan.GetAllCurrencies();

            foreach (var a in currs)
            {
                var query = from element in xml.Elements("Valute")
                            where (string)element.Attribute("ID") == a.CurrencyCode
                            select element;
                foreach (var item in query)
                {
                    var rateToAdd = new Rate();
                    rateToAdd.Currency = a.Currency1;
                    rateToAdd.Date = item.Parent.Attribute("Date").Value; //getting date from XML instead of currentDate as dates may vary depending on XML update time.
                    rateToAdd.Rate1 = Convert.ToDecimal(item.Element("Value").Value, new CultureInfo("ru-RU"));
                    toSave.Add(rateToAdd);
                }
            }

            if (toSave.Count > 0)
            {
                var result = dbMan.AddRates(toSave);
                if (result == 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                //log nothing to save
                return 1;
            }
        }


        /// <summary>
        /// Get XML rates file in XElement object by date in dd.MM.yyyy format
        /// </summary>
        /// <param name="date">Shortdate in dd.MM.yyyy</param>
        /// <returns>
        /// Returns null for invalid date format and WebException
        /// </returns>
        private XElement GetXml(string date)
        {
            //check date
            XElement xml = null;
            try
            {
                //May throw "Too many automatic redirections were attempted" WebException 
                xml = XElement.Load("http://www.cbr.ru/scripts/XML_daily.asp?date_req=" + date);
            }
            catch (System.Net.WebException e)
            {
                //log e
                Console.WriteLine(e.Message);
            }
            return xml;
        }


        /// <summary>
        /// Gets rate data for multiple days before DateTime.Now. For testing
        /// </summary>
        /*public void GetMultipleDayRates(int days)
        {
            for (int i = 0; i < days; i++)
            {
                var result = GetRates(i);
            }
        }*/
    }
}
