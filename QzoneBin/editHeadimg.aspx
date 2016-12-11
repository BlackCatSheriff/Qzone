<%@ Page Language="C#" AutoEventWireup="true" CodeFile="editHeadimg.aspx.cs" Inherits="editHeadimg" %>

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
    
         <asp:Image ID ="imgUserhead" runat="server" BorderColor="Black" ImageUrl="" CssClass="img"/>
                        <asp:FileUpload ID ="FileUpload1" runat ="server"  />
            
                  &nbsp&nbsp
                                 <asp:Button ID="btnSubImg" runat="server" Text ="修改" Width="60px"  OnClick="btnSubImg_Click"/>
                   


    </div>
    </form>
</body>
</html>
