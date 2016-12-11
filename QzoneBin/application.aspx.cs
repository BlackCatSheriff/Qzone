using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class comment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
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

               Applybind(1);
            }
            else
                Response.Write("<script language='javascript'>window.alert('身份过期，请重新登录！');window.location='Login.aspx'</script>");

        }




    }

  

    private void Applybind(int currentPage)
    {

        try
        {

            string sql = "select * from View_Relationship where RhostQq='"+ Request.Cookies["userQQ"].Value + "' and Rstate='0'";
            DataTable dt = class_Operate.SelectT(sql);

            PagedDataSource pds = new PagedDataSource();
            pds.AllowPaging = true;
            pds.PageSize = 5;
            pds.DataSource = dt.DefaultView;
            lbTotal.Text = pds.PageCount.ToString();
            if (pds.PageCount > 1)
                divTool.Visible = true;
            pds.CurrentPageIndex = currentPage - 1;
            rptApply.DataSource = pds;
            rptApply.DataBind();

        }
        catch (Exception ex)
        {
            Response.Write(ex);
            
        }
    }
    
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        Applybind(1);
        lbNow.Text = "1";


    }

    protected void btnUp_Click(object sender, EventArgs e)
    {
        if (lbNow.Text == "1")
        {
            Applybind(1);
            lbNow.Text = "1";
        }
        else
        {
            lbNow.Text = (Convert.ToInt32(lbNow.Text) - 1).ToString();
            Applybind(Convert.ToInt32(lbNow.Text));

        }

    }

    protected void btnDrow_Click(object sender, EventArgs e)
    {
        if (lbNow.Text == lbTotal.Text)
        {
            Applybind(1);
            lbNow.Text = "1";
        }
        else
        {
            lbNow.Text = (Convert.ToInt32(lbNow.Text) + 1).ToString();
            Applybind(Convert.ToInt32(lbNow.Text));
        }

    }

    protected void btnLast_Click(object sender, EventArgs e)
    {
        Applybind(Convert.ToInt32(lbTotal.Text));
        lbNow.Text = lbTotal.Text;

    }
    
    protected void btnJump_Click(object sender, EventArgs e)
    {
        if ((Convert.ToInt32(txtJump.Text.Trim()) < 1) || (Convert.ToInt32(txtJump.Text.Trim()) > Convert.ToInt32(lbTotal.Text.Trim())))
        {
            Applybind(1);
            lbNow.Text = "1";

        }
        else
        {
            Applybind(Convert.ToInt32(txtJump.Text.Trim()));
            lbNow.Text = txtJump.Text.Trim();

        }

    }
    

    protected void rptApply_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
         if(e.CommandName== "Follow")
        {
            string sqlupdate = "update Relation set Rstate=1 where RhostQq='" + Session["HostQQ"].ToString().Trim() + "' and RguestQq='" + e.CommandArgument.ToString() + "' ";
            if(class_Operate.GO(sqlupdate)==1)
            {
                Response.Write("<script language='javascript'>alert('关注成功！互粉成功！');window.location='application.aspx'</script>");
            }
        }
         if(e.CommandName== "jumphome")
        {
            ImageButton imgbtn = (ImageButton)e.Item.FindControl("imgbtnFriend");
            Response.Write("<script language='javascript'>window.open('" + imgbtn.PostBackUrl + "')</script>");
        }
    }
}