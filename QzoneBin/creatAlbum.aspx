<%@ Page Language="C#" AutoEventWireup="true" CodeFile="creatAlbum.aspx.cs" Inherits="creatAlbum" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h3 >创建相册</h3>
        相册名字：<asp:TextBox ID="txtAlbumName" runat ="server" ></asp:TextBox>
        <br />
        <asp:Button ID="btnCreatAlbum" runat="server"  Text ="创建"  OnClick="btnCreatAlbum_Click"/>

      

    </div>
    </form>
</body>
</html>
