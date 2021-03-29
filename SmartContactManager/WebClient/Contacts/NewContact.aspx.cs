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
using WebClient.Models;
using WebClient.Models.ViewModels;

namespace WebClient.Contacts
{
    public partial class NewContact : System.Web.UI.Page
    {
        static HttpClient client = new HttpClient();
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (Session["UserID"] == null)
            {
                this.Context.Items.Add("ErrorMessage", "Please Login");
                Response.Redirect("/Account/Login.aspx?msg=Please Login..!", false);
                return;
            }
        }

        protected async void SubmitButton_Click(object sender, EventArgs e)
        {
            Contact contact = new Contact();
            contact.Email = Email.Text;
            contact.Name = Name.Text;
            contact.PhoneNumber = PhoneNumber.Text;
            contact.Description = Description.Text;
            contact.UserId = Int32.Parse(Session["UserId"].ToString());

            var serializedContact = JsonConvert.SerializeObject(contact);
            var content = new StringContent(serializedContact, Encoding.UTF8, "application/json");
            var resultContact = await client.PostAsync("https://localhost:44373/api/contacts", content);

            if (resultContact.IsSuccessStatusCode)
            {
                Contact createdContact = JsonConvert.DeserializeObject<Contact>(await resultContact.Content.ReadAsStringAsync());
                Response.Redirect("~/Contacts/ContactList.aspx");
            }
            else
            {
                if ((int)resultContact.StatusCode == 400)
                {
                    string errors;
                    errors = await Errors.getErrors(resultContact);
                    ErrorMessage.Text = errors;
                    return;
                }
                else if ((int)resultContact.StatusCode == 404)
                {
                    Response.Redirect("~/404.aspx");
                }

                string response = JsonConvert.DeserializeObject<String>(await resultContact.Content.ReadAsStringAsync());
                ErrorMessage.Visible = true;
                ErrorMessage.Text = response;
            }
        }
    }
}