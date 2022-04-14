<%@ Page Title="See Cart" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SeeCart.aspx.cs" Inherits="CapstoneProject.SeeCart" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #btnCart {
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
    <div class="content">
        <asp:Panel ID="MainPanel" style="width:100%; height:80%;" runat="server">

        </asp:Panel>
    </div>
</asp:Content>
