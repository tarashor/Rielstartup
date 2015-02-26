using Ninject;
using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Parser.Helper;
using RielAp.Parser.Loader;
using RielAp.Parser.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using RielAp.Advertising.TurboSMSService;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using RielAp.Core;

namespace RielAp.Advertising
{
    class Program
    {
        public static string filename = "agents2.xml";
        public static string adsfilename = "ads.xml";
        public static int numbersCountInVashmagazin = 10;

        public static void LoadPhones()
        {
            IPhonesParser parser = new VashmagazinParser(new VashmagazinHttpClientLoader(), new ConsoleLoger());
            IDictionary<VashmagazinPhone, int> rawAgents = parser.GetPhones();

            List<Agent> agents = ParseAgents(rawAgents).OrderByDescending(a => a.AnnouncementsAmount).ToList();
            SaveAgents(agents);

        }

        public static List<Agent> ParseAgents(IDictionary<VashmagazinPhone, int> map)
        {
            List<Agent> agents = new List<Agent>();
            foreach (VashmagazinPhone phone in map.Keys)
            {
                //Agent agent = agents.Where(a => a.Phones. == phone.Phone).FirstOrDefault();
                Agent agent = new Agent();
                agent.Phones = ParsePhones(phone.Phone);
                agent.AnnouncementsAmount = map[phone];
                agent.Rubric = phone.Rubric;
                agents.Add(agent);
            }
            return agents;
        }

        public static List<Phone> ParsePhones(string phoneNumberRaw)
        {
            List<Phone> phones = new List<Phone>();
            MatchCollection mc = Regex.Matches(phoneNumberRaw, @"(\(\d{3}\)\s\d{3}\-\d{4})");
            for (int i = 0; i < mc.Count; i++)
            {
                string phoneNumber = StringHelper.GetOnlyNumbers(mc[i].Value);
                Phone phone = StringToPhone(phoneNumber);
                if (phone != null)
                {
                    phones.Add(phone);
                }
            }
            return phones;
        }

        public static Phone StringToPhone(string phoneNumber) {
            Phone phone = null;
            string onlyNumbers = StringHelper.GetOnlyNumbers(phoneNumber);
            if (!string.IsNullOrEmpty(onlyNumbers) && (onlyNumbers.Length == numbersCountInVashmagazin))
            {
                phone = new Phone();
                phone.RegionCode = onlyNumbers.Substring(0, 3);
                phone.Number = onlyNumbers.Substring(3);
            }
            return phone;
        }


        public static void SaveAgents(List<Agent> agents)
        {
            var serializer = new XmlSerializer(typeof(List<Agent>));
            using (var stream = File.Create(filename))
            {
                serializer.Serialize(stream, agents);
            }
        }

        public static List<Agent> GetAgents()
        {
            List<Agent> agents = null;
            var serializer = new XmlSerializer(typeof(List<Agent>));
            using (var stream = File.Open(filename, FileMode.Open))
            {
                agents = (List<Agent>)serializer.Deserialize(stream);
            }
            return agents;
        }

        public static void SaveAds(List<string> ads)
        {
            var serializer = new XmlSerializer(typeof(List<string>));
            using (var stream = File.Create(adsfilename))
            {
                serializer.Serialize(stream, ads);
            }
        }

        public static List<string> GetAds()
        {
            List<string> ads = null;
            var serializer = new XmlSerializer(typeof(List<string>));
            using (var stream = File.Open(adsfilename, FileMode.Open))
            {
                ads = (List<string>)serializer.Deserialize(stream);
            }
            return ads;
        }

        public static bool sendSMS(string phone, Rubrics? rubric)
        {
            TurboSMSService.ServiceSoapClient service = new TurboSMSService.ServiceSoapClient("ServiceSoap");
            var auth = service.Auth("taras", "TARAS1990");

            string endMessage = "продажу нерухомості";
            if (rubric.HasValue)
            {
                switch (rubric.Value)
                {
                    case Rubrics.Apartments:
                        endMessage = "продажу квартир";
                        break;
                    case Rubrics.Lands:
                        endMessage = "продажу ділянок";
                        break;
                    case Rubrics.Houses:
                        endMessage = "продажу будинків";
                        break;
                    case Rubrics.Rent:
                        endMessage = "оренди нерухомості";
                        break;
                }
            }

            string baseMessage = "Розміщуйте оголошення нерухомості на новому сайті http://aparts.in.ua";

            string message = baseMessage;//string.Format(baseMessage, endMessage);

            ArrayOfString res = service.SendSMS("apartsinua", phone, message, "");

            return (res[0] == "Сообщения успешно отправлены");

            //return true;

        }

        private class KeyValuePair : IComparable
        {
            public KeyValuePair() {
                Rubrics = new List<Rubrics>();
            }

            public string Key { get; set; }
            public int Value { get; set; }
            public ICollection<Rubrics> Rubrics { get; set; }

            public int CompareTo(object obj)
            {
                int res = -1;
                KeyValuePair pair = obj as KeyValuePair;
                if (pair != null)
                {
                /*    res = this.Key.CompareTo(pair.Key);
                    if (res == 0)
                    {*/
                        if (this.Value < pair.Value)
                        {
                            res = -1;
                        }
                        else
                        {
                            if (this.Value > pair.Value)
                            {
                                res = 1;
                            }
                            else
                            {
                                if (this.Value == pair.Value)
                                {
                                    res = 0;
                                }
                            }
                        }
                    /*}*/
                }
                return res*(-1);
            }
        }

        public static void SendAds()
        {
            List<Agent> agents = GetAgents();
            List<KeyValuePair> pairs = new List<KeyValuePair>();
            foreach (Agent agent in agents)
            {
                foreach (Phone phone in agent.Phones)
                {
                    if (phone.IsValid())
                    {
                        string number = phone.GetRawNumbers();
                        KeyValuePair pair = pairs.Where(p => p.Key == number).FirstOrDefault();
                        if (pair != null)
                        {
                            pair.Value++;
                            if (!pair.Rubrics.Contains(agent.Rubric))
                            {
                                pair.Rubrics.Add(agent.Rubric);
                            }
                        }
                        else
                        {
                            pair = new KeyValuePair();
                            pair.Key = number;
                            pair.Value = 1;
                            pair.Rubrics.Add(agent.Rubric);
                            pairs.Add(pair);
                        }
                    }
                }
            }

            pairs.Sort();
            Console.WriteLine(pairs.Count);

            List<string> sendAd = GetAds();

            foreach (KeyValuePair pair in pairs)
            {
                if (sendAd.Count >= 1000) break;
                string number = pair.Key;

                if (!sendAd.Contains(number))
                {
                    sendAd.Add(number);
                    if(pair.Rubrics.Count > 1){
                        sendSMS(number, null);
                    }
                    else{
                        sendSMS(number, pair.Rubrics.FirstOrDefault());
                    }
                }
            }

            SaveAds(sendAd);

            Console.WriteLine(sendAd.Count);
        }

        public static void Main(string[] args)
        {
            //LoadPhones();
            SendAds();
            //sendSMS("+380974482472", Rubrics.Rent);
            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        
    }
}
