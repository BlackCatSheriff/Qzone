using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class creatAlbum : System.Web.UI.Page
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

    protected void btnCreatAlbum_Click(object sender, EventArgs e)
    {
        string qq = Request.Cookies["userQQ"].Value;
        if(txtAlbumName.Text.Trim()=="")
        {
            Response.Write("<script>window.alert('不能不写名字哦~')</script>");//刷新本页面
        }
        else
        {
            //string sql = "insert into Album(AsurfacePath,Aname,Aqq,AsurfaceBelong) values ('~/img/album.png','" + txtAlbumName.Text.Trim() + "','" + qq + "','-1')";
            if (CreatAlbum(txtAlbumName.Text.Trim(), qq))
               // Response.Write("<script>window.alert('创建成功！');window.location='uploadPhoto.aspx'</script>");//跳上传界面

            Response.Write("<script>window.alert('创建成功！');window.open('uploadPhoto.aspx', 'newwindow', 'height=400, width=600, top='+Math.round((window.screen.height-400)/2)+',left='+Math.round((window.screen.width-600)/2)+', toolbar=no,menubar = no, scrollbars = no, resizable = no, location = no, status = no');window.close();</script>");
            else
                Response.Write("<script>window.alert('创建失败！')</script>");//刷新本页面
        }
    }


    private bool CreatAlbum (string name , string qq)
    {

        
        string sqlconn = class_Operate.str;

        SqlConnection connection = new SqlConnection(sqlconn);
        connection.Open();
        string sqlstr = "insert into Album(AsurfacePath,Aname,Aqq,AsurfaceBelong,Acount) VALUES  (@AsurfacePath,@Aname,@Aqq,@AsurfaceBelong,@Acount)";
        SqlCommand cmd = new SqlCommand(sqlstr, connection);
        cmd.Parameters.Clear();
        
        cmd.Parameters.Add("@AsurfacePath", SqlDbType.NVarChar);
        cmd.Parameters["@AsurfacePath"].Value = "~/img/album.png";

        cmd.Parameters.Add("@Aname", SqlDbType.NVarChar);
        cmd.Parameters["@Aname"].Value = name;

        cmd.Parameters.Add("@Aqq", SqlDbType.VarChar);
        cmd.Parameters["@Aqq"].Value = qq;

        cmd.Parameters.Add("@AsurfaceBelong", SqlDbType.Int);
        cmd.Parameters["@AsurfaceBelong"].Value = -1;

        cmd.Parameters.Add("@Acount", SqlDbType.Int);
        cmd.Parameters["@Acount"].Value = 0;



        int i = cmd.ExecuteNonQuery();
        connection.Close();
        if (i > 0)
            return true;
        else
            return false;


    }

}