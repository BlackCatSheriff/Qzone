<%@ Page Language="C#" AutoEventWireup="true" CodeFile="writediary.aspx.cs" Inherits="writediary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>


       
            <asp:Button ID="btnSave" runat ="server" Text ="发表" OnClick="btnSave_Click" />&nbsp&nbsp

          
            <asp:Button ID="btnReturnList" runat ="server" Text="返回日志列表"  OnClick="btnReturnList_Click"/>
            <br /><br />
           标题：（不超过20字） <asp:TextBox ID="txtTile" runat ="server" MaxLength="20"></asp:TextBox><br />
            <asp:TextBox ID="txtDiary" runat ="server" Width="1000" Height ="700" MaxLength="2000"  TextMode="MultiLine"></asp:TextBox>


    
    </div>
    </form>
</body>
</html>
