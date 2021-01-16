using AvitoTestTaskServer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AvitoTestTaskClient
{
    class Program
    {
        static String URL = "https://localhost:44357";
        static void Main(string[] args)
        {
            Console.WriteLine("SET key value [ttl]");
            Console.WriteLine("GET key");
            Console.WriteLine("KEYS pattern");
            Console.WriteLine("DEL key1 key2 ...");

            while (true)
            {
                string str = Console.ReadLine();
                string[] strArr = str.Split(" ");
                if (strArr.Length == 0)
                {
                    Console.WriteLine("Input error");
                    continue;
                }
                using (HttpClient client = new HttpClient())
                {
                    switch (strArr[0])
                    {
                        case "GET":
                            {
                                if (strArr.Length != 2)
                                {
                                    Console.WriteLine("Arguments error");
                                    continue;
                                }
                                Get(strArr[1]);

                            }

                            break;

                        case "SET":
                            {
                                if (strArr.Length > 4)
                                {
                                    Console.WriteLine("Too much arguments");
                                    continue;
                                }
                                if (strArr.Length < 3)
                                {
                                    Console.WriteLine("Few arguments");
                                    continue;
                                }
                                int ttl = (strArr.Length == 4 ? int.Parse(strArr[3]) : 0);
                                Set(strArr[1], strArr[2], ttl);
                            }
                            break;

                        case "KEYS":
                            {
                                if (strArr.Length != 2)
                                {
                                    Console.WriteLine("Arguments error");
                                    continue;
                                }

                                Keys(strArr[1]);
                               
                            }
                            break;

                        case "DEL":
                            {
                                if (strArr.Length < 2)
                                {
                                    Console.WriteLine("Arguments error");
                                    continue;
                                }

                                List<string> keysList = new List<string>();
                                for (int i = 1; i < strArr.Length; i++)
                                {
                                    keysList.Add(strArr[i]);
                                }
                                Del(keysList);
                            }
                            break;

                    }
                }

            }
        }




        public static async void Set(string key, string value, int ttl)
        {

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));
                Pair pair = new Pair { key = key, value = value, ttl = ttl };
                var content = new StringContent(JsonConvert.SerializeObject(pair), Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = await client.PostAsync(URL + "/Set", content);
                Console.WriteLine(httpResponse.StatusCode);
            }
        }

        public static async void Get(string key)
        {

            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetStringAsync($"{URL}/Get?key={key}");
                Console.WriteLine(result);

            }
        }

        public static async void Keys(string pattern)
        {

            using (HttpClient client = new HttpClient())
            {
                var result = await client.GetAsync(URL + "/Keys?pattern=" + pattern);
                Console.WriteLine(await result.Content.ReadAsStringAsync());

            }
        }

        public static async void Del(List<string> keysList)
        {

            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(keysList), Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = await client.PostAsync(URL + "/Del", content);
                Console.WriteLine(await httpResponse.Content.ReadAsStringAsync());

            }
        }


    }
}

