<%@ Page Title="Accounts" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Accounts.aspx.cs" Inherits="HarvestHub.Accounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid d-flex align-items-center justify-content-center">
        <div class="card p-5 shadow" style="max-width: 400px; width: 100%; border-radius: 20px;">

            <!-- Tabs for User and Admin -->
            <ul class="nav nav-pills mb-4 justify-content-center" id="accountTabs" role="tablist">
                <li class="nav-item" role="presentation">
                    <button class="nav-link active" id="user-tab" data-bs-toggle="pill" data-bs-target="#user" type="button" role="tab" aria-controls="user" aria-selected="true">
                        User
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link" id="admin-tab" data-bs-toggle="pill" data-bs-target="#admin" type="button" role="tab" aria-controls="admin" aria-selected="false">
                        Admin
                    </button>
                </li>
            </ul>

            <div class="tab-content" id="accountTabsContent">

                <!-- User Login/Register -->
                <div class="tab-pane fade show active" id="user" role="tabpanel" aria-labelledby="user-tab">

                    <!-- User Login Section -->
                    <div id="userLoginSection">
                        <h4 class="text-center mb-4">User Login</h4>

                        <asp:TextBox ID="txtUserLoginUsername" runat="server" CssClass="form-control mb-3" Placeholder="Username"></asp:TextBox>
                        <asp:TextBox ID="txtUserLoginPassword" runat="server" CssClass="form-control mb-3" Placeholder="Password" TextMode="Password"></asp:TextBox>

                        <asp:Button ID="btnUserLogin" runat="server" CssClass="btn btn-success w-100 mb-3" Text="Login" OnClick="btnUserLogin_Click" />

                        <div class="text-center">
                            <button type="button" class="btn btn-link text-success" onclick="showRegister()">New User? Register Here</button>
                        </div>

                        <asp:Label ID="lblUserMessage" runat="server" CssClass="text-danger d-block mt-3 text-center"></asp:Label>
                    </div>

                    <!-- User Register Section (Initially Hidden) -->
                    <div id="userRegisterSection" style="display: none;">
                        <h4 class="text-center mb-4">Register New User</h4>

                        <asp:TextBox ID="txtNewFullName" runat="server" CssClass="form-control mb-2" Placeholder="Full Name"></asp:TextBox>
                        <asp:TextBox ID="txtNewUsername" runat="server" CssClass="form-control mb-2" Placeholder="Choose Username"></asp:TextBox>
                        <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control mb-3" Placeholder="Choose Password" TextMode="Password"></asp:TextBox>

                        <asp:Button ID="btnRegister" runat="server" CssClass="btn btn-outline-success w-100 mb-3" Text="Register" OnClick="btnRegister_Click" />

                        <div class="text-center">
                            <button type="button" class="btn btn-link text-success" onclick="showLogin()">Back to Login</button>
                        </div>

                        <asp:Label ID="lblRegisterMessage" runat="server" CssClass="text-danger d-block mt-3 text-center"></asp:Label>
                    </div>

                </div>

                <!-- Admin Login Section -->
                <div class="tab-pane fade" id="admin" role="tabpanel" aria-labelledby="admin-tab">
                    <h4 class="text-center mb-4">Admin Login</h4>

                    <asp:TextBox ID="txtAdminUsername" runat="server" CssClass="form-control mb-3" Placeholder="Admin Username"></asp:TextBox>
                    <asp:TextBox ID="txtAdminPassword" runat="server" CssClass="form-control mb-3" Placeholder="Admin Password" TextMode="Password"></asp:TextBox>

                    <asp:Button ID="btnAdminLogin" runat="server" CssClass="btn btn-success w-100 mb-3" Text="Login" OnClick="btnAdminLogin_Click" />

                    <asp:Label ID="lblAdminMessage" runat="server" CssClass="text-danger d-block mt-3 text-center"></asp:Label>
                </div>

            </div>
        </div>
    </div>

    <!-- JavaScript to Switch Sections -->
    <script>
        function showRegister() {
            document.getElementById("userLoginSection").style.display = "none";
            document.getElementById("userRegisterSection").style.display = "block";
        }
        function showLogin() {
            document.getElementById("userRegisterSection").style.display = "none";
            document.getElementById("userLoginSection").style.display = "block";
        }
    </script>

</asp:Content>
