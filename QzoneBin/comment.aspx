﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="comment.aspx.cs" Inherits="comment" %>

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
    <div>

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

                <!--main-->
            <div  id="divNewSay" runat="server" >
            发表说说：<br />
            
               <asp:TextBox ID="txtNewSay" runat="server" BorderStyle="Inset" MaxLength="500" TextMode="MultiLine" Height ="100px" Width="400px"></asp:TextBox></td>
                
                              <asp:Button ID="btnSubSay" runat ="server" Text ="发表" OnClick="btnSubSay_Click" />
        
            <br />
                </div>
            

           
             <div>
            <asp:Repeater ID="rptFist" runat ="server" OnItemCommand="rptFist_ItemCommand" OnItemDataBound="rptFist_ItemDataBound"   >
                      <HeaderTemplate >
                          <table rules=rows border="1" style="border:3px solid #000000;" cellpadding="0"cellspacing="1">
                           

                      </HeaderTemplate>
                      <ItemTemplate >
                          <tr>
                              <td><asp:ImageButton ID="imgbtnHostHead" runat ="server" ImageUrl='<%# Eval("Uheadimg") %>'   PostBackUrl='<%# "comment.aspx?uqq=" + Eval("Sqq") %>'   Width="50px" Height="50px"  CommandName="imgHeadFirst" /> <asp:Label ID="lblHostNike" runat ="server" Text ='<%# Eval("Unick") %>'></asp:Label></td>
                              <td><asp:Label ID="lblSayContent" runat="server" Text ='<%# Eval("Scontent") %>' Font-Size="Large"></asp:Label></td>
                               <td>  <asp:Button ID ="btnSayReply" runat="server" Text ="回复"  CommandName="FirstReply" CommandArgument='<%# Eval("Sid") %>'/></td>
                                <td><asp:Button ID="btnDelFirst" runat ="server" Text ="删除" CommandName="DelFirst"  CommandArgument='<%# Eval("Sid") %>' /></td>
                          </tr>

                          <tr>
                              <td>
                                  <asp:Label ID="lblFirstTime"  runat ="server" Text='<%# Eval("SpublishTime") %>'></asp:Label>
                                  </td>
                                <td>  <asp:ImageButton ID="imgbtnFirstGood" runat ="server" ImageUrl="~/img/good.png" Width="30px" Height="30px"  CommandName="FirstGood" CommandArgument='<%# Eval("Sid") %>'/><%# Eval("SgoodCounts") %> </td>
                               <td>    <asp:ImageButton ID="imgbtnFisrTrans" runat ="server"  ImageUrl="~/img/transpot.png" Width="30px" Height="30px"  CommandName="FirstTrans" CommandArgument='<%# Eval("Sid") %>'/></td>
                              
                          </tr>

                          <asp:Repeater ID="rptSeond" runat="server" OnItemCommand="rptSeond_ItemCommand" >
                              
                              <ItemTemplate>
                                 <tr>
                                   <td><asp:ImageButton ID="imgbtnCommenterHead" runat ="server" ImageUrl='<%# Eval("Uheadimg") %>' PostBackUrl='<%# "comment.aspx?uqq=" + Eval("CguestQQ") %>'  CommandName="imgHeadSecond"  Width="35px" Height="35px" /> <asp:Label ID="lblHostNike" runat ="server" Text ='<%# Eval("Unick") %>'></asp:Label></td>
                              <td><asp:Label ID="lblSayComment" runat="server" Text ='<%# Eval("Ccontent") %>' Font-Size="Large"></asp:Label></td>
                             <td><asp:Button ID="btnDelSecong" runat="server" Text ="删除" CommandName="DelSecond" CommandArgument='<%# Eval("Cid") %>' /></td>
                                      </tr>
                                     </ItemTemplate>
                                  
                          </asp:Repeater>



                          <tr>
                              <td>  <!-- 点击恢复后可见-->
                              <asp:TextBox ID="txtComment" runat ="server"  Visible="false"  BorderStyle="Inset" MaxLength="500" TextMode="MultiLine" Height="50px"></asp:TextBox>
                             
                             </td>
                              <td></td>
                             <td>  <asp:Button ID ="btnSaySub" runat="server"   Visible="false"  Text ="提交"  CommandName="FirstSubSay" CommandArgument='<%# Eval("Sid") %>'/></td>
                              
                          </tr>
                          
                      </ItemTemplate>
                      <FooterTemplate >
                         
                          </table>
                      </FooterTemplate>
                  </asp:Repeater>

                </div>
                <div  id ="divTool" runat ="server"  visible ="false">


                    <asp:Button ID="btnFirst" runat="server" Text="首页" OnClick="btnFirst_Click" />
              <asp:Button ID="btnUp" runat="server" Text="上一页" OnClick="btnUp_Click" />
        <asp:Button ID="btnDrow" runat="server" Text="下一页"  OnClick="btnDrow_Click"/>
        
        <asp:Button ID="btnLast" runat="server" Text="尾页"  OnClick="btnLast_Click"/>
        页次：<asp:Label ID="lbNow" runat="server" Text="1"></asp:Label>
        /<asp:Label ID="lbTotal" runat="server" Text="1"></asp:Label>

        转<asp:TextBox ID="txtJump" Text="1" runat="server" Width="30px" TextMode="Number"></asp:TextBox>
        <asp:Button ID="btnJump" runat="server" Text="Go"  OnClick="btnJump_Click"/>  


        </div>


        </div>
      
        <br />

        <div  align="center"><img src="img/boot.png" /></div>
        <em>Copyright © Sky</em>

    </div>

    </form>
</body>
</html>
