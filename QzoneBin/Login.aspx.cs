using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try                                                            //处理程序中不存在 Session["Succeed_name"] 的情况
        {
            LtxtUsername.Text = Session["Succeed_name"].ToString();     //显示刚刚注册的用户名
        }
        catch
        {
            return;
        }
        


    }

    protected void btnRegester_Click(object sender, EventArgs e)
    {
        
        if (LtxtUsername.Text.Trim() == "" && LtxtPassword.Text.Trim() == "")
        {
            Response.Write("<script>alert('登录失败，请正确填写用户名、密码！')</script>");
            return;
        }
        else if (Session["Vnum"].ToString() != txtValidator.Text.Trim())
        {
            Response.Write("<script>alert('登录失败，请检查验证码！')</script>");
            return;
        }
        else
        {
            /*
            if (mylogin.Login(LtxtUsername.Text.Trim(), LtxtPassword.Text.Trim()) != 1)
            {
                Response.Write("<script>alert('登录失败，请正确填写用户名、密码！')</script>");
                return;

            }
            else
            {
                
                Session.Clear();      //清空全部登录类的页间值
                Response.Write("<script>window.alert('登录成功！');location.href='information.aspx';</script>");
              

            }
            */
            //tmp = mylogin.Encryption(LtxtPassword.Text.Trim());
            //Response.Write(tmp);
            if (Logining(LtxtUsername.Text.Trim(),LtxtPassword .Text .Trim ()) != 1)
            {
                Response.Write("<script>alert('登录失败，请正确填写用户名、密码！')</script>");
                return;

            }
            else
            {


                Session.Clear();      //清空全部登录类的页间值
                Session["Succeed_name"] = LtxtUsername.Text.Trim();
                 Response.Write("<script>window.alert('登录成功！');location.href='Main.aspx';</script>");


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
    public int  Logining(string Username ,string Password )   //登录验证函数
    {

        string sql = "select Username , Password from Users where Username ='" + Username + "' and Password =upper(substring(sys.fn_sqlvarbasetostr(HashBytes('SHA1', '" + Password + "')), 3, 32))";
        DataTable dt = new DataTable();
        dt = class_Operate.SelectT(sql);
        if (dt.Rows.Count != 0) return 1;
        else return 0;
    }
}