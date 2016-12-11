using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class comment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
       


      

            if (Request.Cookies["userQQ"] != null && Request.Cookies["passWord"] != null)
            {


                //判断qq和密码是否匹配
                int judge = class_Operate.isRght(Request.Cookies["userQQ"].Value, Request.Cookies["passWord"].Value);
                if (judge != 1)

                {
                    Response.Write("<script language='javascript'>window.alert('身份过期，请重新登录！');window.location='Login.aspx'</script>");
                    return;
                }
                Session["HostQQ"] = Request.Cookies["userQQ"].Value;
                if(!IsPostBack)
            {
                IniHeadHost(Session["HostQQ"].ToString().Trim());
                myBind();

            }
            }
            else
                Response.Write("<script language='javascript'>window.alert('身份过期，请重新登录！');window.location='Login.aspx'</script>");

        




    }
    private void myBind()
    {
        

        string sqlBind = "select * from Users where Uqq='" + Request.Cookies["userQQ"].Value + "'";
        DataTable dt = class_Operate.SelectT(sqlBind);
        //imgUserhead.ImageUrl = dt.Rows[0][7].ToString();
        txtUserNike.Text = dt.Rows[0][1].ToString();
        ddlSex.Text = dt.Rows[0][5].ToString();
        txtBirthday.Text = dt.Rows[0][6].ToString();
        lblStarttime.Text = dt.Rows[0][8].ToString();
        lblEmail.Text = dt.Rows[0][3].ToString();
        lblPhone.Text = dt.Rows[0][4].ToString();
        txtSign.Text = dt.Rows[0]["UzoneSign"].ToString();

    }
    private int IniHeadHost(string Uqq)
    {


        aHome.HRef = "home.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        aSay.HRef = "comment.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        aDaily.HRef = "diary.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        aAlbum.HRef = "album.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        aMessage.HRef = "message.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        aLog.HRef = "log.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        imgbutFriends.PostBackUrl = "relation.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        imgbtnSetting.PostBackUrl = "editInfo.aspx";
        imgbtnMyhome.PostBackUrl = "home.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        imbtnPersonality.PostBackUrl = "dynamic.aspx";
        imgBtnHostHead.PostBackUrl = "editInfo.aspx";

        string sql = "select Unick,UzoneName,UzoneSign,UzoneGrade,Uheadimg from Users where Uqq=@uqq ";

        SqlConnection connection = new SqlConnection(class_Operate.str);
        connection.Open();

        SqlCommand cmd = new SqlCommand(sql, connection);
        cmd.Parameters.Clear();

        cmd.Parameters.Add("@uqq", SqlDbType.VarChar);
        cmd.Parameters["@uqq"].Value = Uqq;

        int i = cmd.ExecuteNonQuery();

        if (i == 0)
        {
            connection.Close();
            return 0;
        }

        else
        {
            SqlDataReader sdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(sdr);
            connection.Close();
           lbtGuestUsername.Text = HttpUtility.UrlDecode(Request.Cookies["userNick"].Value,Encoding.GetEncoding("utf-8"));
            lblHostQnike.Text = dt.Rows[0][0].ToString();
            lblHostUsername.Text = dt.Rows[0][1].ToString();
            lblHostSignature.Text = dt.Rows[0][2].ToString();
            lblHostGrade.Text = "空间等级:" + dt.Rows[0][3].ToString();
            imgBtnHostHead.ImageUrl = dt.Rows[0][4].ToString();

            return 1;


        }
    }
    
    

    protected void btnbaseinfo_Click(object sender, EventArgs e)
    {
        divbaseinfo.Visible = true;
        divadvanceInfo.Visible = false;
    }

    protected void btnadcvanceInfo_Click(object sender, EventArgs e)
    {
        divadvanceInfo.Visible = true;
        divbaseinfo.Visible = false;
     }

    protected void btnSubpwd_Click(object sender, EventArgs e)
    {
        //认为密码一致且合法
        string mm = class_Operate.EncryptToSHA1(txtpwd.Text);
        string sqluodate = "update Users set Upwd=@pwd where Uqq=@uQQ";

        SqlConnection connection = new SqlConnection(class_Operate.str);
        connection.Open();

        SqlCommand cmd = new SqlCommand(sqluodate, connection);
        cmd.Parameters.Clear();

        cmd.Parameters.Add("@pwd", SqlDbType.NVarChar);
        cmd.Parameters["@pwd"].Value = mm;

        cmd.Parameters.Add("@uQQ", SqlDbType.NVarChar);
        cmd.Parameters["@uQQ"].Value = Session["HostQQ"].ToString();

        int i = cmd.ExecuteNonQuery();
        connection.Close();
        if (i == 1)
            Response.Write("<script language='javascript'>window.alert('密码修改成功！')</script>");
        else
            Response.Write("<script language='javascript'>window.alert('密码修改失败！')</script>");



    }

    private int updateInfo(string Username, string sex, string birthday,string sign, string qq)
    {
        string sqluodate = "update Users set Unick=@uNike ,Usex=@uSex ,Ubirthday=@uBirthay ,UzoneSign=@sign where Uqq=@uQQ";

        SqlConnection connection = new SqlConnection(class_Operate.str);
        connection.Open();

        SqlCommand cmd = new SqlCommand(sqluodate, connection);
        cmd.Parameters.Clear();

        cmd.Parameters.Add("@uNike", SqlDbType.NVarChar);
        cmd.Parameters["@uNike"].Value = Username;

        cmd.Parameters.Add("@sign", SqlDbType.NVarChar);
        cmd.Parameters["@sign"].Value = sign;

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

    protected void btnInfoSub_Click(object sender, EventArgs e)
    {
        string userSex = ddlSex.SelectedValue;
        string userNike = txtUserNike.Text;
        string userBirthday = txtBirthday.Text;
        string userQQ = Request.Cookies["userQQ"].Value;
        string usersign = txtSign.Text;

        if (updateInfo(userNike, userSex, userBirthday,usersign,userQQ) == 0)
        {
            
            Response.Write("<script>window.alert('修改失败！');location.href='editInfo.aspx';</script>");  //如果失败跳转到资料更改页面
        }
        else
        {
            class_Operate inputcookie = new class_Operate();
            inputcookie.WriteCookies(userQQ, userNike, "", "");
            Response.Write("<script>window.alert('修改成功！');location.href='home.aspx';</script>");

        }
    }
    //----------------------------------顶部按钮--------------------
    protected void imgbtnExit_Click(object sender, ImageClickEventArgs e)
    {
        Session.Clear();
        HttpCookie cookiePassWord = new HttpCookie("passWord", "hahahah");
        cookiePassWord.Expires = DateTime.Now.AddDays(-1);  //设置密码-1day 来由浏览器清楚cookies 
        HttpContext.Current.Response.Cookies.Add(cookiePassWord);
        Response.Write("<script language='javascript'>window.alert('登出成功！');window.location='Login.aspx'</script>");
    }



    protected void imgbutFriends_Click(object sender, ImageClickEventArgs e)
    {
        Response.Write("<script language='javascript'>window.location='" + imgbutFriends.PostBackUrl + "'</script>");
    }

    protected void imgbtnSetting_Click(object sender, ImageClickEventArgs e)
    {
        Response.Write("<script language='javascript'>window.location='" + imgbtnSetting.PostBackUrl + "'</script>");
    }

    protected void lbtGuestUsername_Click(object sender, EventArgs e)
    {
        Response.Write("<script language='javascript'>window.location='" + imgbtnSetting.PostBackUrl + "'</script>");
    }

    protected void imbtnPersonality_Click(object sender, ImageClickEventArgs e)
    {
        Response.Write("<script language='javascript'>window.location='" + imbtnPersonality.PostBackUrl + "'</script>");
    }

    protected void imgbtnMyhome_Click(object sender, ImageClickEventArgs e)
    {
        Response.Write("<script language='javascript'>window.location='" + imgbtnMyhome.PostBackUrl + "'</script>");
    }


    protected void imgBtnHostHead_Click(object sender, ImageClickEventArgs e)
    {
        Response.Write("<script language='javascript'>window.location='" + imgBtnHostHead.PostBackUrl + "'</script>");
    }
}