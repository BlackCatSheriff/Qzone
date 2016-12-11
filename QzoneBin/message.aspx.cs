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

                HostbindMessage(1);
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
            if(Uqq!= Session["GuestQQ"].ToString().Trim())
            {
                bntHostMess.Visible = false;
                bntSubmitMess.Visible = false;

            }
                
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

    private void  HostbindMessage  (int currentPage)
    {
       
        try
        {
            
            string sql= "select * from View_MessageFirst where Mqq = '"+Session["HostQQ"].ToString().Trim()+"' order by MpublishTime desc";
            string sqlhope = "select Uhope from Users where Uqq='" + Session["HostQQ"].ToString().Trim() + "'";
            txtHosthope.Text = class_Operate.SelectHead(sqlhope);

            DataTable dt = class_Operate.SelectT(sql);
          
            PagedDataSource pds = new PagedDataSource();
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
        HostbindMessage(1);
        lbNow.Text = "1";


    }

    protected void btnUp_Click(object sender, EventArgs e)
    {
        if (lbNow.Text == "1")
        {
            HostbindMessage(1);
            lbNow.Text = "1";
        }
        else
        {
            lbNow.Text = (Convert.ToInt32(lbNow.Text) - 1).ToString();
            HostbindMessage(Convert.ToInt32(lbNow.Text));

        }

    }

    protected void btnDrow_Click(object sender, EventArgs e)
    {
        if (lbNow.Text == lbTotal.Text)
        {
            HostbindMessage(1);
            lbNow.Text = "1";
        }
        else
        {
            lbNow.Text = (Convert.ToInt32(lbNow.Text) + 1).ToString();
            HostbindMessage(Convert.ToInt32(lbNow.Text));
        }

    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        HostbindMessage(Convert.ToInt32(lbTotal.Text));
        lbNow.Text = lbTotal.Text;

    }

    protected void btnJump_Click(object sender, EventArgs e)
    {
        if ((Convert.ToInt32(txtJump.Text.Trim()) < 1) || (Convert.ToInt32(txtJump.Text.Trim()) > Convert.ToInt32(lbTotal.Text.Trim())))
        {
            HostbindMessage(1);
            lbNow.Text = "1";

        }
        else
        {
            HostbindMessage(Convert.ToInt32(txtJump.Text.Trim()));
            lbNow.Text = txtJump.Text.Trim();

        }

    }

    protected void rptFist_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string url = "message.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        if (e.CommandName== "FirstReply")
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
        else if(e.CommandName== "FirstSubSay")
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
        else if(e.CommandName== "DelFirst")
        {

            if (Session["HostQQ"].ToString().Trim() != Session["GuestQQ"].ToString().Trim())
            {
                Response.Write("<script language='javascript'>window.alert('无权操作！');window.location='" + url + "'</script>");
                return;
            }

            string sqlDelSay = "delete from Say where Sid='"+e.CommandArgument.ToString()+"'";
            if(class_Operate.GO(sqlDelSay) ==1)
                Response.Write("<script language='javascript'>window.alert('留言删除成功！');window.location='" + url + "'</script>");
            else
                Response.Write("<script language='javascript'>window.alert('留言删除失败！');window.location='" + url + "'</script>");


        }
       
        else if(e.CommandName=="imgHeadFirst")
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
       
        string sqlstr = "insert into MessageComment(MCmid,MCtime,MCcontent,MCguestqq) values (@mid,@time,@text,@guestqq)";
        SqlCommand cmd = new SqlCommand(sqlstr, connection);
        cmd.Parameters.Clear();

        

        cmd.Parameters.Add("@guestqq", SqlDbType.VarChar);
        cmd.Parameters["@guestqq"].Value = commentQQ;

        cmd.Parameters.Add("@mid", SqlDbType.Int);
        cmd.Parameters["@mid"].Value = sayID;

        cmd.Parameters.Add("@time", SqlDbType.VarChar);
        cmd.Parameters["@time"].Value = nowTime;

        cmd.Parameters.Add("@text", SqlDbType.NVarChar);
        cmd.Parameters["@text"].Value = content;

        

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

            int Mid = Convert.ToInt32(drvw["Mid"]);

            string sql = "select * from View_MessageSecond where  MCmid='" + Mid + "' order by MCtime desc";

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
        string url = "message.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        if (e.CommandName== "DelSecond")
        {
            // Response.Write("<script language='javascript'>window.alert('Hahaha~');</script>");
            if (Session["HostQQ"].ToString().Trim() != Session["GuestQQ"].ToString().Trim())  //验证是否本人
            {
                Response.Write("<script language='javascript'>window.alert('无权操作！');window.location='" + url + "'</script>");
                return;
            }
            string sqlDelSay = "delete from MessageComment where MCid='" + e.CommandArgument.ToString()+"'";
            if (class_Operate.GO(sqlDelSay) == 1)
                Response.Write("<script language='javascript'>window.alert('评论删除成功！');window.location='" + url + "'</script>");
            else
                Response.Write("<script language='javascript'>window.alert('评论删除失败！');window.location='" + url + "'</script>");

        }
        else if(e.CommandName== "imgHeadSecond")
            {
              ImageButton imgbtn = (ImageButton)e.Item.FindControl("imgbtnCommenterHead");
            string jumpUrl = imgbtn.PostBackUrl;
            Response.Write("<script>window.open('" + jumpUrl + "')</script>");
        }
    }

    protected void btnSubSay_Click(object sender, EventArgs e)
    {
        string url = "message.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        if (txtNewSay.Text.Trim ()=="")
        {
            Response.Write("<script language='javascript'>window.alert('留几句话呗，空着多不好~');</script>");
        }
        else
        {
           
            if (PublishNewMessage(Session["HostQQ"].ToString().Trim(),Session["GuestQQ"].ToString().Trim(), txtNewSay.Text.Trim()))
            {
                if(class_Operate.QqGrade(Session["GuestQQ"].ToString().Trim(), 4))   //等级增加
                    Response.Write("<script language='javascript'>window.alert('等级success！');</script>");
                else
                    Response.Write("<script language='javascript'>window.alert('等级fail！');</script>");

                Response.Write("<script language='javascript'>window.alert('留言发表成功！');window.location='"+url+"'</script>");
            }
                
                else
                    Response.Write("<script language='javascript'>window.alert('留言发表失败！');window.location='" + url + "'</script>");


        }
    }

    private bool PublishNewMessage(string hostQQ, string guestqq,string newContent)
    {

        string nowTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        string sqlconn = class_Operate.str;

        SqlConnection connection = new SqlConnection(sqlconn);
        connection.Open();

        string sqlstr = "insert into Message(Mqq,Mguestqq, MpublishTime,Mcontent) values (@uqq,@guestqq,@time,@text)";
        SqlCommand cmd = new SqlCommand(sqlstr, connection);
        cmd.Parameters.Clear();

        

        cmd.Parameters.Add("@uqq", SqlDbType.VarChar);
        cmd.Parameters["@uqq"].Value = hostQQ;

        cmd.Parameters.Add("@guestqq", SqlDbType.VarChar);
        cmd.Parameters["@guestqq"].Value = guestqq;

        cmd.Parameters.Add("@time", SqlDbType.VarChar);
        cmd.Parameters["@time"].Value = nowTime;

       

        cmd.Parameters.Add("@text", SqlDbType.NVarChar);
        cmd.Parameters["@text"].Value = newContent;



        int i = cmd.ExecuteNonQuery();
        connection.Close();
        if (i > 0)
            return true;
        else
            return false;


    }

    
  
    protected void bntHostMess_Click(object sender, EventArgs e)
    {
        txtHosthope.Enabled = true;
    }

    protected void bntSubmitMess_Click(object sender, EventArgs e)
    {

        string url = "message.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        string fuck = txtHosthope.Text;
        if (txtHosthope.Text.Trim()=="")
        {
            Response.Write("<script language='javascript'>window.alert('写下你的风格吧！');window.location='" + url + "'</script>");
        }
        else
        {
 string sqlconn = class_Operate.str;

        SqlConnection connection = new SqlConnection(sqlconn);
        connection.Open();
            
        string sqlstr = "update Users set Uhope=@text where Uqq=@uqq";
            SqlCommand cmd = new SqlCommand(sqlstr, connection);
        cmd.Parameters.Clear();



        cmd.Parameters.Add("@uqq", SqlDbType.VarChar);
        cmd.Parameters["@uqq"].Value = Session["HostQQ"].ToString().Trim();



        cmd.Parameters.Add("@text", SqlDbType.NVarChar);
        cmd.Parameters["@text"].Value = txtHosthope.Text;



        int i = cmd.ExecuteNonQuery();
        connection.Close();
        if (i > 0)
                Response.Write("<script language='javascript'>window.alert('成功写下你的寄语!');window.location='" + url + "'</script>");
            else
                Response.Write("<script language='javascript'>window.alert('失败，请重新写下你的寄语!');window.location='" + url + "'</script>");

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