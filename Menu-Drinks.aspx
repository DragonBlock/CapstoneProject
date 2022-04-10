<%@ Page Title="Menu-Drinks" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Menu-Drinks.aspx.cs" Inherits="CapstoneProject.Menu-Drinks" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="navbar">
        <asp:Button ID="btnMeals" Text="Meals" runat="server" />
        <asp:Button ID="btnSides" Text="Sides" runat="server" />
        <asp:Button ID="btnDrinks" Text="Drinks" runat="server" />
    </div>
    <div class="content">
        <figure class="menuItem" style="padding: 15px;">
            <img alt="" src="" style="height: 15%; width: 10%;" />
            <figcaption style="text-align: center; width: 10%;">Name</figcaption>
            <input id="btnAddItem" type="button" value="Add To Order" style="width: 10%;" />
        </figure>
    </div>
</asp:Content>