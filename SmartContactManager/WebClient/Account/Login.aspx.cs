using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebClient.Models;
using WebClient.Models.ViewModels;

namespace WebClient.Account
{
    public partial class Login : System.Web.UI.Page
    {
        static HttpClient client = new HttpClient();
        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    this.Context.Items.Add("ErrorMessage", "Access Denied! Please Login");
                    Response.Redirect("~/Dashboard.aspx");
                }
                string Success_Message = (string)this.Context.Items["SuccessMessage"];
                string Error_Message = (string)this.Context.Items["ErrorMessage"];
                if (Success_Message != null)
                {
                    SuccessMessage.Visible = true;
                    SuccessMessage.Text = Success_Message;
                    this.Context.Items.Remove("SuccessMessage");
                    ErrorMessage.Visible = false;
                }
                if (Error_Message != null)
                {
                    ErrorMessage.Visible = true;
                    ErrorMessage.Text = Error_Message;
                    this.Context.Items.Remove("ErrorMessage");
                    SuccessMessage.Visible = false;
                }
                if(Request.QueryString["msg"]!=null)
                {
                    ErrorMessage.Visible = true;
                    ErrorMessage.Text = Request.QueryString["msg"];
                    SuccessMessage.Visible = false;
                }
            }
        }

        protected async void SubmitButton_Click1(object sender, EventArgs e)
        {
            LoginUser loginUser = new LoginUser();
            loginUser.Email = Email.Text;
            loginUser.Password = Password.Text;
            var status = await login(loginUser);
            if(status)
            {
                Session["isLoggedIn"] = true;
                Response.Redirect("/Account/Dashboard.aspx", false);
            }
        }

        public async Task<bool> login(LoginUser loginUser)
        {
            var serializeduser = JsonConvert.SerializeObject(loginUser);
            var content = new StringContent(serializeduser, Encoding.UTF8, "application/json");
            var result = await client.PostAsync("https://localhost:44373/api/account/login", content);

            //model state validation remaining
            if (result.IsSuccessStatusCode)
            {
                User user = JsonConvert.DeserializeObject<User>(await result.Content.ReadAsStringAsync());
                Session["UserID"] = user.Id;
                return true;
            }
            else
            {
                if ((int)result.StatusCode == 400)
                {
                    string errors;
                    errors = await Errors.getErrors(result);
                    ErrorMessage.Text = errors;
                    return false;
                }

                string response = JsonConvert.DeserializeObject<String>(await result.Content.ReadAsStringAsync());
                SuccessMessage.Visible = false;
                ErrorMessage.Visible = true;
                ErrorMessage.Text = response;
                return false;
            }
        }
    }
}