using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebClient.Models;
using WebClient.Models.ViewModels;

namespace WebClient.Groups
{
    public partial class AddGroupContacts : System.Web.UI.Page
    {
        HttpClient client = new HttpClient();
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                this.Context.Items.Add("ErrorMessage", "Please Login");
                Response.Redirect("/Account/Login.aspx?msg=Please Login..!", false);
                return;
            }
            var userId = Int32.Parse(Session["UserId"].ToString());
            if (Request.QueryString["GroupId"] == null)
            {
                Response.Redirect("~/404.aspx", false);
                return;
            }
            int groupId = Int32.Parse(Request.QueryString["GroupId"]);

            var result = await client.GetAsync("https://localhost:44373/api/contacts/user/" + userId.ToString());
            Contact[] allContacts = JsonConvert.DeserializeObject<Contact[]>(await result.Content.ReadAsStringAsync());
            var allContactsSet =allContacts.ToHashSet();

            result = await client.GetAsync("https://localhost:44373/api/groups/getGroupContacts/" + groupId);
            var grpContactViewModel = JsonConvert.DeserializeObject<IEnumerable<GroupContactViewModel>>(await result.Content.ReadAsStringAsync());
            var grpContactsSet = new HashSet<Contact>();
            foreach (var grpContact in grpContactViewModel)
                grpContactsSet.Add(grpContact.Contact);

            allContactsSet.ExceptWith(grpContactsSet);

            foreach(var contact in allContactsSet)
            {
                var contactToAdd = new ListItem();
                contactToAdd.Value = contact.Id.ToString();
                contactToAdd.Text = contact.Name;
                GroupContacts.Items.Add(contactToAdd);
            }
        }

        protected async void SubmitButton_Click(object sender, EventArgs e)
        {
            AddGroupContactsViewModel model = new AddGroupContactsViewModel();
            model.GroupId = Int32.Parse(Request.QueryString["GroupId"]);
            var contactIds = new List<int>();
            foreach (ListItem grpContact in GroupContacts.Items)
            {
                if (grpContact.Selected)
                    contactIds.Add(Int32.Parse(grpContact.Value));
            }
            model.ContactIds = contactIds.ToArray();

            var serializedModel = JsonConvert.SerializeObject(model);
            var content = new StringContent(serializedModel, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("https://localhost:44373/api/groups/addGroupContacts", content);
            var statusCode = (int)result.StatusCode;
            switch (statusCode)
            {
                case (404):
                    Server.Transfer("~/404.aspx");
                    break;

                case (401):
                    Server.Transfer("~/AccessDenied.aspx");
                    break;

                case (400):
                    ErrorMessage.Text = "Something Went Wrong";
                    return;
            }
            Response.Redirect("~/Groups/ViewGroup.aspx?GroupId="+model.GroupId);

        }
    }
}