<%@ Page Language="C#" AutoEventWireup="true" CodeFile="log.aspx.cs" Inherits="comment" %>

<!DOCTYPE html>
<script runat="server">

   
</script>


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
           
            <p style="color:darkblue;font-size:larger"> 总浏览量:<% = All %>     今日浏览量: <% = Today %>  </p> 
            
            <br />
            <br />
            <div id="divtoday">
                今日访客:
                <br />

                <asp:Repeater ID="rptToday" runat ="server" OnItemCommand="rptToday_ItemCommand" >
                    <HeaderTemplate><table></HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:ImageButton ID="imgbtnToday" runat ="server" ImageUrl='<%# Eval("Uheadimg") %>' PostBackUrl='<%# "comment.aspx?uqq=" + Eval("LguestQq") %>' Width="40px" Height="40px" CommandName="jmpGuestHome" />
                            </td>
                            <td>
                                <asp:Label ID="lblTodaynike" runat ="server" Text='<%#Eval("Unick") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTodaytime" runat ="server" Text='<%# Eval("LtimeAll") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="btnDellog" runat ="server"  Text ="删除"  CommandName="Dellog"  CommandArgument='<%# Eval("Lid") %>'/>
                            </td>


                        </tr>
                    </ItemTemplate>
                    <FooterTemplate></table></FooterTemplate>
                </asp:Repeater> 



                    <div  id ="divToolT" runat ="server"  visible ="false">


                    <asp:Button ID="btnFirstT" runat="server" Text="首页"  OnClick="btnFirstT_Click" />
              <asp:Button ID="btnUpT" runat="server" Text="上一页" OnClick="btnUpT_Click" />
        <asp:Button ID="btnDrowT" runat="server" Text="下一页"  OnClick ="btnDrowT_Click" />
        
        <asp:Button ID="btnLastT" runat="server" Text="尾页"  OnClick ="btnLastT_Click" />
        页次：<asp:Label ID="lbNowT" runat="server" Text="1" ></asp:Label>
        /<asp:Label ID="lbTotalT" runat="server" Text="1"></asp:Label>

        转<asp:TextBox ID="txtJumpT" Text="1" runat="server" Width="30px" TextMode="Number"></asp:TextBox>
        <asp:Button ID="btnJumpT" runat="server" Text="Go"  OnClick="btnJumpT_Click" />  


        </div>



            </div>


      
        
                      <div id="divBefore">
                          历史访客:<br />
                <asp:Repeater ID="rptBefore" runat ="server" >
                    <HeaderTemplate><table></HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:ImageButton ID="imgbtnBefore" runat ="server" ImageUrl='<%# Eval("Uheadimg") %>' PostBackUrl='<%# "comment.aspx?uqq=" + Eval("LguestQq") %>' Width="40px" Height="40px" CommandName="jmpGuestHome" />
                            </td>
                            <td>
                                <asp:Label ID="lblBeforenike" runat ="server" Text='<%#Eval("Unick") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblBeforetime" runat ="server" Text='<%# Eval("LtimeAll") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="btnDellog" runat ="server"  Text ="删除"  CommandName="Dellog"  CommandArgument='<%# Eval("Lid") %>'/>
                            </td>


                        </tr>
                    </ItemTemplate>
                    <FooterTemplate></table></FooterTemplate>
                </asp:Repeater> 



                              <div  id ="divToolB" runat ="server"  visible ="false">


                    <asp:Button ID="btnFirstB" runat="server" Text="首页"   OnClick="btnFirstB_Click"/>
              <asp:Button ID="btnUpB" runat="server" Text="上一页"  OnClick="btnUpB_Click" />
        <asp:Button ID="btnDrowB" runat="server" Text="下一页"  OnClick="btnDrowB_Click" />
        
        <asp:Button ID="btnLastB" runat="server" Text="尾页"  OnClick="btnLastB_Click" />
        页次：<asp:Label ID="lbNowB" runat="server" Text="1"></asp:Label>
        /<asp:Label ID="lbTotalB" runat="server" Text="1"></asp:Label>

        转<asp:TextBox ID="txtJumpB" Text="1" runat="server" Width="30px" TextMode="Number"></asp:TextBox>
        <asp:Button ID="btnJumpB" runat="server" Text="Go"  OnClick="btnJumpB_Click" />  


        </div>

            </div>

          </div>


        <div  align="center"><img src="img/boot.png" /></div>
        <em>Copyright © Sky</em>

    </div>



    
    
    </form>
</body>
</html>
