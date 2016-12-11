<%@ Page Language="C#" AutoEventWireup="true" CodeFile="photodetial.aspx.cs" Inherits="photodetial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1><% =Nike %> 的照片</h1>
        
        <br />
        <a>照片名字:<%=PhotoName %>  发表时间：<% = PhotoTime %></a>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<asp:Button ID="btnreturn" runat ="server" Text ="返回" OnClick="btnreturn_Click" /><br />
        
        
       
        <asp:Image ID="imgPhoto" runat ="server"  Width="1024px" Height ="768px" />
    
    </div>
    </form>
</body>
</html>
