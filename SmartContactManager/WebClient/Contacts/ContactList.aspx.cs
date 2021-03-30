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
    public partial class ContactList : System.Web.UI.Page
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
            var UserId = Int32.Parse(Session["UserId"].ToString());

            var resultContact = await client.GetAsync("https://localhost:44373/api/contacts/user/" + UserId.ToString());
            Contact[] contacts = JsonConvert.DeserializeObject<Contact[]>(await resultContact.Content.ReadAsStringAsync());

            CreateContactLink.NavigateUrl = "/Contacts/NewContact.aspx";
            for (int i = 0; i < contacts.Length; i++)
            {
                var contactUrl = "/Contacts/ViewContact.aspx?ContactId=" + contacts[i].Id;
                var editUrl = "/Contacts/EditContact.aspx?ContactId=" + contacts[i].Id;
                TableCell seqNo = new TableCell();
                TableCell contactName = new TableCell();
                TableCell contactPhoneno = new TableCell();
                TableCell viewButton = new TableCell();
                TableCell editButton = new TableCell();
                seqNo.Text = "" + (i + 1);
                contactName.Text = contacts[i].Name;
                contactPhoneno.Text = contacts[i].PhoneNumber;
                viewButton.Text = ("<a class='btn btn-primary' href=" + contactUrl + ">View</a>");
                editButton.Text = ("<a class='btn btn-secondary' href=" + editUrl + ">Edit</a>");
                TableRow row = new TableRow();
                row.Cells.Add(seqNo);
                row.Cells.Add(contactName);
                row.Cells.Add(contactPhoneno);
                row.Cells.Add(viewButton);
                row.Cells.Add(editButton);
                ContactsList.Rows.Add(row);
            }
        }
    }
}