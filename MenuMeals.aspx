<%@ Page Title="MenuMeals" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MenuMeals.aspx.cs" Inherits="CapstoneProject.MenuMeals" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .menuNav {
            background-color: #333;
            overflow: hidden;
            width: 100%;
            float: left;
        }

        .menuLink {
            float: left;
            background-color: lightslategray;
            color: #f2f2f2;
            text-align: center;
            padding: 12px 14px;
            text-decoration: none;
            font-size: 15px;
            width: 8%;
        }

        .menuLink:hover {
            background-color: #ddd;
            color: black;
        }

        .menuLink:active {
            background-color: lightslategray;
            color: white;
        }

        #btnMenu {
            background-color: royalblue;
        }

        #btnMeals {
            background-color: royalblue;
        }
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
    </style>
    <div class="menuNav">
        <asp:Button CssClass="menuLink" ID="btnMeals" Text="Meals" runat="server" />
        <asp:Button CssClass="menuLink" ID="btnSides" Text="Sides" runat="server" OnClick="btnSides_Click" />
        <asp:Button CssClass="menuLink" ID="btnDrinks" Text="Drinks" runat="server" OnClick="btnDrinks_Click" />
    </div>
    <div class="content" >
        <asp:Panel ID="MainPanel" style="width:100%; height:80%;" runat="server">

        </asp:Panel>
    </div>
</asp:Content>