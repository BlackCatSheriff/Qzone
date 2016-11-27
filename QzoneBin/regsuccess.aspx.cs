using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class regsuccess : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Session["isFirst"]==null)
        {
            Response.Redirect("Regester.aspx");
        }
        else
        {
            Session["isFirst"] = null;

            if (Request.Cookies["userQQ"] != null && Request.Cookies["passWord"] != null)
            {
                string sqlcatchpwd = "select Users.Uqq from Users where Uqq='" + Request.Cookies["userQQ"].ToString() + "' and Upwd='" + Request.Cookies["passWord"].ToString() + "'";
                DataTable dt = class_Operate.SelectT(sqlcatchpwd);
                if (dt.Rows.Count <= 0)
                {
                    Response.Redirect("Regester.aspx");
                       return;
            }
                if (!IsPostBack)
                {
                    //显示数据
                    myBind();
           }

               
            }
            else
                Response.Redirect("Regester.aspx");

        }

       

    }






    private void myBind ()
    {
        lbluserNick.Text = Request.Cookies["userNick"].ToString();
        lbluserQQ.Text = Request.Cookies["userQQ"].ToString();

        string sqlBind = "select * from Users where Uqq='" + Request.Cookies["userQQ"].ToString() +"'";
        DataTable dt = class_Operate.SelectT(sqlBind);
        imgUserhead.ImageUrl = dt.Rows[0][7].ToString();
        txtUserNike.Text= dt.Rows[0][1].ToString();
        ddlSex.Text = dt.Rows[0][5].ToString();
        txtBirthday.Text = dt.Rows[0][6].ToString();
        lblStarttime.Text = dt.Rows[0][8].ToString();
        lblEmail.Text = dt.Rows[0][3].ToString();
        lblPhone.Text = dt.Rows[0][4].ToString();



    }

    

    protected void btnInfoSub_Click(object sender, EventArgs e)
    {
        string headUrl;
        string userSex = ddlSex.Text;
        string sqluodate = "";
    }

    protected void btnChangeHead_Click(object sender, EventArgs e)
    {
        //上传图片


    }


}