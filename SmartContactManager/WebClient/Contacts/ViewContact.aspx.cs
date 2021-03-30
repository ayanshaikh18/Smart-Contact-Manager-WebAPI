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
    public partial class ViewContact : System.Web.UI.Page
    {
        static HttpClient client = new HttpClient();
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

            int ContactId = Int32.Parse(Request.QueryString["ContactId"]);
            var UserId = Int32.Parse(Session["UserId"].ToString());

            var resultContact = await client.GetAsync("https://localhost:44373/api/contacts/"+ContactId.ToString());
            
            if(resultContact.IsSuccessStatusCode)
            {
                Contact fetchedContact = JsonConvert.DeserializeObject<Contact>(await resultContact.Content.ReadAsStringAsync());
                if (fetchedContact.UserId != UserId)
                {
                    Response.Redirect("~/AccessDenied.aspx");
                }
                Name.Text = fetchedContact.Name;
                PhoneNumber.Text = fetchedContact.PhoneNumber;
                Description.Text = fetchedContact.Description;
                Email.Text = fetchedContact.Email;
                EditContactLink.NavigateUrl = "/Contacts/EditContact.aspx?ContactId=" + ContactId;
                DeleteContactLink.NavigateUrl = "/Contacts/DeleteContact.aspx?ContactId=" + ContactId;
            }
            else
            {
                Response.Redirect("~/404.aspx");
            }
        }
    }
}