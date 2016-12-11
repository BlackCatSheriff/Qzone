using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class photo : System.Web.UI.Page
{
    //string url = "photo.aspx?uqq=" + Session["HostQQ"].ToString().Trim() + "&aid=" + Session["albumid"].ToString().Trim();
    protected string ablbumName;
    protected void Page_Load(object sender, EventArgs e)
    {
        //    ablbumName



        if (Request.Cookies["userQQ"] == null)
        {
            Response.Write("<script language='javascript'>window.alert('身份过期，请重新登录！');window.location='Login.aspx'</script>");
            return;
        }

        if (Request.QueryString["uqq"] == null || Request.QueryString["aid"] ==null)
        {
            div1.Visible = false;
            divTool.Visible = false;
            div404.Visible = true;
           

           // Response.Write("<script language='javascript'>window.alert('非法访问！');window.close();window.open('','_self');</script>");
            return;

        }
        else
        {
            Session["HostQQ"] = Request.QueryString["uqq"].ToString().Trim();
            Session["GuestQQ"] = Request.Cookies["userQQ"].Value;
            Session["albumid"] = Request.QueryString["aid"].ToString().Trim();


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
                string sqlalbumname = "select Aname from Album where Aid='" + Session["albumid"].ToString().Trim() + "'";
                ablbumName = class_Operate.SelectHead(sqlalbumname);



                HostbindPhoto(1);
            }
            else
                Response.Write("<script language='javascript'>window.alert('身份过期，请重新登录！');window.location='Login.aspx'</script>");

        }

        

    }
    

    protected void rptphotos_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string url = "photo.aspx?uqq=" + Session["HostQQ"].ToString().Trim()+"&aid="+Session["albumid"].ToString().Trim();
        if (e.CommandName== "Delpoto")
        {
            if (Session["HostQQ"].ToString().Trim() != Session["GuestQQ"].ToString().Trim())
            {
                Response.Write("<script language='javascript'>window.alert('无权操作！');window.location='" + url + "'</script>");
                return;
            }
            //检查删除的图片是不是封皮，如果是改变封皮内容，删除本地照片
            string sqlisSurface = "select Aid from Album where Aid='" + Session["albumid"].ToString().Trim() + "' and AsurfaceBelong='" + e.CommandArgument.ToString() + "'";
            DataTable dt = class_Operate.SelectT(sqlisSurface);
            if(dt.Rows.Count>0)
            {
                string sqlreplace1  = "select PimgPath from Photo  where PfolderID='"+ Session["albumid"].ToString().Trim() + "' and Pqq='"+ Session["HostQQ"].ToString().Trim() + "'and pid!='"+e.CommandArgument.ToString()+"' ";//随机找出本相册下面的一个路径
                string repalce = class_Operate.SelectHead(sqlreplace1);
                string sqlupSurface = "update Album set AsurfacePath='" + repalce + "' where Aid='"+ Session["albumid"].ToString().Trim() + "'";
                class_Operate.GO(sqlupSurface);

            }
            string sqlDel = "delete from Photo where Pid='" + e.CommandArgument.ToString() + "'";
            string sqlupdatecount = "update Album set Acount+=-1  where Aid='" + Session["albumid"].ToString().Trim() + "' ";
            //删除本地文件
            string sqldir = "select PimgPath from Photo where pid='" + e.CommandArgument.ToString() + "'";
            string dir = class_Operate.SelectHead(sqldir);
            string filedir= dir.Substring(dir.LastIndexOf("/") + 1);
            //  File.Delete(@"");


            string reallydir = HttpContext.Current.Server.MapPath("/img/photoServer") + "\\" + filedir;
            File.Delete(reallydir);


            if (class_Operate.GO(sqlDel) ==1 && class_Operate.GO(sqlupdatecount)==1)
            {
                Response.Write("<script language='javascript'>window.alert('删除成功！');window.location='" + url + "'</script>");
            }
            else
                Response.Write("<script language='javascript'>window.alert('删除失败！请重试');window.location='" + url + "'</script>");


        }
        else if(e.CommandName== "ChangeName")
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
        else if(e.CommandName== "ConfirmChange")
        {
            TextBox txtComment_cs = (TextBox)e.Item.FindControl("txtRename");
            if (txtComment_cs.Text.Trim() != "")
            {
                string sqlupdateName = "update Photo set PimgName='" + txtComment_cs.Text.Trim() + "' where Pid='" + e.CommandArgument.ToString() + "'";

                if(class_Operate.GO(sqlupdateName)==1)
                    Response.Write("<script language='javascript'>window.alert('改名成功！');window.location='" + url + "'</script>");
                else
                    Response.Write("<script language='javascript'>window.alert('抱歉改名失败，请重试');window.location='" + url + "'</script>");

            }
            else
            {
                Response.Write("<script language='javascript'>window.alert('留几句话呗，空着多不好~');window.location='" + url + "'</script>");
            }
        }
        else if(e.CommandName== "SetSurface")
        {
            if (Session["HostQQ"].ToString().Trim() != Session["GuestQQ"].ToString().Trim())
            {
                Response.Write("<script language='javascript'>window.alert('无权操作！');window.location='" + url + "'</script>");
                return;
            }
            else
            {
                //更该删代码
                string sqlupdateSuface = "update Album set AsurfacePath =(select PimgPath from Photo where Pid='" + e.CommandArgument.ToString() + "') ,AsurfaceBelong='" + e.CommandArgument.ToString() + "' where Aid='" + Session["albumid"].ToString().Trim() + "'";
                if(class_Operate.GO(sqlupdateSuface)==1)
                {
                    Response.Write("<script language='javascript'>window.alert('封面设置成功！');window.location='" + url + "'</script>");
                }
                else
                    Response.Write("<script language='javascript'>window.alert('封面设置失败！请重试！');window.location='" + url + "'</script>");
            }
        }


    }
    private void HostbindPhoto(int currentPage)
    {

        try
        {
            
            string sql = "select * from Photo where PfolderID = '" + Session["albumid"].ToString().Trim() + "' order by Ptime desc";
            DataTable dt = class_Operate.SelectT(sql);

            PagedDataSource pds = new PagedDataSource();
            pds.AllowPaging = true;
            pds.PageSize = 10;
            pds.DataSource = dt.DefaultView;
            lbTotal.Text = pds.PageCount.ToString();
            if (pds.PageCount > 1)
                divTool.Visible = true;
            pds.CurrentPageIndex = currentPage - 1;
            rptphotos.DataSource = pds;
            rptphotos.DataBind();

        }
        catch (Exception ex)
        {
            
            Response.Write(ex);


        }
    }

    protected void btnFirst_Click(object sender, EventArgs e)
    {
        HostbindPhoto(1);
        lbNow.Text = "1";


    }

    protected void btnUp_Click(object sender, EventArgs e)
    {
        if (lbNow.Text == "1")
        {
            HostbindPhoto(1);
            lbNow.Text = "1";
        }
        else
        {
            lbNow.Text = (Convert.ToInt32(lbNow.Text) - 1).ToString();
            HostbindPhoto(Convert.ToInt32(lbNow.Text));

        }

    }

    protected void btnDrow_Click(object sender, EventArgs e)
    {
        if (lbNow.Text == lbTotal.Text)
        {
            HostbindPhoto(1);
            lbNow.Text = "1";
        }
        else
        {
            lbNow.Text = (Convert.ToInt32(lbNow.Text) + 1).ToString();
            HostbindPhoto(Convert.ToInt32(lbNow.Text));
        }

    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        HostbindPhoto(Convert.ToInt32(lbTotal.Text));
        lbNow.Text = lbTotal.Text;

    }

    protected void btnJump_Click(object sender, EventArgs e)
    {
        if ((Convert.ToInt32(txtJump.Text.Trim()) < 1) || (Convert.ToInt32(txtJump.Text.Trim()) > Convert.ToInt32(lbTotal.Text.Trim())))
        {
            HostbindPhoto(1);
            lbNow.Text = "1";

        }
        else
        {
            HostbindPhoto(Convert.ToInt32(txtJump.Text.Trim()));
            lbNow.Text = txtJump.Text.Trim();

        }

    }


    protected void btnexit_Click(object sender, EventArgs e)
    {
        Response.Write("<script language='javascript'>window.alert('别淘气！');window.close();window.open('','_self');</script>");
    }
}







