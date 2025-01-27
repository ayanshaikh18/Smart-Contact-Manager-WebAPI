﻿<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="WebClient.Account.Dashboard" EnableEventValidation="false"%>

<asp:Content ID="content" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row m-sm-4">
            
            <div class="col-md-6 col-xl-6 mt-sm-4">
                <a href="/Contacts/ContactList.aspx" id="ContactListCard">
                <div class="card shadow border-primary border-top-0 border-bottom-0 border-right-0 rounded-bottom py-2" style="border-width:5px !important">
                    <div class="card-body">
                        <div class="row align-items-center no-gutters">
                            <div class="col mr-2">
                                <div class="text-uppercase text-info font-weight-bold text-xs mb-1"><span>My Contacts</span></div>
                                <div class="text-dark font-weight-bold h5 mb-0"><asp:Label ID="ContactLength" runat="server">0</asp:Label></div>
                            </div>
                            <div class="col-auto"><i class="fa fa-user fa-2x text-gray-300 text-info"></i></div>
                        </div>
                    </div>
                </div>
                </a>
            </div>
            
            <div class="col-md-6 col-xl-6 mt-sm-4">
                <a href="/Groups/GroupList.aspx" id="GroupListCard">
                <div class="card shadow border-success border-top-0 border-bottom-0 border-right-0 py-2" style="border-width:5px !important">
                    <div class="card-body">
                        <div class="row align-items-center no-gutters">
                            <div class="col mr-2">
                                <div class="text-uppercase text-success font-weight-bold text-xs mb-1"><span>My Groups</span></div>
                                <div class="text-dark font-weight-bold h5 mb-0"><asp:Label ID="GroupLength" runat="server">0</asp:Label></div>
                            </div>
                            <div class="col-auto"><i class="fa fa-users fa-2x text-gray-300 text-success"></i></div>
                        </div>
                    </div>
                </div>
                </a>
            </div>
        </div>
        <br />
        <br />
        <div class="container">
            <div class="card">
                <div class="card-header text-center text-light font-weight-bold mb-4 bg-dark">
                    Recently Added Contacts
                </div>
                <div class="card-body">
                    <div class="table table-responsive" role="grid">
                        <asp:Table runat="server" ID="ContactsList" CssClass="table my-0">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell>#</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Contact Name</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Contact Number</asp:TableHeaderCell>
                                <asp:TableHeaderCell></asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </div>
                </div>
            </div>
            <br />
            <br />

            <div class="card">
                <div class="card-header text-center text-light font-weight-bold mb-4 bg-dark">
                    Recently Created Groups
                </div>
                <div class="card-body">
                    <div class="table mt-2 table-responsive" role="grid">
                        <asp:Table runat="server" ID="GroupList" CssClass="table my-0">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell>#</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Group Name</asp:TableHeaderCell>
                                <asp:TableHeaderCell></asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </div>
                </div>
            </div>
            <br />
            <br />
            <br />
        </div>
    </div>

</asp:Content>