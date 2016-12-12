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
            IniHeadHost(Request.Cookies["userQQ"].Value);
            if(!IsPostBack)
                Hostbind(1);
            }
            else
                Response.Write("<script language='javascript'>window.alert('身份过期，请重新登录！');window.location='Login.aspx'</script>");
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

            if (!class_Operate.RecordLog(Uqq, Session["GuestQQ"].ToString().Trim()))
                Response.Write("<script>alert('访客记录写入失败！')</script>");//测试用

            return 1;


        }
    }
    private void Hostbind(int currentPage)          //通过火箭飞的形式从上顶端到top，不需要分页
    {
      
        try
        { 
            //通过数据库join命令，查出有关系的好友在分别查询说说日志照片   结果既有自己也有好友
            string sqlsay = "if object_id('tempdb..#tmp1') is not null DROP TABLE  #tmp1 select Relation.RguestQq  into #tmp1 from Relation where RhostQq='"+ Session["HostQQ"].ToString().Trim() + "' and Rstate=1 or  RguestQq='" + Session["HostQQ"].ToString().Trim() + "' select *  from  View_SayFirst right join  #tmp1 on View_SayFirst.Sqq=#tmp1.RguestQq ";
            
            DataTable dtsay = class_Operate.SelectT(sqlsay);

            string sqldiary = "if object_id('tempdb..#tmp1') is not null DROP TABLE  #tmp1 select Relation.RguestQq  into #tmp1 from Relation where RhostQq='"+ Session["HostQQ"].ToString().Trim() + "' and Rstate=1 or  RguestQq='" + Session["HostQQ"].ToString().Trim() + "'  select *  from  View_Diary right join  #tmp1 on View_Diary.Dqq=#tmp1.RguestQq ";
            DataTable dtdiary = class_Operate.SelectT(sqldiary);

            string sqlphoto = "if object_id('tempdb..#tmp1') is not null DROP TABLE  #tmp1 select Relation.RguestQq  into #tmp1 from Relation where RhostQq='" + Session["HostQQ"].ToString().Trim() + "' and Rstate=1 or  RguestQq='" + Session["HostQQ"].ToString().Trim() + "'  select *  from  View_Photo right join  #tmp1 on View_Photo.Pqq=#tmp1.RguestQq ";
            DataTable dtphoto = class_Operate.SelectT(sqlphoto);

            DataTable dt = Class_CreatTable.CreatBigTable(dtsay, dtdiary, dtphoto);


            /*

            PagedDataSource pds = new PagedDataSource();
            pds.AllowPaging = true;
            pds.PageSize = 8;
            pds.DataSource = dt.DefaultView;
            lbTotal.Text = pds.PageCount.ToString();
            if (pds.PageCount > 1)
                divTool.Visible = true;
            pds.CurrentPageIndex = currentPage - 1;
            rptFist.DataSource = pds;
            rptFist.DataBind();
            */
            rptFist.DataSource = dt;
            rptFist.DataBind();

        }
        catch (Exception ex)
        {
            Response.Write(ex);


        }
    }
    
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        Hostbind(1);
        lbNow.Text = "1";


    }

    protected void btnUp_Click(object sender, EventArgs e)
    {
        if (lbNow.Text == "1")
        {
            Hostbind(1);
            lbNow.Text = "1";
        }
        else
        {
            lbNow.Text = (Convert.ToInt32(lbNow.Text) - 1).ToString();
            Hostbind(Convert.ToInt32(lbNow.Text));

        }

    }

    protected void btnDrow_Click(object sender, EventArgs e)
    {
        if (lbNow.Text == lbTotal.Text)
        {
            Hostbind(1);
            lbNow.Text = "1";
        }
        else
        {
            lbNow.Text = (Convert.ToInt32(lbNow.Text) + 1).ToString();
            Hostbind(Convert.ToInt32(lbNow.Text));
        }

    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        Hostbind(Convert.ToInt32(lbTotal.Text));
        lbNow.Text = lbTotal.Text;

    }
    
    protected void btnJump_Click(object sender, EventArgs e)
    {
        if ((Convert.ToInt32(txtJump.Text.Trim()) < 1) || (Convert.ToInt32(txtJump.Text.Trim()) > Convert.ToInt32(lbTotal.Text.Trim())))
        {
            Hostbind(1);
            lbNow.Text = "1";

        }
        else
        {
            Hostbind(Convert.ToInt32(txtJump.Text.Trim()));
            lbNow.Text = txtJump.Text.Trim();

        }

    }




    protected void rptFist_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drvw = (DataRowView)e.Item.DataItem;
            int SID;
            if (drvw["Sid"] == DBNull.Value)
                return;
            else
            {
                SID = Convert.ToInt32(drvw["Sid"]);

                string sql = "select * from View_SaySecond where Csayid='" + SID + "' order by Ctime desc";

                DataTable dt = new DataTable();

                dt = class_Operate.SelectT(sql);

                Repeater rept = (Repeater)e.Item.FindControl("rptSeond");

                // rept = (Repeater)e.Item.FindControl("RptBook");

                rept.DataSource = dt;

                rept.DataBind();
            }


        }
    }

    protected void rptSeond_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        
       
         if (e.CommandName == "imgHeadSecond")
        {
            ImageButton imgbtn = (ImageButton)e.Item.FindControl("imgbtnCommenterHead");
            string jumpUrl = imgbtn.PostBackUrl;
            Response.Write("<script>window.open('" + jumpUrl + "')</script>");
        }
    }
    private bool SubSayComment(string content, string commentQQ, string sayID)
    {

        string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


        string sqlconn = class_Operate.str;

        SqlConnection connection = new SqlConnection(sqlconn);
        connection.Open();
        string sqlstr = "insert into SayComment(Csayid,CguestQQ,Ctime,Ccontent) VALUES  (@sayId,@guestQq,@time,@content)";
        SqlCommand cmd = new SqlCommand(sqlstr, connection);
        cmd.Parameters.Clear();



        cmd.Parameters.Add("@guestQq", SqlDbType.VarChar);
        cmd.Parameters["@guestQq"].Value = commentQQ;

        cmd.Parameters.Add("@sayId", SqlDbType.Int);
        cmd.Parameters["@sayId"].Value = sayID;

        cmd.Parameters.Add("@time", SqlDbType.VarChar);
        cmd.Parameters["@time"].Value = nowTime;

        cmd.Parameters.Add("@content", SqlDbType.NVarChar);
        cmd.Parameters["@content"].Value = content;



        int i = cmd.ExecuteNonQuery();
        connection.Close();
        if (i > 0)
            return true;
        else
            return false;


    }
    private bool PublishNewSay(string hostQQ, string newContent)
    {

        string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string sqlconn = class_Operate.str;

        SqlConnection connection = new SqlConnection(sqlconn);
        connection.Open();
        string sqlstr = "insert into Say(Sqq,SpublishTime,SgoodCounts,Scontent) VALUES  (@Sqq,@SpublishTime,@SgoodCounts,@Scontent)";
        SqlCommand cmd = new SqlCommand(sqlstr, connection);
        cmd.Parameters.Clear();



        cmd.Parameters.Add("@Sqq", SqlDbType.VarChar);
        cmd.Parameters["@Sqq"].Value = hostQQ;

        cmd.Parameters.Add("@SpublishTime", SqlDbType.VarChar);
        cmd.Parameters["@SpublishTime"].Value = nowTime;

        cmd.Parameters.Add("@SgoodCounts", SqlDbType.Int);
        cmd.Parameters["@SgoodCounts"].Value = 0;

        cmd.Parameters.Add("@Scontent", SqlDbType.NVarChar);
        cmd.Parameters["@Scontent"].Value = newContent;



        int i = cmd.ExecuteNonQuery();
        connection.Close();
        if (i > 0)
            return true;
        else
            return false;


    }
    protected void rptFist_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string url = "dynamic.aspx?";
        if (e.CommandName == "FirstReply")
        {
            try
            {

                TextBox txtComment_cs = (TextBox)e.Item.FindControl("txtComment");
                txtComment_cs.Visible = true;
                Button btnComment_cs = (Button)e.Item.FindControl("btnSaySub");
                btnComment_cs.Visible = true;
            }
            catch (Exception ex)
            {
                Response.Write("抱歉，回复失败请刷新" + ex);
            }
        }
        else if (e.CommandName == "FirstSubSay")
        {
            TextBox txtComment_cs = (TextBox)e.Item.FindControl("txtComment");
            if (txtComment_cs.Text.Trim() != "")
            {


                if (!SubSayComment(txtComment_cs.Text.Trim(), Session["GuestQQ"].ToString().Trim(), e.CommandArgument.ToString()))   //写入评论表
                    Response.Write("<script language='javascript'>window.alert('抱歉评论失败，请重新评论');window.location='" + url + "'</script>");
                else
                    Response.Write("<script language='javascript'>window.alert('评论成功！');window.location='" + url + "'</script>");

            }
            else
            {
                Response.Write("<script language='javascript'>window.alert('留几句话呗，空着多不好~');window.location='" + url + "'</script>");
            }
        }
        
        else if (e.CommandName == "FirstGood")
        {
            string sqladdgood = "update Say set SgoodCounts+=1 where Sid='" + e.CommandArgument.ToString() + "'";
            if (class_Operate.GO(sqladdgood) == 1)
                Response.Write("<script language='javascript'>window.alert('点赞成功！');window.location='" + url + "'</script>");
            else
                Response.Write("<script language='javascript'>window.alert('点赞失败！');window.location='" + url + "'</script>");


        }
        else if (e.CommandName == "FirstTrans")
        {
            if (Session["GuestQQ"].ToString().Trim() == Session["HostQQ"].ToString().Trim())
            {
                Response.Write("<script language='javascript'>window.alert('自己不能转载自己哦！');window.location='" + url + "''</script>");

            }
            else
            {
                string sqltranscontent = "select Scontent from Say where Sid='" + e.CommandArgument.ToString() + "'";
                string tranScontent = class_Operate.SelectHead(sqltranscontent);

                if (PublishNewSay(Session["GuestQQ"].ToString().Trim(), tranScontent))

                    Response.Write("<script language='javascript'>window.alert('转载成功！');window.location='" + url + "''</script>");
                else
                    Response.Write("<script language='javascript'>window.alert('转载失败！');window.location='" + url + "'</script>");

            }

        }
        else if (e.CommandName == "imgHeadFirst")
        {
            ImageButton imgbtn = (ImageButton)e.Item.FindControl("imgbtnHostHead");
            string jumpUrl = imgbtn.PostBackUrl;
            Response.Write("<script>window.open('" + jumpUrl + "')</script>");

        }
        else if (e.CommandName == "jumpPhoto")                  //放大图片跳转页面
        {
            string jumpurl = "photodetial.aspx?pid=" + e.CommandArgument.ToString();
            Response.Write("<script language='javascript'>window.open('" + jumpurl + "', 'newwindow', 'height=800, width=1050, top='+Math.round((window.screen.height-800)/2)+',left='+Math.round((window.screen.width-1050)/2)+', toolbar=no,menubar = no, scrollbars = no, resizable = no, location = no, status = no')</script>");
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