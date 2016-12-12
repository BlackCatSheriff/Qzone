using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class forgetpwd : System.Web.UI.Page
{
    string yzmanran;     
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["userQQ"] != null)
        {
            if(!IsPostBack)
            txtqq.Text = Request.Cookies["userQQ"].Value;
        }
        if (Request.Cookies["yzman"] == null)//显示发送验证码按钮是否可用，通过设定的cookies生存时间判定
            btnGetyzm.Enabled = true;
        else
            btnGetyzm.Enabled = false;
    }

    protected void btnGetyzm_Click(object sender, EventArgs e)
    {
        if(txtqq.Text .Trim ()=="")
        {
            Response.Write("<script language='javascript'>window.alert('请检查QQ号！')</script>");
        }
        else
        {
           if(!isQQ(txtqq.Text.Trim()))
                Response.Write("<script language='javascript'>window.alert('请检查QQ号！')</script>");
           else
            {
              if(sendYzm(txtqq.Text.Trim()))
                {
                    btnGetyzm.Enabled = false;
                    btnGetyzm.Text = "60s重新发送";
                    HttpCookie yzman = new HttpCookie("yzman", yzmanran);             //创建按钮判断cookies
                    yzman.Expires = DateTime.Now.AddMinutes(1);                     //设置生存时间
                    HttpContext.Current.Response.Cookies.Add(yzman);                  //发送到客户端
                    Response.Write("<script language='javascript'>window.alert('验证码已发送到你的邮箱！请及时查看！')</script>");
                }
                    
              else
                    Response.Write("<script language='javascript'>window.alert('验证码发送失败，请重试！')</script>");
            }
        }
    }
    private bool isQQ(string qq)  //验证qq的合法性
    {
        SqlConnection scon = new SqlConnection(class_Operate.str);

        scon.Open();

        SqlCommand selectCmd = new SqlCommand();

        selectCmd.CommandText = "select Uqq from Users where Uqq=@uqq";    //查询数据库是否有这个qq

        selectCmd.Parameters.Add("@uqq", SqlDbType.VarChar, 10);//sql指令中存在一个参数，叫@sn,它的类型是字符型，字节长度是10个

        selectCmd.Parameters["@uqq"].Value = qq;

        
        selectCmd.Connection = scon;

        SqlDataAdapter sda = new SqlDataAdapter();

        sda.SelectCommand = selectCmd;

        DataTable dt = new DataTable();

        sda.Fill(dt);
        if (dt.Rows.Count > 0)

            return true;
        else
            return false;

    }
    private bool sendYzm(string qq  )   //发送验证  
    {
        string[] Number = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y" };     //验证码数据源
        Random ran = new Random();          //申请随机数对象
        int tmp;
        string yzm = "";
        for(int i=0;i<7;i++)            //随机挑出7个作为验证码
        {
            tmp = ran.Next() % 62;
            yzm = yzm + Number[tmp];
        }

        for (int i = 0; i < 7; i++)     //随机挑出7个作为刷新时间存储cookies的数据
        {
            tmp = ran.Next() % 60;
            yzmanran = yzmanran + Number[tmp];
        }

        string sqlemail = "select Uemail from Users where Uqq='"+qq+"'";    //查询出找回QQ的注册邮箱
        string useremail = class_Operate.SelectHead(sqlemail);
        string sendtext = "验证码:" + yzm + "\n请妥善保管，务泄露给其他人。验证码将于5分钟后失效。";
        if (class_Operate.Mail(useremail, "来自SKY空间的验证码", sendtext))     //发送邮件
        {
            string yzmjiami = class_Operate.EncryptToSHA1(class_Operate.EncryptToSHA1(yzm));   //验证码两次加密取中间7位存入cookie等待验证码
            string yzmcookie = yzmjiami.Substring(8, 7);
            HttpCookie cookieyzm = new HttpCookie("yzm", yzmcookie);    
            cookieyzm.Expires = DateTime.Now.AddMinutes(5);                             //设置生存时长
            HttpContext.Current.Response.Cookies.Add(cookieyzm);                    //发送到客户端
            return true;
        }
        else
            return false;
    }

    protected void btnsubyzm_Click(object sender, EventArgs e)
    {
        string yzm = txtyzm.Text.Trim();                          //获取用户输入验证码
        string cookieyzm = Request.Cookies["yzm"].Value;            //获取coolie的验证码
        string judege= class_Operate.EncryptToSHA1(class_Operate.EncryptToSHA1(yzm)).Substring(8, 7);   //判断是否一致
        if(judege!=cookieyzm)
            Response.Write("<script language='javascript'>window.alert('验证码错误，请重新获取！')</script>");
        else
        {
            Response.Write("<script language='javascript'>window.alert('验证码正确！')</script>");
            divmm.Visible = true;                                                                               //正确显示出更改密码的窗口

        }
            

        
    }

    protected void btnSubpwd_Click(object sender, EventArgs e)
    {
        
        //认为密码一致且合法
        string mm = class_Operate.EncryptToSHA1(txtpwd.Text);
        string sqluodate = "update Users set Upwd=@pwd where Uqq=@uQQ";        //更新信息

        SqlConnection connection = new SqlConnection(class_Operate.str);
        connection.Open();

        SqlCommand cmd = new SqlCommand(sqluodate, connection);
        cmd.Parameters.Clear();

        cmd.Parameters.Add("@pwd", SqlDbType.NVarChar);
        cmd.Parameters["@pwd"].Value = mm;

        cmd.Parameters.Add("@uQQ", SqlDbType.NVarChar);
        cmd.Parameters["@uQQ"].Value = txtqq.Text.Trim();

        int i = cmd.ExecuteNonQuery();
        connection.Close();
        if (i == 1)
        
            Response.Write("<script language='javascript'>window.alert('密码修改成功！');window.location='Login.aspx';</script>");
       
        else
            Response.Write("<script language='javascript'>window.alert('密码修改失败！')</script>");
    }
}