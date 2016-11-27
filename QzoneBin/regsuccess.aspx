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
    <h2>欢迎你，<asp:Label ID="lbluserNick"  runat ="server"  Text =""></asp:Label>你的QQ号为：<asp:Label ID="lbluserQQ" runat ="server" Text ="" Font-Bold="true" Font-Size="Larger" ForeColor="Red"></asp:Label></h2>
   
        <br />
        <h3>基本资料：</h3><br />
        <table>
            <tr>
                <td>
                <asp:Image ID ="imgUserhead" runat="server" BorderColor="Black" ImageUrl="" CssClass="img"/>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                    <asp:FileUpload ID ="FileUpload1" runat ="server"  BorderStyle="Outset"  />
                    
                   
                    &nbsp&nbsp<asp:Button ID="btnChangeHead" runat="server" Text ="修改" BorderStyle="Outset" OnClick="btnChangeHead_Click" />
                </td>
            </tr>
            <tr>
               <td>昵称:</td> <td><asp:TextBox ID ="txtUserNike" runat ="server"  MaxLength="15" Text=""></asp:TextBox></td>
                <td>性别：</td><td><asp:DropDownList ID ="ddlSex" runat ="server" AutoPostBack="True"  ><asp:ListItem Selected="True">男</asp:ListItem><asp:ListItem>女</asp:ListItem></asp:DropDownList></td>
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
