using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebClient.Models;

namespace WebClient.Account
{
    public partial class Dashboard : System.Web.UI.Page
    {
        static HttpClient client = new HttpClient();
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        protected async void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserID"] == null)
            {
                this.Context.Items.Add("ErrorMessage", "Please Login");
                Response.Redirect("/Account/Login.aspx?msg=Please Login..!", false);
                return;
            }

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var UserId = Int32.Parse(Session["UserId"].ToString());
            var resultContact = await client.GetAsync("https://localhost:44373/api/contacts/user/" + UserId.ToString());
            Contact[] contacts = JsonConvert.DeserializeObject<Contact[]>(await resultContact.Content.ReadAsStringAsync());

            var resultGroup = await client.GetAsync("https://localhost:44373/api/groups/allGroups/"+UserId.ToString());
            var groups = JsonConvert.DeserializeObject<IEnumerable<Group>>(await resultGroup.Content.ReadAsStringAsync());

            var totalContacts = contacts.Length;
            var minContactsLength = (totalContacts >= 3) ? 3 : totalContacts;
            var totalGroups = groups.Count();
            var minGroupsLength = (totalGroups >= 3) ? 3 : totalGroups;

            ContactLength.Text = totalContacts.ToString();
            GroupLength.Text = totalGroups.ToString();

            for (int i = 0; i < minContactsLength; i++)
            {
                var contactUrl = "/Contacts/ViewContact.aspx?ContactId=" + contacts[i].Id;
                TableCell seqNo = new TableCell();
                TableCell contactName = new TableCell();
                TableCell contactPhoneno = new TableCell();
                TableCell viewContactButton = new TableCell();

                seqNo.Text = "" + (i + 1);
                contactName.Text = contacts[i].Name;
                contactPhoneno.Text = contacts[i].PhoneNumber;
                viewContactButton.Text = ("<a class='btn btn-primary' href=" + contactUrl + ">View</a>");
                TableRow row = new TableRow();

                row.Cells.Add(seqNo);
                row.Cells.Add(contactName);
                row.Cells.Add(contactPhoneno);
                row.Cells.Add(viewContactButton);
                ContactsList.Rows.Add(row);
            }
            int j = 0;
            foreach (var group in groups)
            {
                var groupUrl = "/Groups/ViewGroup.aspx?GroupId=" + group.Id;
                TableCell seqNo = new TableCell();
                TableCell groupName = new TableCell();
                TableCell viewGroupButton = new TableCell();

                seqNo.Text = "" + (j + 1);
                groupName.Text = group.Name;
                viewGroupButton.Text = ("<a class='btn btn-primary' href=" + groupUrl + ">View</a>");

                TableRow row = new TableRow();

                row.Cells.Add(seqNo);
                row.Cells.Add(groupName);
                row.Cells.Add(viewGroupButton);
                GroupList.Rows.Add(row);
                j++;
            }
        }
    }
}
