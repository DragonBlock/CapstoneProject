<%@ Page Title="SeeCart" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SeeCart.aspx.cs" Inherits="CapstoneProject.SeeCart" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #btnCart {
            background-color: royalblue;
        }
    </style>
    <div class="content">
        <figure class="menuItem" style="padding: 15px;">
            <img alt="" src="" style="height: 15%; width: 10%;" />
            <figcaption style="text-align: center; width: 10%;">Name</figcaption>
            <asp:Button ID="btnRemoveItem" runat="server" text="Remove From Order" width="10%" />
        </figure>
    </div>
</asp:Content>