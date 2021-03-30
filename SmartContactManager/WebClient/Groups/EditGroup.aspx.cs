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
    public partial class EditGroup : System.Web.UI.Page
    {
        HttpClient client = new HttpClient();
        string url = "https://localhost:44373/api/groups/";
        int userId, groupId;
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["UserID"] == null)
                {
                    this.Context.Items.Add("ErrorMessage", "Please Login");
                    Response.Redirect("/Account/Login.aspx?msg=Please Login..!", false);
                    return;
                }
                if (Request.QueryString["GroupId"] == null)
                {
                    Response.Redirect("~/404.aspx", false);
                    return;
                }
                groupId = Int32.Parse(Request.QueryString["GroupId"]);
                userId = Int32.Parse(Session["UserId"].ToString());
                ViewState["GroupId"] = groupId.ToString();
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
                Name.Text = group.Name;
                Description.Text = group.Description;
            }
        }

        protected async void SubmitButton_Click(object sender, EventArgs e)
        {
            groupId = Int32.Parse(ViewState["GroupId"].ToString());
            url = "https://localhost:44373/api/groups/";
            userId = Int32.Parse(Session["UserId"].ToString());
            var group = new Group()
            {
                Id = groupId,
                UserId = userId,
                Name = Name.Text,
                Description = Description.Text
            };

            var serializedGroup = JsonConvert.SerializeObject(group);
            var content = new StringContent(serializedGroup, Encoding.UTF8, "application/json");
            var result = await client.PutAsync(url, content);
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
                    var errorResponse = JsonConvert.DeserializeObject<ErrorViewModel>(await result.Content.ReadAsStringAsync());
                    string errorMsg = "";
                    foreach (var err in errorResponse.Error)
                        errorMsg += err;
                    ErrorMessage.Text = errorMsg;
                    return;
            }
            Response.Redirect("~/Groups/ViewGroup.aspx?GroupId=" + group.Id);
        }
    }
}