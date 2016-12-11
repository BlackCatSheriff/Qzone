<%@ Page Language="C#" AutoEventWireup="true" CodeFile="relation.aspx.cs" Inherits="comment" %>

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

       
        </div>
         <div>
           短消息：
             <asp:LinkButton ID="lbtnjump" runat ="server"  ForeColor=  "Red" OnClick="lbtnjump_Click"><% = COUNT %></asp:LinkButton>
             <br />
             <br />
            关注好友列表:<br />
            <asp:Repeater ID="rptFriendsList" runat="server" OnItemCommand="rptFriendsList_ItemCommand">
                <HeaderTemplate><table></HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:ImageButton ID="imgbtnFriend" runat ="server"  Width="40px" Height="40px" ImageUrl='<%# Eval("Uheadimg") %>' PostBackUrl='<%# "comment.aspx?uqq="+Eval("Uqq") %>' CommandName="jumphome" />
                        </td>
                       <td>
                         <asp:Label ID="lblFriendName" runat ="server" Text ='<%# Eval("Unick") %>'></asp:Label>
                    </td>
                        <td>
                            <asp:Label ID="lblState" runat ="server" ></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btnUnfollow" runat ="server" Text ="取消关注" CommandName="Unfollow" CommandArgument='<%# Eval("Uqq") %>' />
                        </td>
                           </tr>

                </ItemTemplate>
                <FooterTemplate></table></FooterTemplate>
            </asp:Repeater>
             <div  id ="divToolF" runat ="server"  visible ="false">


                    <asp:Button ID="btnFirstF" runat="server" Text="首页" OnClick="btnFirstF_Click" />
              <asp:Button ID="btnUpF" runat="server" Text="上一页" OnClick="btnUpF_Click" />
        <asp:Button ID="btnDrowF" runat="server" Text="下一页"  OnClick="btnDrowF_Click"/>
        
        <asp:Button ID="btnLastF" runat="server" Text="尾页"  OnClick="btnLastF_Click"/>
        页次：<asp:Label ID="lbNowF" runat="server" Text="1"></asp:Label>
        /<asp:Label ID="lbTotalF" runat="server" Text="1"></asp:Label>

        转<asp:TextBox ID="txtJumpF" Text="1" runat="server" Width="30px" TextMode="Number"></asp:TextBox>
        <asp:Button ID="btnJumpF" runat="server" Text="Go"  OnClick="btnJumpF_Click"/>  


        </div>




        <div>
            好友搜索：<asp:TextBox ID="txtSearchFrd" runat ="server" TextMode="Number" MaxLength="10"></asp:TextBox>
            <asp:Button ID="btnSearchQQ" runat ="server" Text ="搜索" OnClick="btnSearchQQ_Click" />
            <div id="divSearch" >

                <asp:Repeater ID="rptSearchList" runat="server" OnItemCommand="rptSearchList_ItemCommand">
                <HeaderTemplate><table></HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:ImageButton ID="imgbtnFriend" runat ="server"  Width="40px" Height="40px" ImageUrl='<%# Eval("Uheadimg") %>' PostBackUrl='<%# "comment.aspx?uqq="+Eval("Uqq") %>' />
                        </td>
                       <td>
                         <asp:Label ID="lblFriendName" runat ="server" Text ='<%# Eval("Unick") %>'></asp:Label>
                    </td>
                        <td>
                            <asp:Button ID="btnFollow" runat ="server"  Text="+关注" CommandName="Follow" CommandArgument='<%# Eval("Uqq") %>'></asp:Button>
                        </td>
                           </tr>

                </ItemTemplate>
                <FooterTemplate></table></FooterTemplate>
            </asp:Repeater>

            
             <div  id ="divToolS" runat ="server"  visible ="false">


                    <asp:Button ID="btnFirstS" runat="server" Text="首页"  />
              <asp:Button ID="btnUpS" runat="server" Text="上一页" />
        <asp:Button ID="btnDrowS" runat="server" Text="下一页"  />
        
        <asp:Button ID="btnLastS" runat="server" Text="尾页"  />
        页次：<asp:Label ID="lbNowS" runat="server" Text="1"></asp:Label>
        /<asp:Label ID="lbTotalS" runat="server" Text="1"></asp:Label>

        转<asp:TextBox ID="txtJumpS" Text="1" runat="server" Width="30px" TextMode="Number"></asp:TextBox>
        <asp:Button ID="btnJumpS" runat="server" Text="Go" />  


        </div>
</div>

        </div>


       
            <br />
            <br />
            好友推荐：
            <asp:Repeater ID="rptRecommand"  runat ="server" OnItemCommand="rptRecommand_ItemCommand" >
                <HeaderTemplate><table></HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:ImageButton ID="imgbtnFriend" runat ="server"  Width="40px" Height="40px" ImageUrl='<%# Eval("Uheadimg") %>' PostBackUrl='<%# "comment.aspx?uqq="+Eval("Uqq") %>' />
                        </td>
                       <td>
                         <asp:Label ID="lblFriendName" runat ="server" Text ='<%# Eval("Unick") %>'></asp:Label>
                    </td>
                        <td>
                            <asp:Button ID="btnFollow" runat ="server"  Text="+关注" CommandName="Follow" CommandArgument='<%# Eval("Uqq") %>'></asp:Button>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>

             <div  id ="divToolC" runat ="server"  visible ="false">


                    <asp:Button ID="btnFirstC" runat="server" Text="首页" OnClick="btnFirstC_Click" />
              <asp:Button ID="btnUpC" runat="server" Text="上一页" OnClick="btnUpC_Click" />
        <asp:Button ID="btnDrowC" runat="server" Text="下一页"  OnClick="btnDrowC_Click"/>
        
        <asp:Button ID="btnLastC" runat="server" Text="尾页"  OnClick="btnLastC_Click"/>
        页次：<asp:Label ID="lbNowC" runat="server" Text="1"></asp:Label>
        /<asp:Label ID="lbTotalC" runat="server" Text="1"></asp:Label>

        转<asp:TextBox ID="txtJumpC" Text="1" runat="server" Width="30px" TextMode="Number"></asp:TextBox>
        <asp:Button ID="btnJumpC" runat="server" Text="Go"  OnClick="btnJumpC_Click"/>  


        </div>
            
        </div>






        <div  align="center"><img src="img/boot.png" /></div>
        <em>Copyright © Sky</em>

    



    
    
    </form>
</body>
</html>
