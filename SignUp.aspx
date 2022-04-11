<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="CapstoneProject.SignUp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p style="text-align: center; font-size: 1.8em;">Sign Up</p>
    <div style="padding-left:25%; font-size: 1.5em;" class="text-left">
        <div style="float: left">
        Username:
        <br />
        Password:
        <br />
        Email:
        <br />
        First Name:
        <br />
        Last Name:
        <br />
        Phone Num(Optional):
        <br />
        </div>
        <div style ="float: left; clear:right;">
            <asp:TextBox ID="tbUser" runat="server" Height="20px"></asp:TextBox>
            <br />
            <asp:TextBox ID="tbPass" runat="server" Height="20px"></asp:TextBox>
            <br />
            <asp:TextBox ID="tbEmail" runat="server" Height="20px"></asp:TextBox>
            <br />
            <asp:TextBox ID="tbFName" runat="server" Height="20px"></asp:TextBox>
            <br />
            <asp:TextBox ID="tbLName" runat="server" Height="20px"></asp:TextBox>
            <br />
            <asp:TextBox ID="tbPhoneNum" runat="server" Height="20px"></asp:TextBox>
        </div><br /><br /><br /><br /><br /><br />
        <div style="padding-left:25%;"><asp:Button ID="btnSignUp" runat="server" Text="Sign Up" OnClick="btnSignUp_Click" Width="160px"  /></div>
        <div style="text-align:center; margin-right:31%;"><asp:Label  ID="lblMessage" runat="server"></asp:Label></div>
    </div>
</asp:Content>
