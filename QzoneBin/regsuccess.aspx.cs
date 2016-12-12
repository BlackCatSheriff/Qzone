using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class regsuccess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Session["isFirst"] == null)                         //只允许新创建的QQ第一次使用本界面
        {
            Response.Redirect("Regester.aspx");
        }
        else
        {
           // Session["isFirst"] = null;
            if (Request.Cookies["userQQ"] != null && Request.Cookies["passWord"] != null)    //防止cookie被篡改
            {
                string sqlcatchpwd = "select Users.Uqq from Users where Uqq='" + Request.Cookies["userQQ"].Value + "' and Upwd='" + Request.Cookies["passWord"].Value + "'";
                DataTable dt = class_Operate.SelectT(sqlcatchpwd);
                if (dt.Rows.Count <= 0)
                {
                    Response.Redirect("Regester.aspx");
                    return;
                }
                if (!IsPostBack)
                {
                    //显示数据
                    myBind();
                }

            }
            else
                Response.Redirect("Regester.aspx");
        }
        

    }
                
    
    private void myBind ()
    {
        lbluserNick.Text =HttpUtility.UrlDecode(Request.Cookies["userNick"].Value,Encoding.GetEncoding("utf-8"));    //读取cookies，中文进行编码转换

       
        lbluserQQ.Text = Request.Cookies["userQQ"].Value;

        string sqlBind = "select * from Users where Uqq='" + Request.Cookies["userQQ"].Value +"'";
        DataTable dt = class_Operate.SelectT(sqlBind);
        //imgUserhead.ImageUrl = dt.Rows[0][7].ToString();
        txtUserNike.Text= dt.Rows[0][1].ToString();
        ddlSex.Text = dt.Rows[0][5].ToString();
        txtBirthday.Text = dt.Rows[0][6].ToString();
        lblStarttime.Text = dt.Rows[0][8].ToString();
        lblEmail.Text = dt.Rows[0][3].ToString();
        lblPhone.Text = dt.Rows[0][4].ToString();
        
    }
    
    protected void btnInfoSub_Click(object sender, EventArgs e)
    {
        string userSex = ddlSex.SelectedValue;
        string userNike = txtUserNike.Text;
        string userBirthday = txtBirthday.Text;
        string userQQ = Request.Cookies["userQQ"].Value;

        if (updateInfo(userNike, userSex, userBirthday, userQQ)==0)
        {
            Session["isFirst"] = "1";

            Response.Write("<script>window.alert('修改失败！');location.href='regsuccess.aspx';</script>");  //如果失败跳转到资料更改页面
        }
        else
        {
            class_Operate inputcookie = new class_Operate();
            inputcookie.WriteCookies(userQQ, userNike, "", "");                                 //更新cookies
            Response.Write("<script>window.alert('修改成功！');location.href='home.aspx';</script>");

        }
        
    }

   
    private int updateInfo(string Username,string sex,string birthday,string qq)     //更新基本资料
    {
        string sqluodate = "update Users set Unick=@uNike ,Usex=@uSex ,Ubirthday=@uBirthay where Uqq=@uQQ";    //参数化查询语句

        SqlConnection connection = new SqlConnection(class_Operate.str);
        connection.Open();
        
        SqlCommand cmd = new SqlCommand(sqluodate, connection);
        cmd.Parameters.Clear();
        
        cmd.Parameters.Add("@uNike", SqlDbType.NVarChar);
        cmd.Parameters["@uNike"].Value = Username;

        cmd.Parameters.Add("@uSex", SqlDbType.NVarChar);    
        cmd.Parameters["@uSex"].Value = sex;

        cmd.Parameters.Add("@uBirthay", SqlDbType.VarChar);
        cmd.Parameters["@uBirthay"].Value = birthday;

        cmd.Parameters.Add("@uQQ", SqlDbType.VarChar);
        cmd.Parameters["@uQQ"].Value = qq;
        
        int i = cmd.ExecuteNonQuery();
        connection.Close();
        if (i == 0)
            return 0;
        else
            return 1;
    }


    protected void btnSubIMg_ServerClick(object sender, EventArgs e)
    {
     
    }
}
