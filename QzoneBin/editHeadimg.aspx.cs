using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class editHeadimg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Request.Cookies["ImgPath"] !=null)
        {
           imgUserhead.ImageUrl= Request.Cookies["ImgPath"].Value;
        }
    }

    protected void btnSubImg_Click(object sender, EventArgs e)
    {
        if(FileUpload1.PostedFile.FileName=="")       //是否有图片
        {
            Response.Write("<script>window.alert('请选择上传图片！');</script>");
            return;
        }
        //上传图片
        String[] Extensions = { "gif", "png", "jpeg", "jpg", "bmp" };    //允许上传类型
        int FileSize = 2098152;   //2M
        string Dir = "/img/userHead";                                   //图片保存路径
        string res = "";
        string QQ = Request.Cookies["userQQ"].Value;                    //照片所属QQ

        res = Class_img.UpFileFun(FileUpload1, Extensions, FileSize, Dir, QQ);   //上传函数


        Response.Write("<script>window.alert('" + res + "');</script>");
        string sqlHeadimg = "select Uheadimg  from  Users  where Uqq='" + QQ + "'";
        imgUserhead.ImageUrl = class_Operate.SelectHead(sqlHeadimg);
        //写入cookie
        HttpCookie cookieHeadPath = new HttpCookie("ImgPath", imgUserhead.ImageUrl);
        cookieHeadPath.Expires = DateTime.Now.AddDays(2);
        Response.Cookies.Add(cookieHeadPath);
    }
}