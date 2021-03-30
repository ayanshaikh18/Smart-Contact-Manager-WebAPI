using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebClient.Groups
{
    public partial class DeleteGroupContact : System.Web.UI.Page
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                this.Context.Items.Add("ErrorMessage", "Please Login");
                Response.Redirect("/Account/Login.aspx?msg=Please Login..!", false);
                return;
            }
            if (Request.QueryString["Id"] == null)
            {
                Response.Redirect("~/404.aspx");
            }
            int id = Int32.Parse(Request.QueryString["Id"]);
            string url = "https://localhost:44373/api/groups/deleteGroupContact/" + id;
            HttpClient client = new HttpClient();
            var result = await client.DeleteAsync(url);

            int GroupId = Int32.Parse(Request.QueryString["GroupId"]);

            Response.Redirect("~/Groups/ViewGroup.aspx?GroupId=" + GroupId);
        }
    }
}