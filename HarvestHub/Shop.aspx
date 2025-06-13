<%@ Page Title="Shop" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Shop.aspx.cs" Inherits="HarvestHub.Shop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblDebug" runat="server" CssClass="text-danger" />

    <section class="container-fluid my-4">
        <div class="row justify-content-center">
            <div class="col-md-10 text-center">
                <h2 class="fw-bold mb-3">Available Products</h2>
                <p class="fs-5 text-muted">Indoor farming essentials for saffron and mushroom cultivation.</p>
            </div>
        </div>
    </section>

    <section class="container-fluid px-0">
        <asp:ListView ID="ProductsListView" runat="server" OnItemCommand="ProductsListView_ItemCommand">
            <LayoutTemplate>
                <div class="row g-4 mx-0">
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                </div>
            </LayoutTemplate>

            <ItemTemplate>
                <div class="col-md-4">
                    <div class="card h-100 shadow-sm">
                        <img src='<%# "images/" + Eval("ProductImage") %>' class="card-img-top" style="height: 200px; object-fit: cover;">
                        <div class="card-body text-center">
                            <h5 class="card-title mb-2"><%# Eval("ProductName") %></h5>
                            <p class="card-text text-muted mb-2"><%# Eval("ProductDescription") %></p>
                            <p class="fw-bold text-success fs-5">₹<%# Eval("ProductPrice") %></p>

                            <div class="input-group mb-3 justify-content-center mx-auto" style="max-width: 140px;">
                                <asp:LinkButton ID="btnDecreaseQty" runat="server" CommandName="DecreaseQty" CommandArgument='<%# Eval("ProductID") %>' CssClass="btn btn-outline-success btn-sm">-</asp:LinkButton>
                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control text-center qty-input" Text='<%# Eval("CartQuantity").ToString() == "0" ? "1" : Eval("CartQuantity") %>' ReadOnly="true" style="width: 40px; height: 30px; font-size: 14px; padding: 0;" />
                                <asp:LinkButton ID="btnIncreaseQty" runat="server" CommandName="IncreaseQty" CommandArgument='<%# Eval("ProductID") %>' CssClass="btn btn-outline-success btn-sm">+</asp:LinkButton>
                            </div>

                            <asp:Button ID="btnAddToCart" runat="server" CommandName="AddToCart" CommandArgument='<%# Eval("ProductID") %>' CssClass="btn btn-success btn-sm w-100" Text='<%# Eval("CartQuantity").ToString() == "0" ? "Add to Cart" : "Update Quantity" %>' />
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:ListView>
    </section>

    <style>
        body { overflow-x: hidden; }
        .qty-input {
            height: 30px;
            font-size: 14px;
            padding: 0;
        }
    </style>
</asp:Content>