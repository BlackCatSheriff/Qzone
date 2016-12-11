<%@ Page Language="C#" AutoEventWireup="true" CodeFile="displaydiary.aspx.cs" Inherits="comment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

    <style type="text/css">
a:link,a:visited{
 text-decoration:none;  /*超链接无下划线*/
}
a:hover{
 text-decoration:underline;  /*鼠标放上去有下划线*/
}
</style>
</head>
<body>
    <form id="form1" runat="server">
    <div  id="div1" runat ="server" >

              <div style="background: #1F1F1F; height: 40px; font-weight: bold; font-size: 20px; color: white; font-family: Verdana">
            
            &nbsp&nbsp&nbsp<img  runat ="server"  src="/img/logo.png" />
            <asp:ImageButton ID="imbtnPersonality" runat="server" BorderStyle="None" ImageUrl="~/img/personality.png" OnClick="imbtnPersonality_Click"  />
            <asp:ImageButton ID="imgbtnMyhome"  runat="server" BorderStyle="None" ImageUrl="~/img/myhome.png" OnClick="imgbtnMyhome_Click"  />
            <asp:ImageButton ID="imgbutFriends" runat="server" BorderStyle="None" ImageUrl="~/img/friends.png"  OnClick="imgbutFriends_Click" />

            <asp:LinkButton  ID="lbtGuestUsername" runat="server" style="TEXT-DECORATION: none"  ForeColor="White" OnClick="lbtGuestUsername_Click"></asp:LinkButton>
            

            <asp:ImageButton ID ="imgbtnExit" runat="server" BorderStyle ="None" ImageUrl="~/img/exit.png" OnClick="imgbtnExit_Click" />
            <asp:ImageButton ID="imgbtnSetting" runat ="server" BorderStyle ="None" ImageUrl="~/img/set.png" OnClick="imgbtnSetting_Click" />

            
   </div>




         <div style ="height:100px;" >
            <asp:Label ID="lblHostUsername" runat="server" > 添加一个标签,访问对象（目前空间显示）的账号 </asp:Label><br/>
            <asp:Label ID="lblHostSignature" runat="server" > 添加一个标签,签名档 </asp:Label> <br /><br />
             <table>
                 <tr>
                     <td>
                                <asp:ImageButton ID="imgBtnHostHead" runat="server" Width="100px" Height="100px" OnClick="imgBtnHostHead_Click" ></asp:ImageButton>
                     </td>
                     <td>
                         <asp:Label ID="lblHostQnike" runat="server" >标签</asp:Label><br />
            <asp:Label ID="lblHostGrade" runat="server" >label等级标签</asp:Label>
                     </td>
                 </tr>
             </table>
            
        </div>
        <br /><br />
        <br /><br />
        <div style="background: #FFFFFF; height:30px; font-weight:bold; font-size:large;color:blue;font-family:Verdana">
            <a id="aHome" runat ="server" >主页</a>&nbsp&nbsp<a id="aSay" runat="server">说说</a>&nbsp&nbsp<a id="aDaily" runat ="server"  >日志</a>&nbsp&nbsp<a id="aAlbum" runat ="server" >相册</a>&nbsp&nbsp<a id="aMessage" runat ="server" >留言板</a>&nbsp&nbsp<a id="aLog" runat ="server" >访客功能</a> 
        </div>

        <br />

        <div>
            &nbsp&nbsp
            
            <asp:Button ID="btnEdit" runat ="server" Text ="编辑" OnClick="btnEdit_Click"  />&nbsp&nbsp
            <asp:Button ID="btnSave" runat ="server" Text ="保存" OnClick="btnSave_Click" />&nbsp&nbsp

            <asp:Button ID="btnDelete" runat ="server" Text ="删除" OnClick="btnDelete_Click" />&nbsp&nbsp
            <asp:Button ID="btnReturnList" runat ="server" Text="返回日志列表"  OnClick="btnReturnList_Click"/>
            <br /><br />
           标题：（不超过20字） <asp:TextBox ID="txtTile" runat ="server" Enabled ="false" MaxLength="20"></asp:TextBox><br />
            <asp:TextBox ID="txtDiary" runat ="server"  Enabled="false" Width="1000" Height ="700" MaxLength="2000" TextMode="MultiLine"></asp:TextBox>




        </div>
      


        <div  align="center"><img src="img/boot.png" /></div>
        <em>Copyright © Sky</em>

    </div>

                <div id="div404" runat="server" style="background-color:black;width:100%;height:2000px;color:red;font-size:xx-large"  Visible="false">

            <h1 margin-left: auto; margin-right: auto; >不要瞎搞ooo  ^_^</h1>
            <br />
            <br />
            <asp:Button ID="btnexit" runat ="server"  Text ="我知道了"  Font-Bold="true" Font-Size="XX-Large" Font-Italic="true" OnClick="btnexit_Click" />
        </div>


    
    
    </form>
</body>
</html>
