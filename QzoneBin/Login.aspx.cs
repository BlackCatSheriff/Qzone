using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           
            if (Request.Cookies["userQQ"]!=null  && Request.Cookies["passWord"]!=null)
        {
            //判断qq和密码是否匹配
          int judge= class_Operate.isRght(Request.Cookies["userQQ"].Value, Request.Cookies["passWord"].Value);
            if(judge==1)
            {
                Response.Redirect("comment.aspx");
                
            }
           
        }
       else if(Request.Cookies["userQQ"] != null && Request.Cookies["ImgPath"] != null)
        {
           
            
                imgUserImg.ImageUrl = Request.Cookies["ImgPath"].Value;
                imgUserImg.Visible = true;
                LtxtUsername.Text = Request.Cookies["userQQ"].Value;
                lblUserNike.Text = "欢迎你，" + HttpUtility.UrlDecode(Request.Cookies["userNick"].Value, Encoding.GetEncoding("utf-8"));
                lblUserNike.Visible = true;



           
        }
        }


    }

    protected void btnRegester_Click(object sender, EventArgs e)
    {
        
        if (LtxtUsername.Text.Trim() == "" && LtxtPassword.Text.Trim() == "")
        {
            Response.Write("<script>alert('登录失败，请正确填写QQ号、密码！')</script>");
            return;
        }
        else if (Session["Vnum"].ToString() != txtValidator.Text.Trim())
        {
            Response.Write("<script>alert('登录失败，请检查验证码！')</script>");
            return;
        }
        else
        {
           
            if (Logining(LtxtUsername.Text.Trim(),LtxtPassword .Text .Trim ()) != 1)
            {
                Response.Write("<script>alert('登录失败，请正确填写用户名、密码！')</script>");
                return;

            }
            else
            {
                Session.Clear();
                //更新cookies
                bool a=writCookies(LtxtUsername.Text.Trim(), LtxtPassword.Text.Trim());
                if(a)
                {
                   string url = "home.aspx?uqq=" + LtxtUsername.Text.Trim();
                    Response.Write("<script>window.alert('登录成功！');window.location='" + url + "'</script>");
                }
                

                else
                    Response.Write("<script>window.alert('cookies fail！');</script>");
            }

        }

    }

    protected void imgbtnLvalid_Click(object sender, ImageClickEventArgs e)
    {
        this.imgbtnLvalid .ImageUrl = "~/ValidatorPage.aspx?" + DateTime.Now.Millisecond.ToString();
    }

    protected void btnJumpToRegister_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Regester.aspx");
    }
    public int  Logining(string UserQQ ,string Password )   //登录验证函数
    {

        string pwd= class_Operate.EncryptToSHA1(Password);

        if (class_Operate.isRght(UserQQ, pwd) != 1)
             return 0;
        else return 1;

    }

    protected void btnSeekPwd_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/forgetpwd.aspx");
        
    }

    private bool writCookies(string Uqq,string Upwd)
    {

        string sql = "select Unick,Uheadimg from Users where Uqq='" + Uqq+"'";
        DataTable dt = class_Operate.SelectT(sql);
        if (dt.Rows.Count > 0)
        {
            string unike = dt.Rows[0][0].ToString();
            string uimgpath = dt.Rows[0][1].ToString();
            class_Operate inputCookies = new class_Operate();
            if (inputCookies.WriteCookies(Uqq, unike, Upwd, uimgpath) == 1)
                return true;
            else return false;


        }
        else
            return false;

   
    }

    

    }
