<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Regester.aspx.cs" Inherits="Regester" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
.tables{  
        width:500px;  
        margin:auto;  
        border:1px solid #000000;  
        border-collapse:collapse;  
    }  
.tables th,tr,td{  
        border:1px solid #ffffff;  
    }  
.tables th{ background-color:#ffffff;}  
.tables tr:hover{background-color:#ffffff;}  
        
        .auto-style4 {
            width: 429px;
        }
        .auto-style5 {
            width: 429px;
            height: 19px;
        }
    </style>
</head>
<body>
    

    <form id="form1" runat="server">
        <table class="tables">  
       
          &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp  &nbsp &nbsp &nbsp &nbsp&nbsp &nbsp &nbsp&nbsp &nbsp &nbsp&nbsp &nbsp &nbsp&nbsp &nbsp &nbsp&nbsp  <asp:Label ID="lblTitle" runat ="server" Text ="用户注册" Font-Size ="Larger" ForeColor ="Black" Font-Bold ="true" ></asp:Label>
            
        <tr>  
            <td class="auto-style4">昵称： &nbsp&nbsp&nbsp <asp:TextBox ID ="txtUsername" runat ="server" MaxLength ="20" ></asp:TextBox> 
                
                <br />
                <br />
            </td>  
 
        </tr>  
        <tr>  
            <td class="auto-style4">密码：&nbsp&nbsp&nbsp&nbsp <asp:TextBox ID ="txtPassword1" runat ="server" TextMode ="Password" MaxLength ="15"  ></asp:TextBox><br />
                  <br />
            </td>  
    
        </tr>  
                <tr>  
            <td class="auto-style4">确认密码：&nbsp &nbsp<asp:TextBox ID ="txtPassword2" runat ="server" TextMode ="Password" MaxLength ="15"  ></asp:TextBox>
              

                  <asp:CompareValidator ID="CompareValidator_Pwd" runat="server" ErrorMessage="密码输入不一致" ForeColor ="Red" 
                       ControlToValidate="txtPassword2" ControlToCompare ="txtPassword1"
                    ></asp:CompareValidator>
                <br />
                <br />
                    </td>  

        </tr>  
              <tr>  
            <td class="auto-style5">E-mail：&nbsp&nbsp&nbsp <asp:TextBox ID ="txtEmail" runat ="server" MaxLength ="20" ></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator_Email" runat="server" ErrorMessage="请输入合法邮件地址！" ControlToValidate ="txtEmail" ForeColor ="Red" ValidationExpression ="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"></asp:RegularExpressionValidator>
                <br />
                <br />
                  </td>  
    
        </tr> 
              <tr>  
            <td class="auto-style4">手机号：&nbsp&nbsp&nbsp <asp:TextBox ID ="txtPhone" runat ="server"  MaxLength ="11"></asp:TextBox>
    
                <asp:RegularExpressionValidator ID="RegularExpressionValidator_phone" runat="server" ErrorMessage="请输入合法手机号！" ForeColor ="Red" ControlToValidate ="txtPhone" ValidationExpression ="1[3,4,5,8]\d[\s,-]?\d{4}[\s,-]?\d{4}" ></asp:RegularExpressionValidator>
    
                <br />
                <br />
                  </td>  
    
        </tr> 
                    <tr>  
            <td class="auto-style4">验证码(数字):&nbsp<asp:TextBox ID ="txtValidator" runat ="server" ></asp:TextBox>
                <asp:ImageButton ID ="imgValidator" runat ="server" OnClick="imgValidator_Click" ImageUrl ="~/ValidatorPage.aspx"  /> &nbsp;
                
                
                <br />
                <br />
                <br />
                        </td>  
    
        </tr> 
                    <tr>  
            <td class="auto-style4">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:Button ID="btnRegester" runat="server" Height="28px" Text="注册" Width="85px" OnClick="btnRegester_Click" /></td>  
    
        </tr> 
                    <tr>  
            <td class="auto-style4">&nbsp;</td>  
    
        </tr> 
                    <tr>  
            <td class="auto-style4">&nbsp;</td>  
    
        </tr> 
    </table> 
    

    <div style="height: 182px">
  



        
    </div>
    </form>
</body>
</html>
