<%@ Page Language="C#" AutoEventWireup="true" CodeFile="forgetpwd.aspx.cs" Inherits="forgetpwd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>
        忘记密码：
    </h1>
        QQ号：<asp:TextBox  ID="txtqq" runat ="server" ></asp:TextBox><br />
        验证码：<asp:TextBox ID="txtyzm" runat ="server" MaxLength="7" Width ="70px" ></asp:TextBox>
        <asp:Button ID="btnGetyzm" runat ="server" Text ="获取验证码"  OnClick="btnGetyzm_Click"/>
        <br />
        <br />

       <asp:Button ID="btnsubyzm" runat="server" Text ="验证" OnClick="btnsubyzm_Click" />
       
        

        <div id ="divmm" runat ="server"  visible="false">
        <br />

         新密码：<asp:TextBox ID="txtpwd" runat ="server" MaxLength="18" TextMode="Password"></asp:TextBox><br />
        再次确认：<asp:TextBox ID="txtrepeatpwd" runat ="server" MaxLength ="18" TextMode="Password"></asp:TextBox>
        <br />
         <asp:Button ID="btnSubpwd" runat ="server"   Visible="true"  Text ="提交" OnClick="btnSubpwd_Click" />
            </div>

    </div>
    </form>
</body>
</html>
