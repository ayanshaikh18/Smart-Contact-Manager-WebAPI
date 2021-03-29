using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebClient.Models;
using WebClient.Models.ViewModels;

namespace WebClient.Groups
{
    public partial class ViewGroup : System.Web.UI.Page
    {
        HttpClient client = new HttpClient();
        string url = "https://localhost:44373/api/groups/";
        protected async void Page_Load(object sender, EventArgs e)
        {
            int userId = 1;
            if (Request.QueryString["GroupId"] == null)
            {
                Response.Redirect("~/404.aspx",false);
                return;
            }
            int groupId = Int32.Parse(Request.QueryString["GroupId"]);
            url += groupId;
            var result = await client.GetAsync(url);
            var statusCode = (int)result.StatusCode;
            switch (statusCode)
            {
                case (404):
                    Server.Transfer("~/404.aspx");
                    break;

                case (401):
                    Server.Transfer("~/AccessDenied.aspx");
                    break;

            }
            var group = JsonConvert.DeserializeObject<Group>(await result.Content.ReadAsStringAsync());
            if (userId != group.UserId)
                Server.Transfer("~/AccessDenied.aspx");
            GroupName.Text = group.Name;
            GrpDesc.Text = group.Description;
            AddContactLink.NavigateUrl = "AddGroupContacts.aspx?GroupId=" + groupId;
            DeleteGroupLink.NavigateUrl = "DeleteGroup.aspx?GroupId=" + groupId;

            result = await client.GetAsync("https://localhost:44373/api/groups/getGroupContacts/" + groupId);
            var grpContactViewModel = JsonConvert.DeserializeObject<IEnumerable<GroupContactViewModel>>(await result.Content.ReadAsStringAsync());
            int i = 0;
            foreach(var grpContact in grpContactViewModel)
            {
                TableCell seqNo = new TableCell();
                TableCell ContactId = new TableCell();
                TableCell PhoneNumber = new TableCell();
                TableCell button = new TableCell();
                TableCell removeButton = new TableCell();
                seqNo.Text = "" + (i + 1);
                ContactId.Text = grpContact.Contact.Name;
                PhoneNumber.Text = grpContact.Contact.PhoneNumber;
                button.Text = ("<a class='btn btn-primary' href='/ViewContact.aspx?ContactId=" + grpContact.Contact.Id.ToString() + "'>View Contact</a>");
                removeButton.Text = "<a class='btn btn-danger' href='/Groups/DeleteGroupContact.aspx?Id="+grpContact.Id + "&GroupId="+groupId+"' onclick='remove'>Remove From Group</a>";
                TableRow row = new TableRow();
                row.Cells.Add(seqNo);
                row.Cells.Add(ContactId);
                row.Cells.Add(PhoneNumber);
                row.Cells.Add(button);
                row.Cells.Add(removeButton);
                GroupContacList.Rows.Add(row);
                i++;
            }
        }
    }
}