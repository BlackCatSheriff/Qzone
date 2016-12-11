using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Net;

/// <summary>
/// Operate 的摘要说明
/// </summary>
public class class_Operate : System.Web.UI.Page
{
    public class_Operate()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    static public string str = @"Data Source=DESKTOP-A1L7N2O;Initial Catalog=Qzone;Integrated Security=True";
   // string sqlconn= ConfigurationManager.ConnectionStrings["sqlpath"].ToString();
    static public int GO(string sql)   //返回影响行数 成功  0 插入失败 -1 出错
    {
        try
        {
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            int re = cmd.ExecuteNonQuery();
            if (re != 0) return re;
            else return 0;

        }
        catch (Exception ex)
        {
            return -1;
        }
    }

    static public DataTable SelectT(string sql)  //成功返回数据表，失败返回null
    {
        try
        {
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(dt);
            conn.Close();
            return dt;
        }
        catch (Exception ex)
        {
            return null;
        }
    }


    static public string SelectHead(string sql)    //成功返回查询出表的左上角元素 失败返回-1
    {
        try
        {
            DataTable dt = SelectT(sql);
            return dt.Rows[0][0].ToString();
        }
        catch (Exception ex)
        {
            return "-1";
        }
    }
   static public string EncryptToSHA1(string str)//28bit
    {
        SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
        byte[] str1 = Encoding.UTF8.GetBytes(str);
        byte[] str2 = sha1.ComputeHash(str1);
        sha1.Clear();
        (sha1 as IDisposable).Dispose();
        return Convert.ToBase64String(str2);
    }

   
    public  int  WriteCookies(string qq,string nike,string pwd,string HeadimgPath)    //pwd、path空也可以更新cookies
    {
        if(pwd.Trim()!="")
        {
            string jiami = class_Operate.EncryptToSHA1(pwd);
            HttpCookie cookiePassWord = new HttpCookie("passWord",jiami);
            cookiePassWord.Expires = DateTime.Now.AddMinutes(5);  //设置密码5分钟过期,当前时间算往后推 
            HttpContext.Current.Response.Cookies.Add(cookiePassWord);
        }
        if(HeadimgPath.Trim()!="")
        {
            HttpCookie cookieHeadPath = new HttpCookie("ImgPath", HeadimgPath);
            cookieHeadPath.Expires = DateTime.Now.AddDays(2);
            HttpContext.Current.Response.Cookies.Add(cookieHeadPath);
           
        }
        HttpCookie cookieUserQQ = new HttpCookie("userQQ", qq); //创建帐号的cookie实例
        HttpCookie cookieUserName = new HttpCookie("userNick"); //创建帐号的cookie实例

      
         cookieUserName.Value = HttpUtility.UrlEncode(nike, Encoding.GetEncoding("utf-8"));
        cookieUserName.Expires = DateTime.Now.AddDays(2); //设置帐号cookie的过期时间，当前时间算往后推 
        cookieUserQQ.Expires = DateTime.Now.AddDays(2);
        
        HttpContext.Current.Response.Cookies.Add(cookieUserQQ);
        HttpContext.Current.Response.Cookies.Add(cookieUserName);
       
        if (HttpContext.Current.Request.Cookies["userQQ"] != null && HttpContext.Current.Request.Cookies["passWord"] != null && HttpContext.Current.Request.Cookies["userNick"] != null && HttpContext.Current.Request.Cookies["ImgPath"] != null)
        {
             
            return 1;

        }
        else
            return 0;
 

    }

    public static int isRght(string userQQ,string uerPwd)    //每个界面load的验证，存在的话不需要登录  //使用参数化查询防止cookie被恶意修改
    {
        
        SqlConnection scon = new SqlConnection(class_Operate.str);

        scon.Open();

        SqlCommand selectCmd = new SqlCommand();

        selectCmd.CommandText = "select Uqq from Users where Uqq=@uqq and Upwd=@pwd";

        selectCmd.Parameters.Add("@uqq", SqlDbType.VarChar, 10);//sql指令中存在一个参数，叫@sn,它的类型是字符型，字节长度是10个

        selectCmd.Parameters["@uqq"].Value = userQQ;

        selectCmd.Parameters.Add("@pwd", SqlDbType.VarChar, 30);

        selectCmd.Parameters["@pwd"].Value = uerPwd;


        selectCmd.Connection = scon;

        SqlDataAdapter sda = new SqlDataAdapter();

        sda.SelectCommand = selectCmd;

        DataTable dt = new DataTable();

        sda.Fill(dt);
        if (dt.Rows.Count > 0)

            return 1;
        else
            return 0;

    }
    
    public static bool QqGrade(string hostQQ,int operateKind)
    {
        int getGrade;
        
        string sqlgetOldTime = "select Ustarttime from Users  where Uqq='" + hostQQ + "' ";
        DateTime dt = DateTime.Parse(class_Operate.SelectHead(sqlgetOldTime));
        TimeSpan ts = DateTime.Now - dt;
        int TotalTime = Convert.ToInt32(ts.TotalHours);
        
        switch (operateKind)
        {
            case 0: getGrade=Convert.ToInt32( TotalTime*0.15); break;    //注册日期到当前
            case 1:getGrade = 1; break;                                 //说说
            case 2:getGrade = 2;  break;                                  //日志
            case 3:getGrade = 3;  break;                                //照片
            case 4:getGrade = 1;    break;                              //留言
            default: getGrade = 0;  break;


        }

        string sqlupdate = "update Users set UzoneGrade+=" + getGrade.ToString() + "  where Uqq='" + hostQQ + "'";
        if (class_Operate.GO(sqlupdate) == 1)
            return true;
        else
            return false;
    }


    public static bool RecordLog(string hostqq, string guestqq)
    {
        if (hostqq == guestqq)      //自己访问自己的不记录
            return true;
        else
        {
            string today = DateTime.Today.ToString("yyyyMMdd");
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sqlistoday = "select COUNT(*) from View_Log where LhostQq='" + hostqq + "' and LtimeDay='" + today + "' and LguestQq='" + guestqq + "'";//查询是否今天访问过，如果访问过则更新，否则插入
            if (SelectHead(sqlistoday) == "0")
            {
                string sqladdlog = "insert into View_Log(LhostQq,LguestQq,LtimeAll,LtimeDay) values ('" + hostqq + "','" + guestqq + "','" + time + "','" + today + "')";
                if (GO(sqladdlog) == 1)
                    return true;
                else
                    return false;
            }
            else
            {
                string sqluodatelog = "update View_Log set LtimeAll='" + time + "' where LguestQq='" + guestqq + "' and LtimeDay='" + today + "' and LhostQq='" + hostqq + "'";
                if (GO(sqluodatelog) == 1)
                    return true;
                else
                    return false;
            }
        }
    }



        public static bool  Mail(string targetEmai,string theme ,string content)
    {
        try
        {
        MailMessage mailObj = new MailMessage();
        mailObj.From = new MailAddress("wwwsutianyu@163.com"); //发送人邮箱地址
        mailObj.To.Add(targetEmai);   //收件人邮箱地址
        mailObj.Subject = theme;    //主题
        mailObj.Body = content;    //正文
        SmtpClient smtp = new SmtpClient();
        smtp.Host = "smtp.163.com";         //smtp服务器名称
        smtp.UseDefaultCredentials = true;
        smtp.Credentials = new NetworkCredential("wwwsutianyu@163.com", "return!Sty");  //发送人的登录名和密码
        smtp.Send(mailObj);
            return true;
        }
        catch(Exception ex )
        {
            return false;
        }
    }







    }












