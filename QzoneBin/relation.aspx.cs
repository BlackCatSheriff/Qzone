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
    protected string COUNT;
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
            //=COUNT
            string sqlcount = "select count(*) from Relation where RhostQq='" + Session["HostQQ"].ToString().Trim() + "' and Rstate='0'";
            COUNT = class_Operate.SelectHead (sqlcount);
            IniHeadHost(Session["HostQQ"].ToString().Trim());
        }
        catch
        {
            string sqlcount = "select count(*) from Relation where RhostQq='" + Session["HostQQ"].ToString().Trim() + "' and Rstate='0'";
            COUNT = class_Operate.SelectHead(sqlcount);
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

                Followbind(1);
                Recommandbind(1);

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
            if (Uqq != Session["GuestQQ"].ToString().Trim())  //验证是否自己空间  错误处理
            {
                Response.Write("<script language='javascript'>window.alert('别淘气！');window.close();window.open('','_self');</script>");
                return 0;

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

    private void Followbind(int currentPage)
    {//关注好友列表

        try
        {
            string sqlFollow = "select * from View_Relationship where RhostQq='" + Session["HostQQ"].ToString().Trim() + "' and Rstate='1' ";               //查询关注好友语句
            
            DataTable dt = class_Operate.SelectT(sqlFollow);

            PagedDataSource pds = new PagedDataSource();
            pds.AllowPaging = true;
            pds.PageSize = 10;
            pds.DataSource = dt.DefaultView;
            lbTotalF.Text = pds.PageCount.ToString();
            if (pds.PageCount > 1)
                divToolF.Visible = true;
            pds.CurrentPageIndex = currentPage - 1;
            rptFriendsList.DataSource = pds;
            rptFriendsList.DataBind();

        }
        catch (Exception ex)
        {
            Response.Write(ex);
            
        }
    }
    private void Recommandbind(int currentPage)
    {
        //好友推荐
        try
        {
            
            string sqlrecommand  = "if object_id('tempdb..#tmp') is not null DROP TABLE  #tmp select Users.Uqq into #tmp from  Users except select RguestQq  from Relation where Rstate='1' and RhostQq='" + Session["HostQQ"].ToString().Trim() + "' select Users.Uqq,Unick,Uheadimg  from  Users right join  #tmp on Users.Uqq=#tmp.Uqq  where Users.Uqq!='" + Session["HostQQ"].ToString().Trim() + "'";



            DataTable dt = class_Operate.SelectT(sqlrecommand);

            PagedDataSource pds = new PagedDataSource();
            pds.AllowPaging = true;
            pds.PageSize = 10;
            pds.DataSource = dt.DefaultView;
            lbTotalC.Text = pds.PageCount.ToString();
            if (pds.PageCount > 1)
                divToolC.Visible = true;
            pds.CurrentPageIndex = currentPage - 1;
            rptRecommand.DataSource = pds;
            rptRecommand.DataBind();

        }
        catch (Exception ex)
        {
            Response.Write(ex);

        }
    }

    private void Searchbind (int currentPage)
    {//模糊搜索好友QQ号

        try
        {
           
            SqlConnection scon = new SqlConnection(class_Operate.str);

            scon.Open();

            SqlCommand selectCmd = new SqlCommand();

            selectCmd.CommandText = "if object_id('tempdb..#tmp') is not null DROP TABLE  #tmp select Users.Uqq into #tmp from  Users except select RguestQq  from Relation where Rstate='1' if object_id('tempdb..#tmp1') is not null DROP TABLE  #tmp1 select Users.Uqq,Unick,Uheadimg into #tmp1 from  Users right join  #tmp on Users.Uqq=#tmp.Uqq  where Users.Uqq!=@hostqq select #tmp1.Uqq,#tmp1.Unick,#tmp1.Uheadimg from #tmp1 where #tmp1.Uqq like @searchqq";

            selectCmd.Parameters.Add("@hostqq", SqlDbType.VarChar, 10);//sql指令中存在一个参数，叫@sn,它的类型是字符型，字节长度是10个

            selectCmd.Parameters["@hostqq"].Value = Session["HostQQ"].ToString().Trim();

            selectCmd.Parameters.Add("@searchqq", SqlDbType.VarChar, 10);

            selectCmd.Parameters["@searchqq"].Value = "%"+txtSearchFrd.Text.Trim()+"%";


            selectCmd.Connection = scon;

            SqlDataAdapter sda = new SqlDataAdapter();

            sda.SelectCommand = selectCmd;

            DataTable dt = new DataTable();

            sda.Fill(dt);
            
            
            PagedDataSource pds = new PagedDataSource();
            pds.AllowPaging = true;
            pds.PageSize = 10;
            pds.DataSource = dt.DefaultView;
            lbTotalS.Text = pds.PageCount.ToString();
            if (pds.PageCount > 1)
                divToolS.Visible = true;
            pds.CurrentPageIndex = currentPage - 1;
            rptSearchList.DataSource = pds;
            rptSearchList.DataBind();

        }
        catch (Exception ex)
        {
            Response.Write(ex);

        }
    }


    protected void btnFirstF_Click(object sender, EventArgs e)
    {
        Followbind(1);
        lbNowF.Text = "1";


    }

    protected void btnUpF_Click(object sender, EventArgs e)
    {
        if (lbNowF.Text == "1")
        {
            Followbind(1);
            lbNowF.Text = "1";
        }
        else
        {
            lbNowF.Text = (Convert.ToInt32(lbNowF.Text) - 1).ToString();
            Followbind(Convert.ToInt32(lbNowF.Text));

        }

    }

    protected void btnDrowF_Click(object sender, EventArgs e)
    {
        if (lbNowF.Text == lbTotalF.Text)
        {
            Followbind(1);
            lbNowF.Text = "1";
        }
        else
        {
            lbNowF.Text = (Convert.ToInt32(lbNowF.Text) + 1).ToString();
            Followbind(Convert.ToInt32(lbNowF.Text));
        }

    }

    protected void btnLastF_Click(object sender, EventArgs e)
    {
        Followbind(Convert.ToInt32(lbTotalF.Text));
        lbNowF.Text = lbTotalF.Text;

    }
    
    protected void btnJumpF_Click(object sender, EventArgs e)
    {
        if ((Convert.ToInt32(txtJumpF.Text.Trim()) < 1) || (Convert.ToInt32(txtJumpF.Text.Trim()) > Convert.ToInt32(lbTotalF.Text.Trim())))
        {
            Followbind(1);
            lbNowF.Text = "1";

        }
        else
        {
            Followbind(Convert.ToInt32(txtJumpF.Text.Trim()));
            lbNowF.Text = txtJumpF.Text.Trim();

        }

    }
    //---------------------
    protected void btnFirstS_Click(object sender, EventArgs e)
    {
        Searchbind(1);
        lbNowS.Text = "1";


    }

    protected void btnUpS_Click(object sender, EventArgs e)
    {
        if (lbNowS.Text == "1")
        {
            Searchbind(1);
            lbNowS.Text = "1";
        }
        else
        {
            lbNowS.Text = (Convert.ToInt32(lbNowS.Text) - 1).ToString();
            Searchbind(Convert.ToInt32(lbNowS.Text));

        }

    }

    protected void btnDrowS_Click(object sender, EventArgs e)
    {
        if (lbNowS.Text == lbTotalS.Text)
        {
            Searchbind(1);
            lbNowS.Text = "1";
        }
        else
        {
            lbNowS.Text = (Convert.ToInt32(lbNowS.Text) + 1).ToString();
            Searchbind(Convert.ToInt32(lbNowS.Text));
        }

    }

    protected void btnLastS_Click(object sender, EventArgs e)
    {
        Searchbind(Convert.ToInt32(lbTotalS.Text));
        lbNowS.Text = lbTotalS.Text;

    }

    protected void btnJumpS_Click(object sender, EventArgs e)
    {
        if ((Convert.ToInt32(txtJumpS.Text.Trim()) < 1) || (Convert.ToInt32(txtJumpS.Text.Trim()) > Convert.ToInt32(lbTotalS.Text.Trim())))
        {
            Searchbind(1);
            lbNowS.Text = "1";

        }
        else
        {
            Searchbind(Convert.ToInt32(txtJumpS.Text.Trim()));
            lbNowS.Text = txtJumpS.Text.Trim();

        }

    }
    //------------------------
    protected void btnFirstC_Click(object sender, EventArgs e)
    {
        Recommandbind(1);
        lbNowC.Text = "1";


    }

    protected void btnUpC_Click(object sender, EventArgs e)
    {
        if (lbNowC.Text == "1")
        {
            Recommandbind(1);
            lbNowC.Text = "1";
        }
        else
        {
            lbNowC.Text = (Convert.ToInt32(lbNowC.Text) - 1).ToString();
            Recommandbind(Convert.ToInt32(lbNowC.Text));

        }

    }

    protected void btnDrowC_Click(object sender, EventArgs e)
    {
        if (lbNowC.Text == lbTotalC.Text)
        {
            Recommandbind(1);
            lbNowC.Text = "1";
        }
        else
        {
            lbNowC.Text = (Convert.ToInt32(lbNowC.Text) + 1).ToString();
            Recommandbind(Convert.ToInt32(lbNowC.Text));
        }

    }

    protected void btnLastC_Click(object sender, EventArgs e)
    {
        Recommandbind(Convert.ToInt32(lbTotalC.Text));
        lbNowC.Text = lbTotalC.Text;

    }

    protected void btnJumpC_Click(object sender, EventArgs e)
    {
        if ((Convert.ToInt32(txtJumpC.Text.Trim()) < 1) || (Convert.ToInt32(txtJumpC.Text.Trim()) > Convert.ToInt32(lbTotalC.Text.Trim())))
        {
            Recommandbind(1);
            lbNowC.Text = "1";

        }
        else
        {
            Recommandbind(Convert.ToInt32(txtJumpC.Text.Trim()));
            lbNowC.Text = txtJumpC.Text.Trim();

        }

    }



    protected void btnSearchQQ_Click(object sender, EventArgs e)
    {
       
            Searchbind(1);
        
    }

    protected void rptFriendsList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if(e.CommandName== "Unfollow")    //取消关注
        {
            string sqlundollow = "delete from  Relation  where RhostQq='" + Session["HostQQ"].ToString().Trim() + "' and RguestQq='" + e.CommandArgument.ToString() + "' and Rstate='1'";           //从数据库中删除关系
            if(class_Operate.GO(sqlundollow)==1)
                Response.Write("<script language='javascript'>window.alert('成功取消关注！');window.location='relation.aspx'</script>");
            else
                Response.Write("<script language='javascript'>window.alert('操作失败，请重试！');window.location='relation.aspx'</script>");

        }
        if(e.CommandName== "jumphome")                      //跳转目标好友空间
        {
            ImageButton imgbtn = (ImageButton)e.Item.FindControl("imgbtnFriend");
            Response.Write("<script language='javascript'>window.open('"+imgbtn.PostBackUrl+"', 'newwindow', 'height=400, width=800, top='+Math.round((window.screen.height-400)/2)+',left='+Math.round((window.screen.width-800)/2)+', toolbar=no,menubar = no, scrollbars = no, resizable = no, location = no, status = no')</script>");
        }
    }

    protected void rptSearchList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if(e.CommandName== "Follow")  // 关注好友   发起人直接与被关注好友建立关注关系，同时被关注人也将受到关注人的关注消息   数据库方面：发起一次关注好友的请求，创建两条数据，一条为当前发起人和被关好友的关系建立，两一条为被关注还有和发起人的关系建立
        {
            //string sqlfollow1 = "update Relation set Rstate=0 where RhostQq='" + Session["HostQQ"].ToString().Trim() + "' and RguestQq='" + e.CommandArgument.ToString() + "' ";

            string sqlfollow1 = "insert into Relation(RhostQq,RguestQq,Rstate) values('"+ Session["HostQQ"].ToString().Trim() + "','"+e.CommandArgument.ToString()+"','1')";      //建立发起人和关注人的关系

            string sqlfollow2= "insert into Relation(RguestQq,RhostQq,Rstate) values('" + Session["HostQQ"].ToString().Trim() + "','" + e.CommandArgument.ToString() + "','0')";    //建立被关注人和发起人的关系 
            if (class_Operate.GO(sqlfollow1) == 1 && class_Operate.GO(sqlfollow2)==1)
                Response.Write("<script language='javascript'>window.alert('关注成功！');window.location='relation.aspx'</script>");
            else
                Response.Write("<script language='javascript'>window.alert('关注失败，请重试！');window.location='relation.aspx'</script>");

        }
    }

    protected void rptRecommand_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Follow")              //关注功能
        {
           
            string sqlfollow1 = "insert into Relation(RhostQq,RguestQq,Rstate) values('" + Session["HostQQ"].ToString().Trim() + "','" + e.CommandArgument.ToString() + "','1')";

            string sqlfollow2 = "insert into Relation(RguestQq,RhostQq,Rstate) values('" + Session["HostQQ"].ToString().Trim() + "','" + e.CommandArgument.ToString() + "','0')";
            if (class_Operate.GO(sqlfollow1) == 1 && class_Operate.GO(sqlfollow2) == 1)
                Response.Write("<script language='javascript'>window.alert('关注成功！');window.location='relation.aspx'</script>");
            else
                Response.Write("<script language='javascript'>window.alert('关注失败，请重试！');window.location='relation.aspx'</script>");

        }
    }

    protected void lbtnjump_Click(object sender, EventArgs e)
    {

        Response.Write("<script language='javascript'>window.open('application.aspx', 'newwindow', 'height=500, width=800, top='+Math.round((window.screen.height-500)/2)+',left='+Math.round((window.screen.width-800)/2)+', toolbar=no,menubar = no, scrollbars = no, resizable = no, location = no, status = no')</script>");
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