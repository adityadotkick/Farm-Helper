<%@ Page Title="Cart" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="HarvestHub.Cart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container my-5">
    <h2 class="text-center mb-4 fw-bold">Your Cart</h2>

    <!-- Debug Label for Errors -->
    <asp:Label ID="lblDebug" runat="server" CssClass="text-danger text-center d-block mb-3" Visible="false" />

    <!-- Validation Summary -->
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="alert alert-danger" DisplayMode="BulletList" 
                           ShowSummary="true" HeaderText="Please correct the following errors:" ValidationGroup="PlaceOrderGroup" />

    <!-- Cart Table -->
    <asp:Panel ID="CartTable" runat="server" Visible="false">
        <div class="table-responsive mb-4 shadow-sm">
            <table class="table table-bordered align-middle text-center">
                <thead class="table-light">
                    <tr>
                        <th>Product</th>
                        <th>Price (₹)</th>
                        <th>Quantity</th>
                        <th>Subtotal (₹)</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="CartRepeater" runat="server" OnItemCommand="CartRepeater_ItemCommand">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("ProductName") %></td>
                                <td>₹<%# Eval("ProductPrice") %></td>
                                <td>
                                    <div class="input-group justify-content-center mx-auto" style="max-width: 120px;">
                                        <asp:Button ID="btnDecrease" runat="server" CssClass="btn btn-outline-success btn-sm" Text="-" 
                                                    CommandName="DecreaseQty" CommandArgument='<%# Eval("ProductID") %>' />
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control text-center" 
                                                     Text='<%# Eval("Quantity") %>' ReadOnly="true" Style="width: 40px;" />
                                        <asp:Button ID="btnIncrease" runat="server" CssClass="btn btn-outline-success btn-sm" Text="+" 
                                                    CommandName="IncreaseQty" CommandArgument='<%# Eval("ProductID") %>' />
                                    </div>
                                </td>
                                <td>₹<%# Eval("Subtotal") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td colspan="3" class="text-end fw-bold">Total</td>
                        <td class="fw-bold text-success fs-5"><asp:Label ID="lblTotal" runat="server" /></td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </asp:Panel>

    <!-- Empty Cart Message -->
    <asp:Label ID="lblEmpty" runat="server" CssClass="text-muted text-center d-block" Text="No items in your cart." Visible="false" />

    <!-- Billing Form -->
    <h4 class="mb-3">Billing Information</h4>
    <div id="billingForm">
        <div class="row g-3 mb-4">
            <div class="col-md-6">
                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Full Name"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                    ErrorMessage="Full Name is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="PlaceOrderGroup" />
            </div>
            <div class="col-md-6">
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Phone Number"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                    ErrorMessage="Phone Number is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="PlaceOrderGroup" />
                <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone"
                    ErrorMessage="Phone Number must be exactly 10 digits." CssClass="text-danger" Display="Dynamic"
                    ValidationExpression="^\d{10}$" ValidationGroup="PlaceOrderGroup" />
            </div>
            <div class="col-12">
                <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" placeholder="Delivery Address"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress"
                    ErrorMessage="Delivery Address is required." CssClass="text-danger" Display="Dynamic" ValidationGroup="PlaceOrderGroup" />
            </div>
        </div>
        <div class="d-grid d-md-flex justify-content-md-end">
            <asp:Button ID="btnPlaceOrder" runat="server" CssClass="btn btn-success px-4" Text="Place Order" 
                        OnClick="btnPlaceOrder_Click" ValidationGroup="PlaceOrderGroup" />
        </div>
    </div>
</div>

<style>
    #cartTable td, #cartTable th {
        vertical-align: middle;
    }
    .alert-danger {
        background-color: #f8d7da;
        border-color: #f5c6cb;
        color: #721c24;
        padding: 10px;
        margin-bottom: 15px;
    }
    .input-group .form-control {
        height: 30px;
        font-size: 14px;
        padding: 0;
    }
</style>

</asp:Content>