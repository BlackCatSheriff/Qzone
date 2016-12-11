<%@ Page Language="C#" AutoEventWireup="true" CodeFile="editInfo.aspx.cs" Inherits="comment" %>

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
            
            <asp:Button ID ="btnbaseinfo" runat ="server" Text ="基本资料"  OnClick="btnbaseinfo_Click"/>  &nbsp&nbsp&nbsp&nbsp
            
            <asp:Button ID="btnadcvanceInfo" runat ="server" Text ="高级资料" OnClick="btnadcvanceInfo_Click" />
               
            <br />

        <div id="divbaseinfo" runat ="server" >
      <div class="content" id ="divbooksearch" runat ="server" visible ="true" ><!--主面板的内容放在content内，content为一级版面，不要轻易编辑该层内容-->
         <div id="iframe-wrap">
        <iframe name="inner1" id="iframe" runat ="server"  src="editHeadimg.aspx" width="550px" height="110px" frameborder="no" border="0" marginwidth="0" marginheight="0" scrolling="no" allowtransparency="yes" >
        </iframe>
        <style>
		
		#iframe-wrap { height: 100%; overflow: visible; position: relative; top: 0; z-index: 0 }
		.tablet-width iframe { height: 100px!important }
		.mobile-width iframe { height: 100px!important }
		.mobile-width-2 iframe { height: 100px!important }
		.mobile-width-3 iframe { height: 100px!important }
		</style>
    </div>
         <!--嵌入外部html-->
         
		</div> <!--一级版面-->

        <table>
           
            <tr>
               <td>昵称:</td> <td><asp:TextBox ID ="txtUserNike" runat ="server"  MaxLength="15" Text=""></asp:TextBox></td>
                <td>性别：</td><td><asp:DropDownList ID ="ddlSex" runat ="server" AutoPostBack="false"  ><asp:ListItem Selected="True">男</asp:ListItem><asp:ListItem>女</asp:ListItem></asp:DropDownList></td>
            </tr>
            <tr>
                <td>生日:</td><td><asp:TextBox ID ="txtBirthday" runat ="server" Text ="" MaxLength="10"></asp:TextBox></td>
                <td>注册日期:</td><td><asp:Label ID ="lblStarttime" runat ="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td>E-mail:</td> <td><asp:Label ID ="lblEmail" runat="server" Text =""></asp:Label></td>
                <td>电话:</td><td><asp:Label ID="lblPhone" runat="server" Text =""></asp:Label></td>
            </tr>
            <tr>
                <td>
                    
                        签名档：</td><td>
                            <asp:TextBox ID="txtSign" runat ="server" MaxLength="50" ></asp:TextBox>
                            </td>
                    
                
            </tr>
        </table>

        <asp:Button ID="btnInfoSub" runat="server" Text ="提交" Font-Bold="true"  Width="50px" Height="20px" OnClick="btnInfoSub_Click" />

        

    </div>
      

            <div  id="divadvanceInfo" runat ="server" visible ="false">
               
                <br />
                <table  >
                    <tr>
                       <td>
                        新密码：<asp:TextBox ID="txtpwd" runat ="server" MaxLength="18" TextMode="Password"></asp:TextBox>
                       </td>
                        
                    </tr>

                    <tr>
                        <td>
                            再次确认：<asp:TextBox ID="txtrepeatpwd" runat ="server" MaxLength ="18" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <asp:Button ID="btnSubpwd" runat ="server"   Visible="true"  Text ="提交更改"  OnClick="btnSubpwd_Click"/>
          
                
                
                  </div>


        </div>
      


        <div  align="center"><img src="img/boot.png" /></div>
        <em>Copyright © Sky</em>

  



    
    
    </form>
</body>
</html>
