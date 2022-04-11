<%@ Page Title="View Account" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewAccount.aspx.cs" Inherits="CapstoneProject.ViewAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #btnAccount0 {
            background-color: royalblue;
        }
    </style>
    Username:<asp:Label ID="lblUsername" runat="server" Text="Label"></asp:Label>
<br />
Email:<asp:Label ID="lblEmail" runat="server" Text="Label"></asp:Label>
&nbsp;
    <br />
    <br />
    Old Password:&nbsp;&nbsp; <asp:TextBox ID="tbOldPass" runat="server"></asp:TextBox>
    <br />
    New Password:&nbsp; <asp:TextBox ID="tbNewPass" runat="server"></asp:TextBox>
    <br />
    <asp:Button ID="btnChangePass" runat="server" Text="Change Password" OnClick="btnChangePass_Click" Width="177px" UseSubmitBehavior="False" />
    <br />
    <asp:Label ID="Label1" runat="server" style="clear:right"></asp:Label>
    <br />
    New Email:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox>
    <br />
    <asp:Button ID="btnNewEmail" runat="server" Text="Change Email" OnClick="btnNewEmail_Click" Width="177px" UseSubmitBehavior="False" />
    <br />
    <asp:Label ID="Label2" runat="server" style="clear:right"></asp:Label>
    <br />
    New Username: <asp:TextBox ID="tbNewUser" runat="server"></asp:TextBox>
    <br />
    <asp:Button ID="btnChangeUser" runat="server" Text="Change Username" OnClick="btnNewUsername_Click" Width="177px" UseSubmitBehavior="False" />
    <br />
    <asp:Label ID="Label3" runat="server" style="clear:right"></asp:Label>
    <br />
</asp:Content>
