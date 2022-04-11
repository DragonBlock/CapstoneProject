<%@ Page Title="MenuMeals" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MenuMeals.aspx.cs" Inherits="CapstoneProject.MenuMeals" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .MenuItem 
        { 
            width: 10%; height: 20%; float: left;
        }
        .MenuImage 
        {
            height: 80%; width: 100%;
        }
        .MenuLabel 
        {
            text-align: center;
            width: 100%;
            height: 10%;
        }
        .MenuButton 
        {
            width: 100%;
            height: 10%;
        }

        #btnMenu {
            background-color: royalblue;
        }

        #btnMeals {
            background-color: royalblue;
        }
    </style>
    <div class="navbar">
        <asp:Button ID="btnMeals" Text="Meals" runat="server" />
        <asp:Button ID="btnSides" Text="Sides" runat="server" />
        <asp:Button ID="btnDrinks" Text="Drinks" runat="server" />
    </div>
    <div class="content" >
        <asp:Panel ID="MainPanel" style="width:100%; height:80%;" runat="server">

        </asp:Panel>
    </div>
</asp:Content>