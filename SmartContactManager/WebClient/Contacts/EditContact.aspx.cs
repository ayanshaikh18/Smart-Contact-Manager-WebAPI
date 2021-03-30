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
    public partial class EditContact : System.Web.UI.Page
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

            if (!Page.IsPostBack)
            {
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

                var resultContact = await client.GetAsync("https://localhost:44373/api/contacts/" + ContactId.ToString());
                
                if (resultContact.IsSuccessStatusCode)
                {
                    Contact fetchedContact = JsonConvert.DeserializeObject<Contact>(await resultContact.Content.ReadAsStringAsync());
                    if (fetchedContact.UserId != UserId)
                    {
                        Response.Redirect("~/AccessDenied.aspx");
                    }
                    ViewState["ContactId"] = ContactId.ToString();
                    Name.Text = fetchedContact.Name;
                    Description.Text = fetchedContact.Description;
                    Email.Text = fetchedContact.Email;
                    PhoneNumber.Text = fetchedContact.PhoneNumber;
                }
                else
                {
                    Response.Redirect("~/404.aspx");
                }
            }
        }

        protected async void SubmitButton_Click(object sender, EventArgs e)
        {
            UpdateContact contact = new UpdateContact();
            contact.Id = Int32.Parse(ViewState["ContactId"].ToString());
            contact.Name = Name.Text;
            contact.Description = Description.Text;
            contact.UserId = Int32.Parse(Session["UserId"].ToString());
            contact.PhoneNumber = PhoneNumber.Text;
            contact.Email = Email.Text;

            var serializedContact = JsonConvert.SerializeObject(contact);
            var content = new StringContent(serializedContact, Encoding.UTF8, "application/json");
            var resultContact = await client.PutAsync("https://localhost:44373/api/contacts/" + contact.Id.ToString(), content);

            if (resultContact.IsSuccessStatusCode)
            {
                Contact updatedContact = JsonConvert.DeserializeObject<Contact>(await resultContact.Content.ReadAsStringAsync());
                Response.Redirect("~/Contacts/ViewContact.aspx?ContactId=" + contact.Id);
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
                else if((int)resultContact.StatusCode == 401)
                {
                    Response.Redirect("~/AccessDenied.aspx");
                }

                string response = JsonConvert.DeserializeObject<String>(await resultContact.Content.ReadAsStringAsync());
                ErrorMessage.Visible = true;
                ErrorMessage.Text = response;
            }
        }

    }
}