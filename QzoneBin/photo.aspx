<%@ Page Language="C#" AutoEventWireup="true" CodeFile="photo.aspx.cs" Inherits="photo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="div1" runat ="server">
      <h3>相册 <% = ablbumName  %> 内容：</h3>
        <br />
        <asp:Repeater ID="rptphotos" runat ="server" OnItemCommand="rptphotos_ItemCommand" >
            <HeaderTemplate><table></HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:Image ID="imgPhoto" runat ="server" ImageUrl='<%# Eval("PimgPath") %>'  Width="125px" Height ="125px" />
                    
                 </td>
                        <td>
                     
                       <asp:Label  runat="server" ID="lblPhotoName" Text='<%#"名字："+ Eval("PimgName") %>'></asp:Label>
                 </td>   
                    <td>
                       
                       <asp:Label ID="lblPhotoTime" runat="server" Text='<%# "上传时间："+ Eval("Ptime") %>' ></asp:Label>
                     </td>
                    <td>
                        <asp:Button ID="btnSetSurface" runat ="server" Text ="设为封面" CommandName="SetSurface"  CommandArgument='<%# Eval("Pid") %>'/>
                    </td>
                    <td>

                         <asp:Button ID="btnDelPhoto" runat ="server"  Text ="删除"  CommandName="Delpoto" CommandArgument='<%# Eval("Pid") %>'/>
                         &nbsp&nbsp
                        <asp:Button ID="btnRename" runat ="server" Text ="改名" CommandName="ChangeName" CommandArgument='<%# Eval("Pid") %>' />
                        <asp:TextBox ID="txtRename" runat="server" Visible="false"  ></asp:TextBox>
                       <asp:Button ID="btnConfirm" runat ="server"   Text="修改" Visible="false" CommandName="ConfirmChange"  CommandArgument='<%# Eval("Pid") %>'/>
                       
                     </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate></table></FooterTemplate>
        </asp:Repeater>
    
    
     <div  id ="divTool" runat ="server"  visible ="false">


                    <asp:Button ID="btnFirst" runat="server" Text="首页" OnClick="btnFirst_Click" />
              <asp:Button ID="btnUp" runat="server" Text="上一页" OnClick="btnUp_Click" />
        <asp:Button ID="btnDrow" runat="server" Text="下一页"  OnClick="btnDrow_Click"/>
        
        <asp:Button ID="btnLast" runat="server" Text="尾页"  OnClick="btnLast_Click"/>
        页次：<asp:Label ID="lbNow" runat="server" Text="1"></asp:Label>
        /<asp:Label ID="lbTotal" runat="server" Text="1"></asp:Label>

        转<asp:TextBox ID="txtJump" Text="1" runat="server" Width="30px" TextMode="Number"></asp:TextBox>
        <asp:Button ID="btnJump" runat="server" Text="Go"  OnClick="btnJump_Click"/>  


        </div>

    
       <div  align="center"><img src="img/boot.png" /></div>
        <em>Copyright © Sky</em>
    
    
    </div>


        <div id="div404" runat="server" style="background-color:black;width:100%;height:2000px;color:red;font-size:xx-large"  Visible="false">

            <h1 margin-left: auto; margin-right: auto; >不要瞎搞ooo  ^_^</h1>
            <br />
            <br />
            <asp:Button ID="btnexit" runat ="server"  Text ="我知道了"  Font-Bold="true" Font-Size="XX-Large" Font-Italic="true" OnClick="btnexit_Click" />
        </div>




    </form>
</body>
</html>
