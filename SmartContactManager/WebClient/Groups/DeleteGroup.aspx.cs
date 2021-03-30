using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebClient.Models;

namespace WebClient.Groups
{
    public partial class DeleteGroup : System.Web.UI.Page
    {
        int userId, groupId;
        HttpClient client = new HttpClient();
        string url = "https://localhost:44373/api/groups/";
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] == null)
                {
                    this.Context.Items.Add("ErrorMessage", "Please Login");
                    Response.Redirect("/Account/Login.aspx?msg=Please Login..!", false);
                    return;
                }
                if (Request.QueryString["GroupId"] == null)
                {
                    Response.Redirect("~/404.aspx",false);
                    return;
                }
                groupId = Int32.Parse(Request.QueryString["GroupId"]);

                userId = Int32.Parse(Session["UserId"].ToString());
                var group = new Group();
                group.UserId = userId;
                group.Id = groupId;
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
                group = JsonConvert.DeserializeObject<Group>(await result.Content.ReadAsStringAsync());
                if (userId != group.UserId)
                    Server.Transfer("~/AccessDenied.aspx");
                GrpData.Text = "Name :- " + group.Name +
                                "<br>Description :- " + group.Description +
                                "<br>Total Contacts :- " + 5;

                ViewState["GroupId"] = groupId.ToString();

            }
        }

        protected async void SubmitButton_Click(object sender, EventArgs e)
        {
            url = "https://localhost:44373/api/groups/";
            groupId = Int32.Parse(ViewState["GroupId"].ToString());
            url += groupId;
            var result = await client.DeleteAsync(url);
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
            Response.Redirect("~/Groups/GroupList.aspx");
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups/GroupList.aspx");
        }
    }
}