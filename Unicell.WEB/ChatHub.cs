using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Mirante.Util;
using Newtonsoft.Json;

namespace Unicell.WEB
{
    public class ChatHub : Hub
    {
        private static List<Users> users = new List<Users>();
        public void Hello()
        {
            Clients.All.hello();
        }

        public void SendMessage(string message, string userName)
        //I have defined 2 parameters. These are the parameters to be sent here from the client software
        {
            //var sender_connectionID = Context.ConnectionId;
            Users user = users.Where(x => x.username.Equals(userName)).FirstOrDefault();
            if (user != null)
            {
                Clients.Client(user.connectionID).SendMessage(message, user.username);
            }
        }

        public List<Users> GetUsers()
        {
            return users;
        }

        public override Task OnConnected()
        {
            var connectionID = Context.ConnectionId;
            string servidor = Context.QueryString["servidor"];
            string userName = UsuarioLogado.Usuario.ID_Empresa.ToString();
            if (servidor != "1")
            {
                userName = Context.Headers["username"];
            }

            var match = users.FirstOrDefault(x => x.username == userName);
            if (match != null)
                match.connectionID = connectionID;
            else
                users.Add(new Users() {
                    username = userName,
                    connectionID = connectionID
                }); //add the connection user to the list

            string json = JsonConvert.SerializeObject(users); //send to client
            Clients.All.getUserList(json);
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            //string servidor = Context.QueryString["servidor"];
            //string userName = UsuarioLogado.Usuario.ID_Empresa.ToString();
            //if (servidor != "1")
            //{
            //    userName = Context.Headers["username"];
            //}

            //Users user = users.Where(x => x.username.Equals(userName)).FirstOrDefault();
            //if (user != null)
            //{
            //    users.Remove(user); //in the case of connection termination we removed the user from the list
            //    string json = JsonConvert.SerializeObject(users); //send to client
            //    Clients.All.getUserList(json);
            //}
            return base.OnDisconnected(stopCalled);
        }
        public class Users
        {
            public string username { get; set; }
            public string connectionID { get; set; }
        }
    }
}