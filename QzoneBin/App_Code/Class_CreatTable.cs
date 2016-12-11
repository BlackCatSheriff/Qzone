using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Class_CreatTable 的摘要说明
/// </summary>
public class Class_CreatTable
{
    public Class_CreatTable()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }                                 
   private static string[] column = new string[] {"QQ", "Unick", "Uheadimg", "Sid","SgoodCounts","Scontent","Did","Dtitle","Dcontent","Pid","PimgPath","PimgName", "PfolderID","DATE" };

    public static DataTable CreatBigTable(DataTable dtSay,DataTable dtDiary,DataTable dtPhoto)
    {
        DataTable bigTable = new DataTable();
        for(int i=0;i<column.Length;i++)    //给新创建的表添加列名
        {
            DataColumn dc = new DataColumn(column[i], Type.GetType("System.String"));   //第一个参数 列名，第二个参数 列的类型
            bigTable.Columns.Add(dc);                                                     ///把列添加到新建表中
        }

        
        //开始填入dtSay的数据
        for (int i=0;i<dtSay.Rows.Count;i++)
        {
            DataRow dr = bigTable.NewRow();
            dr["QQ"] = dtSay.Rows[i]["Sqq"].ToString();
            dr["Unick"] = dtSay.Rows[i]["Unick"].ToString();
            dr["Uheadimg"] = dtSay.Rows[i]["Uheadimg"].ToString();
            dr["Sid"] = dtSay.Rows[i]["Sid"].ToString();
            dr["SgoodCounts"] = dtSay.Rows[i]["SgoodCounts"].ToString();
            dr["Scontent"] = dtSay.Rows[i]["Scontent"].ToString();
            dr["DATE"] = dtSay.Rows[i]["SpublishTime"].ToString ();
            bigTable.Rows.Add(dr);
        }

        //开始填入dtDiary的数据
        for(int i=0;i<dtDiary.Rows.Count;i++)
        {
            DataRow dr = bigTable.NewRow();
            dr["QQ"] = dtDiary.Rows[i]["Dqq"].ToString();
            dr["Unick"] = dtDiary.Rows[i]["Unick"].ToString();
            dr["Uheadimg"] = dtDiary.Rows[i]["Uheadimg"].ToString();
            dr["Did"] = dtDiary.Rows[i]["Did"].ToString();
            dr["Dtitle"] = dtDiary.Rows[i]["Dtitle"].ToString();
            dr["Dcontent"] = dtDiary.Rows[i]["Dcontent"].ToString();
            dr["DATE"] = dtDiary.Rows[i]["DpublishTime"].ToString ();
            bigTable.Rows.Add(dr);
        }

        //开始填入dtPhoto的数据
        for(int i=0;i<dtPhoto.Rows.Count;i++)
        {
            DataRow dr = bigTable.NewRow();
            dr["QQ"] = dtPhoto.Rows[i]["Pqq"].ToString();
            dr["Unick"] = dtPhoto.Rows[i]["Unick"].ToString();
            dr["Uheadimg"] = dtPhoto.Rows[i]["Uheadimg"].ToString();
            dr["Pid"] = dtPhoto.Rows[i]["Pid"].ToString();
            dr["PimgPath"] = dtPhoto.Rows[i]["PimgPath"].ToString();
            dr["PimgName"] = dtPhoto.Rows[i]["PimgName"].ToString();
            dr["PfolderID"] = dtPhoto.Rows[i]["PfolderID"].ToString();
            dr["DATE"] = dtPhoto.Rows[i]["Ptime"].ToString ();
            bigTable.Rows.Add(dr);
        }
        bigTable.DefaultView.Sort = "DATE DESC";
        return bigTable;

    }
    
}