using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace WebApp.Models
{
    public class Client
    {
        public static string ClientFile = HttpContext.Current.Server.MapPath("~/App_Data/Clients.json");

        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool Trusted { get; set; }

        public static List<Client> GetClients()
        {
            var clients = new List<Client>();
            if (File.Exists(ClientFile))
            {
                string content = File.ReadAllText(ClientFile);

                //Deserialize the object
                clients = JsonConvert.DeserializeObject<List<Client>>(content);

                //retunr the list, either empty or containning clients
                return clients;
            }
            else
            {
                //Create the file
                File.Create(ClientFile).Close();
                //Write Data to it; [] means an array
                File.WriteAllText(ClientFile, "[]");

                //Re run the function
                GetClients();
            }

            return clients;
        }


    }
}