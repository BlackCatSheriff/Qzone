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

                HostbindAlbum(1);
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
                btnUploadImg.Visible = false;
                btnCreatFolder.Visible = false;
                

            }
            //HttpUtility.UrlDecode(Request.Cookies["userNick"].Value,Encoding.GetEncoding("utf-8"));
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

    private void HostbindAlbum(int currentPage)
    {

        try
        {

            string sql = "select * from Album where Aqq = '" + Session["HostQQ"].ToString().Trim() + "'order by Aid asc";
            DataTable dt = class_Operate.SelectT(sql);

            PagedDataSource pds = new PagedDataSource();
            pds.AllowPaging = true;
            pds.PageSize = 5;
            pds.DataSource = dt.DefaultView;
            lbTotal.Text = pds.PageCount.ToString();
            if (pds.PageCount > 1)
                divTool.Visible = true;
            pds.CurrentPageIndex = currentPage - 1;
            rptDisplayAlbum.DataSource = pds;
            rptDisplayAlbum.DataBind();

        }
        catch (Exception ex)
        {
            Response.Write(ex);

        }
    }


    protected void btnFirst_Click(object sender, EventArgs e)
    {
        HostbindAlbum(1);
        lbNow.Text = "1";


    }

    protected void btnUp_Click(object sender, EventArgs e)
    {
        if (lbNow.Text == "1")
        {
            HostbindAlbum(1);
            lbNow.Text = "1";
        }
        else
        {
            lbNow.Text = (Convert.ToInt32(lbNow.Text) - 1).ToString();
            HostbindAlbum(Convert.ToInt32(lbNow.Text));

        }

    }

    protected void btnDrow_Click(object sender, EventArgs e)
    {
        if (lbNow.Text == lbTotal.Text)
        {
            HostbindAlbum(1);
            lbNow.Text = "1";
        }
        else
        {
            lbNow.Text = (Convert.ToInt32(lbNow.Text) + 1).ToString();
            HostbindAlbum(Convert.ToInt32(lbNow.Text));
        }

    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        HostbindAlbum(Convert.ToInt32(lbTotal.Text));
        lbNow.Text = lbTotal.Text;

    }

    protected void btnJump_Click(object sender, EventArgs e)
    {
        if ((Convert.ToInt32(txtJump.Text.Trim()) < 1) || (Convert.ToInt32(txtJump.Text.Trim()) > Convert.ToInt32(lbTotal.Text.Trim())))
        {
            HostbindAlbum(1);
            lbNow.Text = "1";

        }
        else
        {
            HostbindAlbum(Convert.ToInt32(txtJump.Text.Trim()));
            lbNow.Text = txtJump.Text.Trim();

        }

    }




    protected void btnUploadImg_Click(object sender, EventArgs e)
    {

        // string url = "uploadPhoto.aspx?uqq=" + Session["HostQQ"].ToString().Trim(); 
        Response.Write("<script language='javascript'>window.open('uploadPhoto.aspx', 'newwindow', 'height=400, width=800, top='+Math.round((window.screen.height-400)/2)+',left='+Math.round((window.screen.width-800)/2)+', toolbar=no,menubar = no, scrollbars = no, resizable = no, location = no, status = no')</script>");
    }

    protected void btnCreatFolder_Click(object sender, EventArgs e)
    {
        Response.Write("<script language='javascript'>window.open('creatAlbum.aspx', 'newwindow', 'height=200, width=300, top='+Math.round((window.screen.height-200)/2)+',left='+Math.round((window.screen.width-300)/2)+', toolbar=no,menubar = no, scrollbars = no, resizable = no, location = no, status = no')</script>");
    }

    protected void rptDisplayAlbum_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string url = "album.aspx?uqq=" + Session["HostQQ"].ToString().Trim();
        if (e.CommandName == "DelAlbum")
        {
            if (Session["HostQQ"].ToString().Trim() != Session["GuestQQ"].ToString().Trim())
            {
                Response.Write("<script language='javascript'>window.alert('无权操作！');window.location='" + url + "'</script>");
                return;
            }
            //解决级联问题再写
        }
        else if (e.CommandName == "imgbtnJumpPhotos")
        {
            string jumpurl = "photo.aspx?uqq=" + Session["HostQQ"].ToString().Trim() + "&aid=" + e.CommandArgument.ToString().Trim();
            Response.Write("<script language='javascript'>window.open('" + jumpurl + "')</script>");
        }
        else if (e.CommandName == "Changename")
        {
            if (Session["HostQQ"].ToString().Trim() != Session["GuestQQ"].ToString().Trim())
            {
                Response.Write("<script language='javascript'>window.alert('无权操作！');window.location='" + url + "'</script>");
                return;
            }
            else
            {
                TextBox txtchange = (TextBox)e.Item.FindControl("txtRename");
                txtchange.Visible = true;
                Button btnconfirm = (Button)e.Item.FindControl("btnConfirm");
                btnconfirm.Visible = true;
            }

        }
        else if (e.CommandName == "ConfirmChange")
        {
            TextBox txtComment_cs = (TextBox)e.Item.FindControl("txtRename");
            if (txtComment_cs.Text.Trim() != "")
            {

                string sqlupdateName = "update Album set Aname = '" + txtComment_cs.Text.Trim() + "' where Aid = '" + e.CommandArgument.ToString() + "'";

                if (class_Operate.GO(sqlupdateName) == 1)
                    Response.Write("<script language='javascript'>window.alert('改名成功！');window.location='" + url + "'</script>");
                else
                    Response.Write("<script language='javascript'>window.alert('抱歉改名失败，请重试');window.location='" + url + "'</script>");

            }
            else
            {
                Response.Write("<script language='javascript'>window.alert('留几句话呗，空着多不好~');window.location='" + url + "'</script>");
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