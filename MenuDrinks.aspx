<%@ Page Title="Menu-Drinks" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MenuDrinks.aspx.cs" Inherits="CapstoneProject.MenuDrinks" %>

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

        #btnDrinks {
            background-color: royalblue;
        }
    </style>
    <div class="menuNav">
        <asp:Button CssClass="menuLink" ID="btnMeals" Text="Meals" runat="server" OnClick="btnMeals_Click" />
        <asp:Button CssClass="menuLink" ID="btnSides" Text="Sides" runat="server" OnClick="btnSides_Click" />
        <asp:Button CssClass="menuLink" ID="btnDrinks" Text="Drinks" runat="server" />
    </div>
    <div class="content">
        <figure class="menuItem" style="padding: 15px;">
            <img alt="" src="" style="height: 15%; width: 10%;" />
            <figcaption style="text-align: center; width: 10%;">Name</figcaption>
            <input id="btnAddItem" type="button" value="Add To Order" style="width: 10%;" />
        </figure>
    </div>
</asp:Content>