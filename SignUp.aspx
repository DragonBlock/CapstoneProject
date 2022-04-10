<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="CapstoneProject.SignUp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p style="text-align: center; font-size: 1.8em;">Sign Up</p>
    <div style="padding-left:25%; font-size: 1.5em;" class="text-left">
        Username:<span style="margin-left:15%;"><asp:TextBox ID="tbUser" runat="server"></asp:TextBox></span>
    <br />
        Password:<span style="margin-left:15%;"><asp:TextBox ID="tbPass" runat="server"></asp:TextBox></span>
    <br />
    Email:<span style="margin-left:20%;"><asp:TextBox ID="tbEmail" runat="server"></asp:TextBox></span>
    <br />
    First Name:<span style="margin-left:14%;"><asp:TextBox ID="tbFName" runat="server"></asp:TextBox></span>
    <br />
    Last Name:<span style="margin-left:14%;"><asp:TextBox ID="tbLName" runat="server"></asp:TextBox></span>
    <br />
    Phone Num(Optional):&nbsp;&nbsp;<asp:TextBox ID="tbPhoneNum" runat="server"></asp:TextBox>
    <br />
    <div style="padding-left:18%;"><asp:Button ID="btnSignUp" runat="server" Text="Sign Up" OnClick="btnSignUp_Click" Width="160px"  /></div>
        <div style="text-align:center; margin-right:25%;"><asp:Label  ID="lblMessage" runat="server"></asp:Label></div>
    </div>
</asp:Content>
