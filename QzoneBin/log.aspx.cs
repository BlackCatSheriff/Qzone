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
    protected string All = "";                          //绑定所有访客数，变量声明供前台使用
    protected string Today = "";                        //绑定今日访客数，变量声明供前台使用
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["uqq"] == null)
        {

            Session["GuestQQ"] = Request.Cookies["userQQ"].Value;
            Session["HostQQ"] = Request.Cookies["userQQ"].Value;

        }
        else
        {
            Session["HostQQ"] = Request.QueryString["uqq"].ToString().Trim();
            Session["GuestQQ"] = Request.Cookies["userQQ"].Value;

        }
        try     //防止uqq正常传值遭到破坏，如果破坏刷新访客主页
        {
            string today = DateTime.Today.ToString("yyyyMMdd");     //获取当天日期
            string sqltoday = "select COUNT(*) from View_Log where LhostQq = '" + Session["HostQQ"].ToString().Trim() + "' and LtimeDay = '" + today + "'";             //查询一共访问条数
            string sqlall = "select COUNT(*) from View_Log where LhostQq = '" + Session["HostQQ"].ToString().Trim() + "'";                                              //查询今日访问条数
            Today = class_Operate.SelectHead(sqltoday);
            All = class_Operate.SelectHead(sqlall);
        IniHeadHost(Session["HostQQ"].ToString().Trim());
        }
        catch   //错误处理，如果uqq被篡改，绑定当前访客账户
        {
            string today = DateTime.Today.ToString("yyyyMMdd");
            string sqltoday = "select COUNT(*) from View_Log where LhostQq = '" + Session["HostQQ"].ToString().Trim() + "' and LtimeDay = '" + today + "'";
            string sqlall = "select COUNT(*) from View_Log where LhostQq = '" + Session["HostQQ"].ToString().Trim() + "'";
            Today = class_Operate.SelectHead(sqltoday);
            All = class_Operate.SelectHead(sqlall);
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
                
                LogBindToday(1);
                LogBindBefore(1);
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
           lbtGuestUsername.Text = HttpUtility.UrlDecode(Request.Cookies["userNick"].Value,Encoding.GetEncoding("utf-8"));
            lblHostQnike.Text = dt.Rows[0][0].ToString();
            lblHostUsername.Text = dt.Rows[0][1].ToString();
            lblHostSignature.Text = dt.Rows[0][2].ToString();
            lblHostGrade.Text = "空间等级:" + dt.Rows[0][3].ToString();
            imgBtnHostHead.ImageUrl = dt.Rows[0][4].ToString();

            return 1;


        }
    }

    private void LogBindToday (int currentPage)
    {
        //查询今日数据
        try
        {
            string today = DateTime.Today.ToString("yyyyMMdd");
            string sqltoday = "select * from View_Log where LhostQq='"+ Session["HostQQ"].ToString().Trim() + "' and LtimeDay='" +today+ "' order by LtimeAll desc";
           
            DataTable dt = class_Operate.SelectT(sqltoday);
            PagedDataSource pds = new PagedDataSource();
            pds.AllowPaging = true;
            pds.PageSize = 10;
            pds.DataSource = dt.DefaultView;
            lbTotalT.Text = pds.PageCount.ToString();
            if (pds.PageCount > 1)
                divToolT.Visible = true;
            pds.CurrentPageIndex = currentPage - 1;
            rptToday.DataSource = pds;
            rptToday.DataBind();

        }
        catch (Exception ex)
        {
            Response.Write(ex);
            
        }
    }

    private void LogBindBefore( int currentPage)
    {
        //查询历史数据
        try
        {
            string today = DateTime.Today.ToString("yyyyMMdd");
            string sqlBefore  = "select * from View_Log where LhostQq='" + Session["HostQQ"].ToString().Trim() + "' and LtimeDay!='" + today + "' order by LtimeAll desc";
            
            DataTable dt = class_Operate.SelectT(sqlBefore);
            PagedDataSource pds = new PagedDataSource();
            pds.AllowPaging = true;
            pds.PageSize = 10;
            pds.DataSource = dt.DefaultView;
            lbTotalB.Text = pds.PageCount.ToString();
            if (pds.PageCount > 1)
                divToolB.Visible = true;
            pds.CurrentPageIndex = currentPage - 1;
            rptBefore.DataSource = pds;
            rptBefore.DataBind();

        }
        catch (Exception ex)
        {
            Response.Write(ex);

        }
    }


    protected void btnFirstT_Click(object sender, EventArgs e)
    {
        LogBindToday(1);
        lbNowT.Text = "1";


    }

    protected void btnUpT_Click(object sender, EventArgs e)
    {
        if (lbNowT.Text == "1")
        {
            LogBindToday(1);
            lbNowT.Text = "1";
        }
        else
        {
            lbNowT.Text = (Convert.ToInt32(lbNowT.Text) - 1).ToString();
            LogBindToday(Convert.ToInt32(lbNowT.Text));

        }

    }

    protected void btnDrowT_Click(object sender, EventArgs e)
    {
        if (lbNowT.Text == lbTotalT.Text)
        {
            LogBindToday(1);
            lbNowT.Text = "1";
        }
        else
        {
            lbNowT.Text = (Convert.ToInt32(lbNowT.Text) + 1).ToString();
            LogBindToday(Convert.ToInt32(lbNowT.Text));
        }

    }

    protected void btnLastT_Click(object sender, EventArgs e)
    {
        LogBindToday(Convert.ToInt32(lbTotalT.Text));
        lbNowT.Text = lbTotalT.Text;

    }
    
    protected void btnJumpT_Click(object sender, EventArgs e)
    {
        if ((Convert.ToInt32(txtJumpT.Text.Trim()) < 1) || (Convert.ToInt32(txtJumpT.Text.Trim()) > Convert.ToInt32(lbTotalT.Text.Trim())))
        {
            LogBindToday(1);
            lbNowT.Text = "1";

        }
        else
        {
            LogBindToday(Convert.ToInt32(txtJumpT.Text.Trim()));
            lbNowT.Text = txtJumpT.Text.Trim();

        }

    }
    

   

    protected void btnFirstB_Click(object sender, EventArgs e)
    {
        LogBindBefore(1);
        lbNowB.Text = "1";


    }

    protected void btnUpB_Click(object sender, EventArgs e)
    {
        if (lbNowB.Text == "1")
        {
            LogBindBefore(1);
            lbNowB.Text = "1";
        }
        else
        {
            lbNowB.Text = (Convert.ToInt32(lbNowB.Text) - 1).ToString();
            LogBindBefore(Convert.ToInt32(lbNowB.Text));

        }

    }

    protected void btnDrowB_Click(object sender, EventArgs e)
    {
        if (lbNowB.Text == lbTotalB.Text)
        {
            LogBindBefore(1);
            lbNowB.Text = "1";
        }
        else
        {
            lbNowB.Text = (Convert.ToInt32(lbNowB.Text) + 1).ToString();
            LogBindBefore(Convert.ToInt32(lbNowB.Text));
        }

    }

    protected void btnLastB_Click(object sender, EventArgs e)
    {
        LogBindBefore(Convert.ToInt32(lbTotalB.Text));
        lbNowB.Text = lbTotalB.Text;

    }

    protected void btnJumpB_Click(object sender, EventArgs e)
    {
        if ((Convert.ToInt32(txtJumpB.Text.Trim()) < 1) || (Convert.ToInt32(txtJumpB.Text.Trim()) > Convert.ToInt32(lbTotalB.Text.Trim())))
        {
            LogBindBefore(1);
            lbNowB.Text = "1";

        }
        else
        {
            LogBindBefore(Convert.ToInt32(txtJumpB.Text.Trim()));
            lbNowB.Text = txtJumpB.Text.Trim();

        }

    }



    protected void rptToday_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string url= "log.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        if (e.CommandName== "Dellog")                   //访客记录删除
        {
            if (Session["HostQQ"].ToString().Trim() != Session["GuestQQ"].ToString().Trim())
            {
                Response.Write("<script language='javascript'>window.alert('无权操作！');window.location='" + url + "'</script>");
                return;
            }
            else
            {
                string sqldellog = "delete Log where Lid='"+e.CommandArgument.ToString()+"'";
                if(class_Operate.GO(sqldellog)==1)
                    Response.Write("<script language='javascript'>window.alert('访客记录删除成功！');window.location='" + url + "'</script>");
                else
                    Response.Write("<script language='javascript'>window.alert('访客记录删除失败！');window.location='" + url + "'</script>");
            }
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