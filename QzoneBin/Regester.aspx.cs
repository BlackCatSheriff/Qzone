using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Regester : System.Web.UI.Page
{
    public string qq;
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Session["Vnum"]==null)
        {
            this.imgValidator.ImageUrl = "~/ValidatorPage.aspx?" + DateTime.Now.Millisecond.ToString();      //验证码刷新
        }
        /*
        else
        {
            Response.Write("<script>alert('"+Session["Vnum"].ToString()+"')</script>");
        }
        */

    }

    protected void btnRegester_Click(object sender, EventArgs e)
    {
        if (Session["Vnum"].ToString() != txtValidator.Text.Trim())
        {
            Response.Write("<script>alert('登录失败，请检查验证码！')</script>");
            return;
        }
        else if ( !RegularExpressionValidator_Email.IsValid && !RegularExpressionValidator_phone.IsValid && !CompareValidator_Pwd.IsValid)   //是否通过验证控件的判断
        {
            Response.Write("<script>alert('注册失败请正确填写信息！')</script>");
            return;
        }
        else
        {
            
                if (Register(txtUsername.Text.Trim(), txtPassword1.Text.Trim(), txtEmail.Text.Trim(), txtPhone.Text.Trim()) != 1)
                {
                    Response.Write("<script>alert('注册失败请正确填写信息！')</script>");
                    return;
                }
                else
                {
             
                class_Operate inputCookies = new class_Operate();
                
               int xx= inputCookies.WriteCookies(qq, txtUsername.Text.Trim(), txtPassword1.Text.Trim(), "~/img/userHead/base.png");

                Session["isFirst"] = "1";
                
                Response.Write("<script language='javascript'>window.alert('注册成功"+xx.ToString()+"session"+ Session["isFirst"].ToString() + "');window.location='regsuccess.aspx'</script>");
            }

            }
        
        
    }


    protected void imgValidator_Click(object sender, ImageClickEventArgs e)
    {
        this.imgValidator.ImageUrl = "~/ValidatorPage.aspx?" + DateTime.Now.Millisecond.ToString();      //验证码刷新

    }
    public int Register(string Username, string Password, string Email, string Phone)    //写入到数据库Users
    {
         qq = product_qq();
        string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string Encrypt = class_Operate .EncryptToSHA1(Password);
        string sqlconn = class_Operate.str;
        string headimg = "~/img/userHead/base.png"; 
        SqlConnection connection = new SqlConnection(sqlconn);
        connection.Open();
        string sqlstr = "insert into Users(Uqq,Unick,Upwd,Uemail,Uphone,Uheadimg,Ustarttime,UzoneName,UzoneSign,UzoneGrade,Usignin,UsignNow) VALUES  (@Uqq,@Unick,@Upwd,@Uemail,@Uphone,@Uheadimg,@Ustarttime,@UzoneName,@UzoneSign,@UzoneGrade,@Usignin,@UsignNow)";
        SqlCommand cmd = new SqlCommand(sqlstr, connection);
        cmd.Parameters.Clear();

       
        cmd.Parameters.Add("@Uqq", SqlDbType.Text);
        cmd.Parameters["@Uqq"].Value = qq;

        cmd.Parameters.Add("@Unick", SqlDbType.Text);
        cmd.Parameters["@Unick"].Value = Username;

        cmd.Parameters.Add("@Upwd", SqlDbType.Text);
        cmd.Parameters["@Upwd"].Value = Encrypt;

        cmd.Parameters.Add("@Uemail", SqlDbType.Text);
        cmd.Parameters["@Uemail"].Value = Email;

        cmd.Parameters.Add("@Uphone", SqlDbType.Text);
        cmd.Parameters["@Uphone"].Value = Phone;

        cmd.Parameters.Add("@Ustarttime", SqlDbType.Text);
        cmd.Parameters["@Ustarttime"].Value = time;

        cmd.Parameters.Add("@Uheadimg", SqlDbType.Text);
        cmd.Parameters["@Uheadimg"].Value = headimg;

        cmd.Parameters.Add("@UzoneName", SqlDbType.NVarChar);
        cmd.Parameters["@UzoneName"].Value = Username;

        cmd.Parameters.Add("@UzoneSign", SqlDbType.NVarChar);
        cmd.Parameters["@UzoneSign"].Value = "我的空间自己主宰！";

        cmd.Parameters.Add("@UzoneGrade", SqlDbType.Char);
        cmd.Parameters["@UzoneGrade"].Value ="0";

        cmd.Parameters.Add("@Usignin", SqlDbType.Int );
        cmd.Parameters["@Usignin"].Value = 0;


        cmd.Parameters.Add("@UsignNow", SqlDbType.VarChar);
        cmd.Parameters["@UsignNow"].Value = "0";

        int i=cmd.ExecuteNonQuery();
        connection.Close();
        if (i == 1)
            return 1;
        else          
            return 0;
       
          
    }
    
    public string  product_qq()
    {
        int tmpqq;
        string sql = "";
        DataTable dt = new DataTable();
        Random rad = new Random(ConvertDateTimeInt( DateTime.Now));
        while (true)
        {
            tmpqq = rad.Next(10000000, 100000000);
            sql = "select Users.Uqq from Users where Uqq ='" + tmpqq + "'";
            dt = class_Operate.SelectT(sql);
            if (dt.Rows.Count == 0) break;
        }

        return tmpqq.ToString();
    }
    private int ConvertDateTimeInt(System.DateTime time)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        return (int)(time - startTime).TotalSeconds;
    }


}