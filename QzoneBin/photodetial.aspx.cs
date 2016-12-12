using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class photodetial : System.Web.UI.Page
{
    protected string Nike;
    protected string PhotoName;
    protected string PhotoTime;
    private string PhotoPath;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["pid"] == null)                 //验证传值
        {
            Response.Write("<script>alert('发生了意想不到的事！');window.close();window.open('','_self');</script>");
        }
        else
        {
            //判断pid正确性,如果正确显示，否则退出

            int pid = Convert.ToInt32(Request.QueryString["pid"].ToString()) ;

            if(!IsPostBack)
            {

                if (!bindData(pid))
                Response.Write("<script>alert('发生了意想不到的事！');window.close();window.open('','_self');</script>");
                
            }
            imgPhoto.ImageUrl = PhotoPath;
            
        }

    }
    private bool bindData(int pid)
    {
       
        SqlConnection scon = new SqlConnection(class_Operate.str);

        scon.Open();

        SqlCommand selectCmd = new SqlCommand();

        selectCmd.CommandText = "select * from View_Photo where Pid=@pid";

        selectCmd.Parameters.Add("@pid", SqlDbType.Int);//sql指令中存在一个参数，叫@sn,它的类型是字符型，字节长度是10个

        selectCmd.Parameters["@pid"].Value = pid;
        
        selectCmd.Connection = scon;

        SqlDataAdapter sda = new SqlDataAdapter();

        sda.SelectCommand = selectCmd;

        DataTable dt = new DataTable();


        sda.Fill(dt);
        if (dt.Rows.Count > 0)

        {
            Nike = dt.Rows[0]["Unick"].ToString();
            PhotoName = dt.Rows[0]["PimgName"].ToString();
            PhotoTime = dt.Rows[0]["Ptime"].ToString();
            PhotoPath = dt.Rows[0]["PimgPath"].ToString();
            return true;

        }
        else
            return false;

    }
    protected void btnreturn_Click(object sender, EventArgs e)
    {
        Response.Write("<script>window.close();window.open('','_self');</script>");
    }
}