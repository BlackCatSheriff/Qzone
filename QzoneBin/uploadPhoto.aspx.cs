using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class uploadPhoto : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        
       

            if (Request.Cookies["userQQ"] != null && Request.Cookies["passWord"] != null)
            {


                //判断qq和密码是否匹配
                int judge = class_Operate.isRght(Request.Cookies["userQQ"].Value, Request.Cookies["passWord"].Value);
                if (judge != 1)

                {
                    Response.Write("<script language='javascript'>window.alert('身份过期，请重新登录！');window.open('Login.aspx');window.close();</script>");
                    return;
                }
                if(!IsPostBack)
                BindFolder(Request.Cookies["userQQ"].Value);
            }
            else
                Response.Write("<script language='javascript'>window.alert('身份过期，请重新登录！');window.open('Login.aspx');window.close();</script>");

      



        


    



    }
    private void BindFolder(string qq)
    {
        string sql = "select Aname from Album where Aqq = '" + qq+"'";

        DataTable dt = class_Operate.SelectT(sql);
        //ddlSeleectFoldder.DataTextField = "Sid";
        ddlSeleectFoldder.DataValueField = "Aname";
        ddlSeleectFoldder.DataSource = dt;
        ddlSeleectFoldder.DataBind();

    }

    

  

    protected void btnSubImg_Click(object sender, EventArgs e)
    {
        string folder  = ddlSeleectFoldder.SelectedValue;
        
        if (folder=="" )
        {
            Response.Write("<script>alert('请先创建相册！');window.location='creatAlbum.aspx'</script>");
            return;
        }
        else if (FileUpload1.PostedFile.FileName == "")
        {
            Response.Write("<script>alert('请选择照片！');</script>");
            return;
        }

        
        //上传图片
        String[] Extensions = { "gif", "png", "jpeg", "jpg", "bmp" };
        int FileSize = 2098152;   //2M
        string Dir = "/img/photoServer";
        string res = "";
        string QQ = Request.Cookies["userQQ"].Value;

        string sqlCatch = "select Aid from Album where Aname='"+folder+"'";
        string folerid = class_Operate.SelectHead(sqlCatch);
        if(folerid=="")
        {
            Response.Write("<script>window.alert('上传失败，请重新上传！');</script>");
            return;
        }
        int tfolder = Convert.ToInt32(folerid);
        res = UpFileFun(FileUpload1, Extensions, FileSize, Dir, QQ,tfolder);
        if (res != "")
            imgUserhead.ImageUrl = res;
        class_Operate.QqGrade(Request.Cookies["userQQ"].Value, 3);
        
        
      
    }


    private  string UpFileFun(FileUpload Controlfile, string[] FileType, int FileSize, string SaveFileName, string QQ, int folder)
    {
        // string FileDir = Path.GetFileName(Controlfile.PostedFile.FileName);
        string FileDir = Controlfile.PostedFile.FileName;
        string FileName = FileDir.Substring(FileDir.LastIndexOf("\\") + 1);                            //获取上传文件名称
        string FileNameType = FileDir.Substring(FileDir.LastIndexOf(".") + 1).ToString().ToLower();    //获取上传文件类型
        int FileNameSize = Controlfile.PostedFile.ContentLength;                                       //获取上传文件大小
        string filenameClear = System.IO.Path.GetFileNameWithoutExtension(FileDir);
        //  定义上传文件类型，并初始化
        string Types = "";
        string strDate = DateTime.Now.ToString();//取当前时间用来修改上传文件名   
        string str = strDate.Replace("/", "").Replace(":", "").Replace(" ", "");   //过滤当前时间里的特殊字符，如: - / : ,
        string EditFileName = QQ + str;

        try
        {
            if (FileNameSize < FileSize)
            {
                for (int i = 0; i < FileType.Length; i++)
                {
                    if (FileNameType == FileType[i])
                    {
                        Types = FileNameType;
                    }
                }
                if (FileNameType == Types)
                {
                    EditFileName += "." + Types;
                    Controlfile.PostedFile.SaveAs(HttpContext.Current.Server.MapPath(SaveFileName) + "/" + EditFileName);


                    string path = "~" + SaveFileName + "/" + EditFileName;
                    string sqlupdatehead = "insert into Photo(Pqq,PimgPath,PimgName,PfolderID,Ptime) values ('" + QQ + "','" + path + "','" + filenameClear + "','" + folder + "','"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"')";
                    string sqlupdatecount = "update Album set Acount+=1 where Aid='"+ folder + "'";
                    if (class_Operate.GO(sqlupdatehead) != 1 || class_Operate.GO(sqlupdatecount) != 1)
                    {
                        Response.Write("<script>window.alert('上传失败，请重新上传！');</script>");
                        return "";
                    }
                    else
                    {
                        class_Operate.QqGrade(QQ, 3);
                        Response.Write("<script>window.alert('上传成功！');</script>");
                        return path;

                    }
                        
                  
                  

                }
                else
                {
                    Response.Write("<script>window.alert('上传失败！上传文件类型不符合！');</script>");
                    return "";
                }
            }
            else
            {
                Response.Write("<script>window.alert('上传失败！上传文件尺寸超出限制！');</script>");
                return "";
            }

        }
        catch
        {
            Response.Write("上传失败，请重新上传！');</script>");
            return "";
        }
    }




    protected void btnReturnAlbum_Click(object sender, EventArgs e)
    {

        Response.Write("<script>window.close();window.open('','_self');</script>");
 
    }
}
