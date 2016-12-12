using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class writediary : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Request.Cookies["userQQ"] != null && Request.Cookies["passWord"] != null)
        {
            //判断qq和密码是否匹配
            int judge = class_Operate.isRght(Request.Cookies["userQQ"].Value, Request.Cookies["passWord"].Value);
            if (judge != 1)

            {
                Response.Write("<script language='javascript'>window.alert('身份过期，请重新登录！');window.close();window.open('Login.aspx')</script>");
                return;
            }

        }
        else
            Response.Write("<script language='javascript'>window.alert('身份过期，请重新登录！');window.close();window.open('Login.aspx')</script>");


    }

    protected void btnReturnList_Click(object sender, EventArgs e)    //销毁窗口
    {
       // Response.Write("<script language='javascript'>window.location='diary.aspx'</script>");
        Response.Write("<script language='javascript'>window.close();window.open('','_self');</script>");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");     

        string sqlconn = class_Operate.str;

        SqlConnection connection = new SqlConnection(sqlconn);
        connection.Open();

       
        string sqlstr = "insert into Diary (Dqq,DpublishTime,Dtitle,Dcontent) values (@uqq,@time,@title,@text)";        //插入新日志
        SqlCommand cmd = new SqlCommand(sqlstr, connection);
        cmd.Parameters.Clear();

        cmd.Parameters.Add("@uqq", SqlDbType.VarChar);
        cmd.Parameters["@uqq"].Value = Request.Cookies["userQQ"].Value;

        cmd.Parameters.Add("@text", SqlDbType.NVarChar);
        cmd.Parameters["@text"].Value = txtDiary.Text;

        cmd.Parameters.Add("@time", SqlDbType.VarChar);
        cmd.Parameters["@time"].Value = time;


        cmd.Parameters.Add("@title", SqlDbType.NVarChar);
        cmd.Parameters["@title"].Value = txtTile.Text.Trim();




        int i = cmd.ExecuteNonQuery();
        connection.Close();
        if (i == 1)
        {
            class_Operate.QqGrade(Request.Cookies["userQQ"].Value, 2);
            Response.Write("<script language='javascript'>window.alert('发表成功');window.close();window.open('','_self');</script>");
        }
            
        else
            Response.Write("<script language='javascript'>window.alert('发表失败，请重试！');window.close();window.open('','_self');</script>");

    }
}