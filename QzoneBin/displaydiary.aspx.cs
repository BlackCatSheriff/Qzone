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
        //uqq=" + Eval("Dqq")+"&diaryid=" 
        if (Request.Cookies["userQQ"] == null)
        {
            Response.Write("<script language='javascript'>window.alert('身份过期，请重新登录！');window.location='Login.aspx'</script>");
            return;
        }

        if (Request.QueryString["uqq"] == null || Request.QueryString["diaryid"] == null)
        {
            div1.Visible = false;
       
            div404.Visible = true;


            // Response.Write("<script language='javascript'>window.alert('非法访问！');window.close();window.open('','_self');</script>");
            return;

        }
        else
        {
            Session["HostQQ"] = Request.QueryString["uqq"].ToString().Trim();
            Session["GuestQQ"] = Request.Cookies["userQQ"].Value;
            Session["Diaryid"] = Request.QueryString["diaryid"].ToString().Trim();


        }

        if (!IsPostBack)
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

                IniHeadHost(Session["HostQQ"].ToString().Trim());
                Contentbind();
            }
            else
                Response.Write("<script language='javascript'>window.alert('身份过期，请重新登录！');window.location='Login.aspx'</script>");

        }



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
            if (Uqq != Session["GuestQQ"].ToString().Trim())  //验证是否自己空间
            {
                btnDelete.Visible = false;
                btnEdit.Visible = false;
                btnSave.Visible = false;

            }
           lbtGuestUsername.Text = HttpUtility.UrlDecode(Request.Cookies["userNick"].Value,Encoding.GetEncoding("utf-8"));
            lblHostQnike.Text = dt.Rows[0][0].ToString();
            lblHostUsername.Text = dt.Rows[0][1].ToString();
            lblHostSignature.Text = dt.Rows[0][2].ToString();
            lblHostGrade.Text = "空间等级:" + dt.Rows[0][3].ToString();
            imgBtnHostHead.ImageUrl = dt.Rows[0][4].ToString();

            return 1;


        }
    }
 

    private void Contentbind()
    {

        

        try
        {

            string sql = "select Dcontent from Diary where Did='"+ Session["Diaryid"].ToString()+ "'";
            string sql1 = "select Dtitle from Diary where Did='" + Session["Diaryid"].ToString() + "'";

            txtDiary.Text = class_Operate.SelectHead(sql);
            txtTile.Text = class_Operate.SelectHead(sql1);

        }
        catch (Exception ex)
        {
            Response.Write(ex);
            
        }
    }
    protected void btnReturnList_Click(object sender, EventArgs e)
    {
        //Response.Write("<script language='javascript'>window.close();window.open('','_self');</script>");
       Response.Write("<script language='javascript'>window.location='diary.aspx'</script>");
      //  Response.Write("<script language='javascript'>window.close();window.open('','_self');</script>");
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
       txtDiary.Enabled = true;
        txtTile.Enabled = true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string url = "~/displaydiary.aspx?uqq=" + Request.Cookies["userQQ"].Value + "&diaryid=" + Session["Diaryid"].ToString();


        string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string sqlconn = class_Operate.str;
  
        SqlConnection connection = new SqlConnection(sqlconn);
        connection.Open();
        
        string sqlstr = "update Diary set Dcontent=@text ,DpublishTime =@time ,Dtitle=@title  where Did='" + Session["Diaryid"].ToString() + "'";
        SqlCommand cmd = new SqlCommand(sqlstr, connection);
        cmd.Parameters.Clear();


        cmd.Parameters.Add("@text", SqlDbType.NVarChar);
        cmd.Parameters["@text"].Value = txtDiary.Text;

        cmd.Parameters.Add("@time", SqlDbType.VarChar);
        cmd.Parameters["@time"].Value = time;


        cmd.Parameters.Add("@title", SqlDbType.NVarChar);
        cmd.Parameters["@title"].Value = txtTile.Text.Trim();

        int i = cmd.ExecuteNonQuery();
        connection.Close();
        if (i == 1)
            Response.Write("<script language='javascript'>window.alert('修改成功');window.location='diary.aspx';</script>");
        else
            Response.Write("<script language='javascript'>window.alert('修改失败，请重试！');window.location='diary.aspx'</script>");

    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        //string url = "~/displaydiary.aspx?uqq=" + Request.Cookies["userQQ"].Value + "&diaryid="+ Session["Diaryid"].ToString();
        string sqldel = "delete Diary where Did='" + Session["Diaryid"].ToString() + "'";
        if(class_Operate.GO(sqldel)==1)
            Response.Write("<script language='javascript'>window.alert('删除成功');window.location='diary.aspx'</script>");
        else
            Response.Write("<script language='javascript'>window.alert('删除失败！请重试！');window.location='diary.aspx'</script>");
    }

    protected void btnexit_Click(object sender, EventArgs e)
    {
        Response.Write("<script language='javascript'>window.alert('别淘气！');window.location='diary.aspx'</script>");
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