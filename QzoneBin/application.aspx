<%@ Page Language="C#" AutoEventWireup="true" CodeFile="application.aspx.cs" Inherits="comment" %>

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
      <h2 >与我相关@</h2>
       <asp:Repeater ID="rptApply"  runat ="server" OnItemCommand="rptApply_ItemCommand"  >
                <HeaderTemplate><table></HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:ImageButton ID="imgbtnFriend" runat ="server"  Width="40px" Height="40px" ImageUrl='<%# Eval("Uheadimg") %>'  PostBackUrl=  '<%# "comment.aspx?uqq="+Eval("Uqq") %>' CommandName="jumphome"/>
                        </td>
                       <td>
                         <asp:Label ID="lblFriendName" runat ="server" Text ='<%# "好友   "+ Eval("Unick")+ "      关注了你" %>'></asp:Label>
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








      </div>
    <div  id ="divTool" runat ="server"  visible ="false">


                    <asp:Button ID="btnFirst" runat="server" Text="首页"  />
              <asp:Button ID="btnUp" runat="server" Text="上一页"  />
        <asp:Button ID="btnDrow" runat="server" Text="下一页"  />
        
        <asp:Button ID="btnLast" runat="server" Text="尾页"  />
        页次：<asp:Label ID="lbNow" runat="server" Text="1"></asp:Label>
        /<asp:Label ID="lbTotal" runat="server" Text="1"></asp:Label>

        转<asp:TextBox ID="txtJump" Text="1" runat="server" Width="30px" TextMode="Number"></asp:TextBox>
        <asp:Button ID="btnJump" runat="server" Text="Go"  />  


        </div>



 


    
    
    </form>
</body>
</html>
