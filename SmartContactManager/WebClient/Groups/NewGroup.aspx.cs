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
    public partial class NewGroup : System.Web.UI.Page
    {
        HttpClient client = new HttpClient();
        string url = "https://localhost:44373/api/groups/";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                this.Context.Items.Add("ErrorMessage", "Please Login");
                Response.Redirect("/Account/Login.aspx?msg=Please Login..!", false);
                return;
            }
        }

        protected async void SubmitButton_Click(object sender, EventArgs e)
        {
            var group = new Group();
            group.Name = Name.Text;
            group.Description = Description.Text;
            group.UserId = Int32.Parse(Session["UserId"].ToString());

            var serializedGroup = JsonConvert.SerializeObject(group);
            var content = new StringContent(serializedGroup, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(url, content);
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
            Response.Redirect("~/Groups/GroupList.aspx");

        }
    }
}