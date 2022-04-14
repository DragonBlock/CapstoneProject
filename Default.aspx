<%@ Page Title="Home Page" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CapstoneProject._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #btnHome {
            background-color: royalblue;
        }

        .ImgDiv {
            width: 98%;
        }
        .MenuImg {
            width: 100%;
            height: 200px;
            object-fit: fill;
        }
    </style>
    <div class="ImgDiv">
        <asp:Image ID="menuImg1" runat="server" ImageUrl="~/Images/Pizza.jpg" ImageAlign="Middle" CssClass="MenuImg" />
    </div>
    <br />
    <div class="ImgDiv">
        <asp:Image ID="menuImg2" runat="server" ImageUrl="~/Images/Sushi.jpg" ImageAlign="Middle" CssClass="MenuImg" />
    </div>
</asp:Content>
