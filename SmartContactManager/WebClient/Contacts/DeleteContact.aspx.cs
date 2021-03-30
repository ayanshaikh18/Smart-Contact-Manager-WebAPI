using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebClient.Models;

namespace WebClient.Contacts
{
    public partial class DeleteContact : System.Web.UI.Page
    {
        static HttpClient client = new HttpClient();
        int UserId, ContactId;

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        protected async void Page_Load(object sender, EventArgs e)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (Session["UserID"] == null)
            {
                this.Context.Items.Add("ErrorMessage", "Please Login");
                Response.Redirect("/Account/Login.aspx?msg=Please Login..!", false);
                return;
            }
            if (Request.QueryString["ContactId"] == null)
            {
                Response.Redirect("~/404.aspx");
            }

            ContactId = Int32.Parse(Request.QueryString["ContactId"]);
            UserId = Int32.Parse(Session["UserId"].ToString());

            var resultContact = await client.GetAsync("https://localhost:44373/api/contacts/"+ ContactId.ToString());

            if (resultContact.IsSuccessStatusCode)
            {
                Contact fetchedContact = JsonConvert.DeserializeObject<Contact>(await resultContact.Content.ReadAsStringAsync());
                if (fetchedContact.UserId != UserId)
                {
                    Response.Redirect("~/AccessDenied.aspx");
                }
                ContactData.Text = "Name :- " + fetchedContact.Name +
                "<br>Phone number :- " + fetchedContact.PhoneNumber +
                "<br>Email :- " + fetchedContact.Email +
                "<br>Description :- " + fetchedContact.Description;
            }
            else
            {
                Response.Redirect("~/404.aspx");
            }
        }

        protected async void SubmitButton_Click(object sender, EventArgs e)
        {

            var resultContact = await client.DeleteAsync("https://localhost:44373/api/contacts/"+ ContactId.ToString());
             
            if(resultContact.IsSuccessStatusCode)
            {
                Contact fetchedContact = JsonConvert.DeserializeObject<Contact>(await resultContact.Content.ReadAsStringAsync());
                Response.Redirect("~/Contacts/ContactList.aspx");
            }
            else
            {
                Response.Redirect("~/404.aspx");
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Contacts/ViewContact.aspx?ContactId=" + ContactId);
        }
    }
}