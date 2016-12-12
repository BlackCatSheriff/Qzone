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
        if (Request.QueryString["uqq"] == null)                                 //获取地址栏的uqq  
        {                                                                       //如果不存在就规定为当前登录主账号

            Session["GuestQQ"] = Request.Cookies["userQQ"].Value;
            Session["HostQQ"] = Request.Cookies["userQQ"].Value;

        }
        else
        {
            Session["HostQQ"] = Request.QueryString["uqq"].ToString().Trim();   //如果存在，主账号为guest， 访问空间为host
            Session["GuestQQ"] = Request.Cookies["userQQ"].Value;

        }
        try     //防止uqq正常传值遭到破坏，如果破坏刷新访客主页
        {
            IniHeadHost(Session["HostQQ"].ToString().Trim());                    //初始化页面控件属性
        }
        catch
        {
            IniHeadHost(Request.Cookies["userQQ"].Value);
            Session["HostQQ"] = Request.Cookies["userQQ"].Value;
            Response.Write(Session["HostQQ"].ToString());
        }


        if (!IsPostBack)
        {

            if (Request.Cookies["userQQ"] != null && Request.Cookies["passWord"] != null)             //判断是否用户名、密码匹配
            {


                //判断qq和密码是否匹配
                int judge = class_Operate.isRght(Request.Cookies["userQQ"].Value, Request.Cookies["passWord"].Value);
                if (judge != 1)

                {
                    Response.Write("<script language='javascript'>window.alert('身份过期，请重新登录！');window.location='Login.aspx'</script>");
                    return;
                }

                Hostbind(1);
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

            if (Uqq != Session["GuestQQ"].ToString().Trim())  //验证是否自己空间
            {
                divquickSay.Visible = false;
                btndaka.Visible = false;

            }





            LblAlldak.Text = "已签到：" + Daka(Uqq) + "天";     //签到功能

            if (isTodaySign(Uqq))
            {
                btndaka.Enabled = true;
            }

            else
            {
                btndaka.Text = "已签到";
                btndaka.Enabled = false;
            }
                





            if (!class_Operate.RecordLog(Uqq, Session["GuestQQ"].ToString().Trim()))
                Response.Write("<script>alert('访客记录写入失败！')</script>");//测试用

            return 1;


        }
    }
    private void Hostbind(int currentPage)
    {
      
        try
        { //某个人空间全部动态
            string sqlsay = "select * from View_SayFirst where Sqq='"+ Session["HostQQ"].ToString().Trim() + "'";
            DataTable dtsay = class_Operate.SelectT(sqlsay);

            string sqldiary = "select * from View_Diary where Dqq='"+ Session["HostQQ"].ToString().Trim() + "'";
            DataTable dtdiary = class_Operate.SelectT(sqldiary);

            string sqlphoto = "select * from View_Photo where Pqq='"+ Session["HostQQ"].ToString().Trim() + "'";
            DataTable dtphoto = class_Operate.SelectT(sqlphoto);

            DataTable dt = Class_CreatTable.CreatBigTable(dtsay, dtdiary, dtphoto);    //动态生成新表进行查询

          


            PagedDataSource pds = new PagedDataSource();                            //使用分页功能
            pds.AllowPaging = true;
            pds.PageSize = 20;
            pds.DataSource = dt.DefaultView;
            lbTotal.Text = pds.PageCount.ToString();
            if (pds.PageCount > 1)
                divTool.Visible = true;
            pds.CurrentPageIndex = currentPage - 1;
            rptFist.DataSource = pds;
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




    protected void rptFist_ItemDataBound(object sender, RepeaterItemEventArgs e)    //内层repeater 绑定。通过外层repeater的绑定事件来激发，外层repeater每行绑定完，都会执行这个bind事件进行内层绑定
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drvw = (DataRowView)e.Item.DataItem;                    //初始化一个具有外层repeater结构的行对象，用来获取行中某一列的值
            int SID;
            if (drvw["Sid"] == DBNull.Value)                                //获取外层repeater本行的唯一标识，共内层repeater使用
                return;
            else
            {
                SID = Convert.ToInt32(drvw["Sid"]);

                string sql = "select * from View_SaySecond where Csayid='" + SID + "' order by Ctime desc";

                DataTable dt = new DataTable();

                dt = class_Operate.SelectT(sql);

                Repeater rept = (Repeater)e.Item.FindControl("rptSeond");       //不能跨过外层repeater直接操作内层repeater，因此需要通过在外层repeater中通过Find命令找到内层repeater来进行操作

                

                rept.DataSource = dt;

                rept.DataBind();
            }


        }
    }

    protected void rptSeond_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string url = "home.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
       
         if (e.CommandName == "imgHeadSecond")                                      //点击头像跳转到目标界面
        {
            ImageButton imgbtn = (ImageButton)e.Item.FindControl("imgbtnCommenterHead");
            string jumpUrl = imgbtn.PostBackUrl;
            Response.Write("<script>window.open('" + jumpUrl + "')</script>");
        }
         
    }
    private bool SubSayComment(string content, string commentQQ, string sayID)    //发送说说评论，参数content 评论内容，commentQQ 评论者QQ,sayID 评论所属的说说唯一标识
    {

        string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");              //格式化时间，方便排序


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
    private bool PublishNewSay(string hostQQ, string newContent)                  //发送说说，参数hostQQ 发送者QQ，newContent 发送内容
    {

        string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");              //格式化日期，方便排序

        string sqlconn = class_Operate.str;

        SqlConnection connection = new SqlConnection(sqlconn);
        connection.Open();
        string sqlstr = "insert into Say(Sqq,SpublishTime,SgoodCounts,Scontent) VALUES  (@Sqq,@SpublishTime,@SgoodCounts,@Scontent)";
        SqlCommand cmd = new SqlCommand(sqlstr, connection);
        cmd.Parameters.Clear();                                                 //清空查询参数



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
        string url = "home.aspx?uqq=" + Session["HostQQ"].ToString().Trim();    //刷新网站的网址
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
        else if(e.CommandName== "jumpPhoto")
        {
            string jumpurl = "photodetial.aspx?pid="+e.CommandArgument.ToString();
            Response.Write("<script language='javascript'>window.open('"+jumpurl+ "', 'newwindow', 'height=800, width=1050, top='+Math.round((window.screen.height-800)/2)+',left='+Math.round((window.screen.width-1050)/2)+', toolbar=no,menubar = no, scrollbars = no, resizable = no, location = no, status = no')</script>");
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

    protected void btnSubSay_Click(object sender, EventArgs e)
    {

        string url = "home.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        if (txtNewSay.Text.Trim() == "")
        {
            Response.Write("<script language='javascript'>window.alert('留几句话呗，空着多不好~');</script>");
        }
        else
        {

            if (PublishNewSay(Session["HostQQ"].ToString().Trim(), txtNewSay.Text.Trim()))
            {
                class_Operate.QqGrade(Session["HostQQ"].ToString().Trim(), 1);   //等级增加

                Response.Write("<script language='javascript'>window.alert('说说发表成功！');window.location='" + url + "'</script>");
            }

            else
                Response.Write("<script language='javascript'>window.alert('说说发表失败！');window.location='" + url + "'</script>");


        }



    }

    protected void btndaka_Click(object sender, EventArgs e)
    {

        if (SignNow(Session["HostQQ"].ToString().Trim()))
        {
            btndaka.Text = "已签到";
            btndaka.Enabled = false;
            Response.Write("<script language='javascript'>alert('签到成功！')window.location='" + aAlbum.HRef + "'</script>");
        }

    }

    private  string Daka(string qq)                  //查询qq的已经签到天数
    {
        string sqldakad = "select Usignin from Users where Uqq='" + qq + "'";
        return class_Operate. SelectHead(sqldakad);
    }
    private  bool isTodaySign(string qq)            //检查当前账号今天是否已经签到,返回当日是否可以签到
    {
        string nowTime = DateTime.Now.ToString("yyyy-MM-dd");           //格式化时间，用来和当前时间进行对比，判断今日是否签到过
        string sqlis = "select UsignNow from Users where Uqq='" + qq + "'";         //查询最后一次签到时间
        string sqlnow =class_Operate. SelectHead(sqlis);
        if (nowTime == sqlnow)                                          //是否相同
            return false;
        else
            return true;
    }
    private  bool SignNow(string qq)                //签到功能，返回是否签到成功
    {
        string nowTime = DateTime.Now.ToString("yyyy-MM-dd");           //格式化时间存入数据库，方便下一次比较
        string sqluptae = "update Users set UsignNow='" + nowTime + "' ,Usignin+=1 where Uqq='" + qq + "'";         //更新数据库签到时间
        if (class_Operate. GO(sqluptae) == 1)       
            return true;
        else
            return false;

    }
}