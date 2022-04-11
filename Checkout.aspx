<%@ Page Title="Checkout" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="CapstoneProject.Checkout" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="float: right; background-color: #ccc; height: 275px; width: 25%; margin-right: 20px; margin-top: 40px">
    </div>
    <div style="margin-top: 20px;">
        <label for="payment">Payment Method: </label>
        <select name="payment" id="payment">
            <option value="cash">Cash</option>
            <option value="card">Card</option>
        </select>
    </div>
    <br>
    <asp:Button ID="btnCard" runat="server" Text="Process Card Through PayPal" Width="265px" />
    <br>
    <p style="width: 17%; text-align: center;">Deliver Address</p>
    <div>
        <p style="float: left; text-align: right; width: 6%;">Address: </p>
        <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
    </div>
    <div style="margin-top: 10px;">
        <p style="float: left; text-align: right; width: 6%;">City: </p>
        <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
    </div>
    <div style="margin-top: 10px;">
        <p style="float: left; text-align: right; width: 6%;">Zip Code: </p>
        <asp:TextBox ID="txtZip" runat="server"></asp:TextBox>
    </div>
    <div style="margin-top: 10px;">
        <p style="float: left; text-align: right; width: 6%;">State: </p>
        <asp:TextBox ID="txtState" runat="server"></asp:TextBox>
    </div>
    <div style="margin-top: 10px;">
        <p style="float: left; text-align: right; width: 6%;">Country: </p>
        <asp:TextBox ID="txtCountry" runat="server"></asp:TextBox>
        <div style="float: right; margin-right: 8%; width: 10%; margin-bottom: 20px">
            <asp:Button ID="btnCheckout" runat="server" Text="Checkout" Width="100%" />
        </div>
    </div>
</asp:Content>
