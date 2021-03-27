using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebClient.Models.ViewModels;

namespace WebClient.Account
{
    public partial class Register : System.Web.UI.Page
    {
        static HttpClient client = new HttpClient(); 
        protected void Page_Load(object sender, EventArgs e)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected async void SubmitButton_Click(object sender, EventArgs e)
        {
            RegisterUser user = new RegisterUser();
            user.Name = Name.Text;
            user.Email = Email.Text;
            user.Password = Password.Text;
            user.ConfirmPassword = ConfirmPassword.Text;
            user.PhoneNumber = PhoneNumber.Text;

            var serializeduser = JsonConvert.SerializeObject(user);
            var content = new StringContent(serializeduser, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("https://localhost:44373/api/account/register", content);
        
            // validation of input remains =--> Badrequest in Register api
            if(result.IsSuccessStatusCode)
            {
                var JsonData = JsonConvert.DeserializeObject(await result.Content.ReadAsStringAsync());
                this.Context.Items.Add("SuccessMessage", "Registered Successfully..! Please Login");
                Server.Transfer("/Login.aspx", true);
            }
        }
    }
}