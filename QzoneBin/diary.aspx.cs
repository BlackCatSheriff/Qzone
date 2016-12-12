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

        if (Request.Cookies["userQQ"] == null)
        {
            Response.Write("<script language='javascript'>window.alert('身份过期，请重新登录！');window.location='Login.aspx'</script>");
            return;
        }
        if (Request.QueryString["uqq"] == null)                     //验证地址栏传值
        {

            Session["GuestQQ"] = Request.Cookies["userQQ"].Value;           //初始化host和gueset
            Session["HostQQ"] = Request.Cookies["userQQ"].Value;

        }
        else
        {
            Session["HostQQ"] = Request.QueryString["uqq"].ToString().Trim();
            Session["GuestQQ"] = Request.Cookies["userQQ"].Value;

        }
        try     //防止uqq正常传值遭到破坏，如果破坏刷新访客主页
        {
            IniHeadHost(Session["HostQQ"].ToString().Trim());
        }
        catch
        {
            IniHeadHost(Request.Cookies["userQQ"].Value);
            Session["HostQQ"] = Request.Cookies["userQQ"].Value;
            Response.Write(Session["HostQQ"].ToString());
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

                HostbindTitle(1);
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
        imgbutFriends.PostBackUrl = "relation.aspx?uqq=" + Session["GuestQQ"].ToString().Trim();
        imgbtnSetting.PostBackUrl = "editInfo.aspx";
        imgbtnMyhome.PostBackUrl = "home.aspx?uqq=" + Session["GuestQQ"].ToString().Trim();
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
                     btnWrite.Visible = false; 
           lbtGuestUsername.Text = HttpUtility.UrlDecode(Request.Cookies["userNick"].Value,Encoding.GetEncoding("utf-8"));
            lblHostQnike.Text = dt.Rows[0][0].ToString();
            lblHostUsername.Text = dt.Rows[0][1].ToString();
            lblHostSignature.Text = dt.Rows[0][2].ToString();
            lblHostGrade.Text = "空间等级:" + dt.Rows[0][3].ToString();
            imgBtnHostHead.ImageUrl = dt.Rows[0][4].ToString();

            return 1;


        }
    }

    private void HostbindTitle (int currentPage)
    {

        try
        {
            
            string sql = "select * from Diary where Dqq='" + Session["HostQQ"].ToString().Trim() + "' order by DpublishTime desc";
            DataTable dt = class_Operate.SelectT(sql);

            PagedDataSource pds = new PagedDataSource();
            pds.AllowPaging = true;
            pds.PageSize = 8;
            pds.DataSource = dt.DefaultView;
            lbTotal.Text = pds.PageCount.ToString();
            if (pds.PageCount > 1)
                divTool.Visible = true;
            pds.CurrentPageIndex = currentPage - 1;
            rptDisplayDiay.DataSource = pds;
            rptDisplayDiay.DataBind();

        }
        catch (Exception ex)
        {
            Response.Write(ex);
            
        }
    }
    
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        HostbindTitle(1);
        lbNow.Text = "1";


    }

    protected void btnUp_Click(object sender, EventArgs e)
    {
        if (lbNow.Text == "1")
        {
            HostbindTitle(1);
            lbNow.Text = "1";
        }
        else
        {
            lbNow.Text = (Convert.ToInt32(lbNow.Text) - 1).ToString();
            HostbindTitle(Convert.ToInt32(lbNow.Text));

        }

    }

    protected void btnDrow_Click(object sender, EventArgs e)
    {
        if (lbNow.Text == lbTotal.Text)
        {
            HostbindTitle(1);
            lbNow.Text = "1";
        }
        else
        {
            lbNow.Text = (Convert.ToInt32(lbNow.Text) + 1).ToString();
            HostbindTitle(Convert.ToInt32(lbNow.Text));
        }

    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        HostbindTitle(Convert.ToInt32(lbTotal.Text));
        lbNow.Text = lbTotal.Text;

    }
    
    protected void btnJump_Click(object sender, EventArgs e)
    {
        if ((Convert.ToInt32(txtJump.Text.Trim()) < 1) || (Convert.ToInt32(txtJump.Text.Trim()) > Convert.ToInt32(lbTotal.Text.Trim())))
        {
            HostbindTitle(1);
            lbNow.Text = "1";

        }
        else
        {
            HostbindTitle(Convert.ToInt32(txtJump.Text.Trim()));
            lbNow.Text = txtJump.Text.Trim();

        }

    }
    

    protected void btnWrite_Click(object sender, EventArgs e)
    {
        Response.Write("<script language='javascript'>window.open('writediary.aspx', 'newwindow', 'height=800, width=1100, top='+Math.round((window.screen.height-800)/2)+',left='+Math.round((window.screen.width-1100)/2)+', toolbar=no,menubar = no, scrollbars = no, resizable = no, location = no, status = no')</script>");
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