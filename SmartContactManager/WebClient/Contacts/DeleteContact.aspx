﻿<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeleteContact.aspx.cs" Inherits="WebClient.Contacts.DeleteContact" EnableEventValidation="false" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <form style="margin-top: 5%" runat="server">
        <div class="row justify-content-center">
            <div class="jumbotron">
                <h4 class="text-danger">Are you Sure You want to delete this Contact ?</h4>
                <asp:Label ID="ContactData" runat="server" Text=""></asp:Label>
                <br />
                <div class="float-right">
                    <asp:Button CssClass="btn  btn-danger" runat="server" ID="SubmitButton" Text="Delete" OnClick="SubmitButton_Click" />
                    <asp:Button CssClass="btn  btn-secondary" runat="server" ID="CancelButton" Text="Cancel" OnClick="CancelButton_Click" />
                </div>
            </div>
        </div>
    </form>
</asp:Content>