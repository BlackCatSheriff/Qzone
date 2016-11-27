using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;





/// <summary>
/// Class_img 的摘要说明
/// </summary>
public class Class_img
{
    public Class_img()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }


    public static string  UpFileFun(FileUpload Controlfile, string[] FileType, int FileSize, string SaveFileName)
    {
        string FileDir = Controlfile.PostedFile.FileName;
        string FileName = FileDir.Substring(FileDir.LastIndexOf("\\") + 1);                  //获取上传文件名称
        string FileNameType = FileDir.Substring(FileDir.LastIndexOf(".") + 1).ToString();    //获取上传文件类型
        int FileNameSize = Controlfile.PostedFile.ContentLength;                             //获取上传文件大小
        
        //  定义上传文件类型，并初始化
        string Types = "";


        //string strDate = DateTime.Now.ToString();//取当前时间用来修改上传文件名   
        //string str = strDate.Replace("/", "").Replace(":", "").Replace("   ", "");   //过滤当前时间里的特殊字符，如: - / : ,
        //HttpContext.Current.Response.Write("<hr><br>" + str + "<br><br><br><hr");
        string EditFileName = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff").Replace(" ", "_").Replace(":", "-") + Guid.NewGuid().ToString();
        //string strNewFileName = Guid.NewGuid().ToString();   

        //HttpContext.Current.Response.Write("<hr><br>" + strNewFileName + "<br><br><br><hr");

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
                    Controlfile.PostedFile.SaveAs(HttpContext.Current.Server.MapPath(SaveFileName) + "/" + EditFileName + FileName);
                    return "上传成功！";
                }
                else
                {
                    return "上传失败！上传文件类型不符合";
                }
            }
            else
            {
                
                
                return "上传失败！上传文件尺寸超出限制！";
            }

        }
        catch
        {
            return "上传失败，请重新上传！";
        }
    }



}