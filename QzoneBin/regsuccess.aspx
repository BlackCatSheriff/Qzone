<%@ Page Language="C#" AutoEventWireup="true" CodeFile="regsuccess.aspx.cs" Inherits="regsuccess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
img{width:100px;height:100px}
</style> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h2>欢迎你，<asp:Label ID="lbluserNick"  runat ="server"  Text =""></asp:Label>&nbsp;&nbsp; 你的QQ号为：<asp:Label ID="lbluserQQ" runat ="server" Text ="" Font-Bold="true" Font-Size="Larger" ForeColor="Red"></asp:Label></h2>
   
        <br />
        <h3>基本资料：</h3><br />
        
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
        </table>

        <asp:Button ID="btnInfoSub" runat="server" Text ="提交" Font-Bold="true"  Width="50px" Height="20px" OnClick="btnInfoSub_Click" />

        

    </div>
    </form>
</body>
</html>
