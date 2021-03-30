
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

namespace WebClient.Groups
{
    public partial class GroupList : System.Web.UI.Page
    {
        HttpClient client = new HttpClient();
        string url = "https://localhost:44373/api/groups/allGroups/";
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                this.Context.Items.Add("ErrorMessage", "Please Login");
                Response.Redirect("/Account/Login.aspx?msg=Please Login..!", false);
                return;
            }
            var userId = Int32.Parse(Session["UserId"].ToString());
            url += userId;
            var result = await client.GetAsync(url);
            if (result.IsSuccessStatusCode)
            {
                var groups = JsonConvert.DeserializeObject<IEnumerable<WebClient.Models.Group>>(await result.Content.ReadAsStringAsync());
                int i = 0;
                foreach(var group in groups)
                {
                    var grpUrl = "ViewGroup.aspx?GroupId=" + group.Id;
                    var editUrl = "EditGroup.aspx?GroupId=" + group.Id;
                    TableCell seqNo = new TableCell();
                    TableCell grpName = new TableCell();
                    TableCell grpDesc = new TableCell();
                    TableCell button = new TableCell();
                    TableCell editButton = new TableCell();
                    seqNo.Text = "" + (i + 1);
                    grpName.Text = group.Name;
                    grpDesc.Text = group.Description;
                    button.Text = ("<a class='btn btn-primary' href='" + grpUrl + "'>View</a>");
                    editButton.Text = ("<a class='btn btn-secondary' href='" + editUrl + "'>Edit</ a>");
                    TableRow row = new TableRow();
                    row.Cells.Add(seqNo);
                    row.Cells.Add(grpName);
                    row.Cells.Add(grpDesc);
                    row.Cells.Add(button);
                    row.Cells.Add(editButton);
                    GroupsList.Rows.Add(row);
                    i++;
                }
            }
            
        }
    }
}