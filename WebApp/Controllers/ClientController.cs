using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ClientController : Controller
    {
        // GET: Client
        public ActionResult Index()
        {
            //Load data from client
            var clients = Client.GetClients();

            //Return the view
            return View(clients);
        }

        public ActionResult Create()
        {
            ViewBag.Submitted = false;
            var created = false;

            //Create the client
            if (HttpContext.Request.RequestType == "POST")
            {
                ViewBag.Submitted = true;

                //Since posted, get values fomr the form
                var id = Request.Form["id"];
                var name = Request.Form["name"];
                var address = Request.Form["address"];
                var trusted = (Request.Form["trusted"] == "on")? true : false;

                //Crete a new Client for these details
                var client = new Client()
                {
                    ID = Convert.ToInt32(id),
                    Name = name,
                    Address = address,
                    Trusted = trusted
                };

                //Save the client in the client list
                var clientFile = Client.ClientFile;
                var clientData = System.IO.File.ReadAllText(clientFile);

                var clientList = JsonConvert.DeserializeObject<List<Client>>(clientData) ?? new List<Client>();

                clientList.Add(client);

                //Now save the list on the disk
                System.IO.File.WriteAllText(clientFile, JsonConvert.SerializeObject(clientList));

                //Denote that the client was created
                created = true;
            }

            if (created)
                ViewBag.Message = "Client was created succesfully";
            else
                ViewBag.Message = "There was an error while creating the client";

            return View();

        }


        public ActionResult Update(int id)
        {
            if (HttpContext.Request.RequestType == "POST")
            {
                //Request is post type; must be a submit
                var name = Request.Form["name"];
                var address = Request.Form["address"];
                var trusted = Request.Form["trusted"];

                //Get all the clients
                var clients = Client.GetClients();

                foreach (var client in clients)
                {
                    //Find the client
                    if (client.ID == id)
                    {
                        //Client found, now update his properties and save it.
                        client.Name = name;
                        client.Address = address;
                        client.Trusted = Convert.ToBoolean(trusted);
                        break;
                    }
                }

                //Update clients in the disk
                System.IO.File.WriteAllText(Client.ClientFile, JsonConvert.SerializeObject(clients));

                //Add the details to the view
                Response.Redirect("~/Client/Index?Message=Client_Updated");

            }

            //Client a Model Object
            var clnt = new Client();
            var clients2 = Client.GetClients();
            //Search within the clients
            foreach (var item in clients2)
            {
                //if the client's id matches
                if (item.ID == id)
                {
                    clnt = item;
                    break;
                }
            }

            if (clnt == null)
            {
                //No Client was found
                ViewBag.Message = "No Client was found";
            }

            return View(clnt);
        }

        public ActionResult Delete(int id)
        {
            //Get the clients
            var clients = Client.GetClients();
            var deleted = false;

            //Delete specific one
            foreach (var item in clients)
            {
                //Find the client
                if (item.ID == id)
                {
                    //Delete this client
                    var index = clients.IndexOf(item);
                    clients.RemoveAt(index);

                    //Removed now save the data back
                    System.IO.File.WriteAllText(Client.ClientFile, JsonConvert.SerializeObject(clients));
                    deleted = true;
                    break;
                }
            }

            //Add the process details to the ViewBag
            if(deleted)
                ViewBag.Message = "Client was deleted succesfully";
            else
                ViewBag.Message = "There was an error while deleting the client";

            return View();

        }

    }
}