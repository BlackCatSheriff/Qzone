<%@ Page Language="C#" AutoEventWireup="true" CodeFile="uploadPhoto.aspx.cs" Inherits="uploadPhoto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    选择相册：上传到
        <asp:DropDownList ID="ddlSeleectFoldder" runat="server"   ></asp:DropDownList>
       
    <br />
        
         <asp:Image ID ="imgUserhead" runat="server" BorderColor="Black"  Width="256px" Height="256px"/>
                        <asp:FileUpload ID ="FileUpload1" runat ="server"  />
            
                  &nbsp&nbsp
                                 <asp:Button ID="btnSubImg" runat="server" Text ="上传" Width="60px"  OnClick="btnSubImg_Click"/>
                   <asp:Button ID="btnReturnAlbum" runat ="server" Text ="返回相册" OnClick="btnReturnAlbum_Click" />
    </div>
    </form>
</body>
</html>
