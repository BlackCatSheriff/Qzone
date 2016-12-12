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

        if (Request.QueryString["uqq"] == null)                                 //验证地址栏传值
            {
            
               Session["GuestQQ"] = Request.Cookies["userQQ"].Value;            //分配host和guest 
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
          

        if(!IsPostBack)
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
               
                HostbindSay(1);
            }
            else
                Response.Write("<script language='javascript'>window.alert('身份过期，请重新登录！');window.location='Login.aspx'</script>");

        }
     

    }



    private int IniHeadHost(string Uqq)                 //初始化界面控件属性
    {
        aHome.HRef= "home.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        aSay.HRef= "comment.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        aDaily.HRef= "diary.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        aAlbum.HRef= "album.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        aMessage.HRef= "message.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        aLog.HRef= "log.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
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
            if(Uqq!= Session["GuestQQ"].ToString().Trim())
                divNewSay.Visible = false;
           lbtGuestUsername.Text = HttpUtility.UrlDecode(Request.Cookies["userNick"].Value,Encoding.GetEncoding("utf-8"));
            lblHostQnike.Text = dt.Rows[0][0].ToString();
            lblHostUsername.Text = dt.Rows[0][1].ToString();
            lblHostSignature.Text = dt.Rows[0][2].ToString();
            lblHostGrade.Text = "空间等级:"+dt.Rows[0][3].ToString();
            imgBtnHostHead.ImageUrl = dt.Rows[0][4].ToString();

            if (!class_Operate.RecordLog(Uqq, Session["GuestQQ"].ToString().Trim()))
                Response.Write("<script>alert('访客记录写入失败！')</script>");//测试用

            return 1;


        }
     }

    private void  HostbindSay (int currentPage)         //绑定界面数据
    {
       
        try
        {
            string sql = "select * from View_SayFirst where sqq='"+ Session["HostQQ"].ToString().Trim() + "' order by SpublishTime desc";
            DataTable dt = class_Operate.SelectT(sql);
          
            PagedDataSource pds = new PagedDataSource();                //分页
            pds.AllowPaging = true;
            pds.PageSize = 10;
            pds.DataSource = dt.DefaultView;
            lbTotal.Text = pds.PageCount.ToString();
            if (pds.PageCount > 1)
                divTool.Visible = true;
            pds.CurrentPageIndex = currentPage - 1;
            rptFist.DataSource = pds;
            rptFist.DataBind();

        }
        catch(Exception ex)
        {
            Response.Write(ex);


        }
    }    


    protected void btnFirst_Click(object sender, EventArgs e)
    {
        HostbindSay(1);
        lbNow.Text = "1";


    }

    protected void btnUp_Click(object sender, EventArgs e)
    {
        if (lbNow.Text == "1")
        {
            HostbindSay(1);
            lbNow.Text = "1";
        }
        else
        {
            lbNow.Text = (Convert.ToInt32(lbNow.Text) - 1).ToString();
            HostbindSay(Convert.ToInt32(lbNow.Text));

        }

    }

    protected void btnDrow_Click(object sender, EventArgs e)
    {
        if (lbNow.Text == lbTotal.Text)
        {
            HostbindSay(1);
            lbNow.Text = "1";
        }
        else
        {
            lbNow.Text = (Convert.ToInt32(lbNow.Text) + 1).ToString();
            HostbindSay(Convert.ToInt32(lbNow.Text));
        }

    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        HostbindSay(Convert.ToInt32(lbTotal.Text));
        lbNow.Text = lbTotal.Text;

    }

    protected void btnJump_Click(object sender, EventArgs e)
    {
        if ((Convert.ToInt32(txtJump.Text.Trim()) < 1) || (Convert.ToInt32(txtJump.Text.Trim()) > Convert.ToInt32(lbTotal.Text.Trim())))
        {
            HostbindSay(1);
            lbNow.Text = "1";

        }
        else
        {
            HostbindSay(Convert.ToInt32(txtJump.Text.Trim()));
            lbNow.Text = txtJump.Text.Trim();

        }

    }

    protected void rptFist_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string url = "comment.aspx?uqq=" + Session["HostQQ"].ToString().Trim();    //页面刷新网址
        if (e.CommandName== "FirstReply")        //说说回复
        {
            try
            {
                
                TextBox txtComment_cs = (TextBox)e.Item.FindControl("txtComment");
                txtComment_cs.Visible = true;
                Button btnComment_cs = (Button)e.Item.FindControl("btnSaySub");
                btnComment_cs.Visible = true;
            }
            catch(Exception ex)
            {
                Response.Write("抱歉，回复失败请刷新"+ex);
            }
        }
        else if(e.CommandName== "FirstSubSay")        //评论说说
        {
            TextBox txtComment_cs = (TextBox)e.Item.FindControl("txtComment");
            if(txtComment_cs.Text.Trim()!= "")
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
        else if(e.CommandName== "DelFirst")    //删除说说
        {

            if (Session["HostQQ"].ToString().Trim() != Session["GuestQQ"].ToString().Trim())            //判断是否有删除权限
            {
                Response.Write("<script language='javascript'>window.alert('无权操作！');window.location='" + url + "'</script>");
                return;
            }

            string sqlDelSay = "delete from Say where Sid='"+e.CommandArgument.ToString()+"'";        //删除sql
            if(class_Operate.GO(sqlDelSay) ==1)
                Response.Write("<script language='javascript'>window.alert('说说删除成功！');window.location='" + url + "'</script>");
            else
                Response.Write("<script language='javascript'>window.alert('说说删除失败！');window.location='" + url + "'</script>");


        }
        else if(e.CommandName== "FirstGood")                                                        //点赞功能
        {
            string sqladdgood = "update Say set SgoodCounts+=1 where Sid='" + e.CommandArgument.ToString() + "'";
            if(class_Operate.GO(sqladdgood)==1)
                Response.Write("<script language='javascript'>window.alert('点赞成功！');window.location='" + url + "'</script>");
            else
                Response.Write("<script language='javascript'>window.alert('点赞失败！');window.location='" + url + "'</script>");


        }
        else if(e.CommandName== "FirstTrans")                                                   //说说转发功能
        {
            if(Session["GuestQQ"].ToString().Trim()== Session["HostQQ"].ToString().Trim())      //限制自己转载自己
            {
                Response.Write("<script language='javascript'>window.alert('自己不能转载自己哦！');window.location='" + url + "''</script>");

            }
            else
            {
                string sqltranscontent = "select Scontent from Say where Sid='" + e.CommandArgument.ToString() + "'";     //找出要转发说说的内容
                string tranScontent = class_Operate.SelectHead(sqltranscontent);

                if (PublishNewSay(Session["GuestQQ"].ToString().Trim(), tranScontent))                          //把要转发的说说写入转发者的空间中

                    Response.Write("<script language='javascript'>window.alert('转载成功！');window.location='" + url + "''</script>");
                else
                    Response.Write("<script language='javascript'>window.alert('转载失败！');window.location='" + url + "'</script>");

            }

        }
        else if(e.CommandName=="imgHeadFirst")                                          //跳转查看这个头像好友的空间
        {
            ImageButton imgbtn = (ImageButton)e.Item.FindControl("imgbtnHostHead");
            string jumpUrl = imgbtn.PostBackUrl;
            Response.Write("<script>window.open('" + jumpUrl + "')</script>");

        }
       
        

    }

   private bool SubSayComment(string content,string  commentQQ, string sayID)
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



    protected void rptFist_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
       
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drvw = (DataRowView)e.Item.DataItem;

            int SID  = Convert.ToInt32(drvw["Sid"]);

            string sql = "select * from View_SaySecond where Csayid='"+SID+"' order by Ctime desc";

            DataTable dt = new DataTable();

            dt = class_Operate.SelectT(sql);

            Repeater rept = (Repeater)e.Item.FindControl("rptSeond");

            // rept = (Repeater)e.Item.FindControl("RptBook");

            rept.DataSource = dt;

            rept.DataBind();

        }
    }

    protected void rptSeond_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string url = "comment.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        if (e.CommandName== "DelSecond")                                      //评论删除
        {
            // Response.Write("<script language='javascript'>window.alert('Hahaha~');</script>");
            if (Session["HostQQ"].ToString().Trim() != Session["GuestQQ"].ToString().Trim())   //判断是否有删除权限
            {
                Response.Write("<script language='javascript'>window.alert('无权操作！');window.location='" + url + "'</script>");
                return;
            }
            string sqlDelSay = "delete from SayComment where Cid='"+e.CommandArgument.ToString()+"'";   //执行删除操作
            if (class_Operate.GO(sqlDelSay) == 1)
                Response.Write("<script language='javascript'>window.alert('评论删除成功！');window.location='" + url + "'</script>");
            else
                Response.Write("<script language='javascript'>window.alert('评论删除失败！');window.location='" + url + "'</script>");

        }
        else if(e.CommandName== "imgHeadSecond")                                    //查看空间
            {
              ImageButton imgbtn = (ImageButton)e.Item.FindControl("imgbtnCommenterHead");
            string jumpUrl = imgbtn.PostBackUrl;
            Response.Write("<script>window.open('" + jumpUrl + "')</script>");
        }
    }

    protected void btnSubSay_Click(object sender, EventArgs e)
    {
        string url = "comment.aspx?uqq=" + Session["HostQQ"].ToString().Trim();                 //刷新网址
        if (txtNewSay.Text.Trim ()=="")
        {
            Response.Write("<script language='javascript'>window.alert('留几句话呗，空着多不好~');</script>");
        }
        else
        {
           
            if ( PublishNewSay(Session["HostQQ"].ToString().Trim(), txtNewSay.Text.Trim()))
            {
                class_Operate.QqGrade(Session["HostQQ"].ToString().Trim(), 1);   //等级增加
                
                Response.Write("<script language='javascript'>window.alert('说说发表成功！');window.location='"+url+"'</script>");
            }
                
                else
                    Response.Write("<script language='javascript'>window.alert('说说发表失败！');window.location='" + url + "'</script>");


        }
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
        Response.Write("<script language='javascript'>window.location='"+imgbutFriends.PostBackUrl+"'</script>");
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