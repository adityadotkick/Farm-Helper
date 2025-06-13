<%@ Page Title="Buy Now" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="BuyNow.aspx.cs" Inherits="HarvestHub.BuyNow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section class="container-fluid my-4">
        <div class="row justify-content-center">
            <div class="col-md-10 text-center">
                <h2 class="fw-bold mb-3">Buy Now</h2>
                <p class="fs-5 text-muted">Select quantity and add items to your cart.</p>
            </div>
        </div>
    </section>

    <section class="container-fluid px-0">
        <asp:ListView ID="BuyNowListView" runat="server">
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
                            <h5 class="card-title"><%# Eval("ProductName") %></h5>
                            <p class="text-muted"><%# Eval("ProductDescription") %></p>
                            <p class="fw-bold text-success">₹<%# Eval("ProductPrice") %></p>

                <div class="input-group mb-2 mx-auto" style="max-width: 140px;">
                <button class="btn btn-outline-success btn-sm" type="button" onclick="updateQty(this, -1)">-</button>
                <input type="text" class="form-control text-center qty-input" value="1" readonly style="width: 40px;">
                <button class="btn btn-outline-success btn-sm" type="button" onclick="updateQty(this, 1)">+</button>
                </div>


                                                            <asp:Button ID="addtocart" Text="Add To cart" CssClass="btn btn-outline-light" OnClick="AddToCart(this, <%# Eval("ProductID") %>)" />

                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:ListView>
    </section>

    <script>
        
        function updateQty(btn, delta) {
            const input = btn.parentElement.querySelector(".qty-input");
            let value = parseInt(input.value);
            value = Math.max(1, Math.min(10, value + delta));
            input.value = value;
        }

        function addToCart(button, productId) {
            const card = button.closest(".card");
            const qty = parseInt(card.querySelector(".qty-input").value);

            $.ajax({
                type: "POST",
                url: "BuyNow.aspx.cs/AddToCart",
                data: JSON.stringify({ productId: productId, quantity: qty }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    if (res.d === "NotLoggedIn") {
                        alert("Please login to continue.");
                        window.location.href = "Accounts.aspx";
                    } else {
                        alert("Item added to cart!");
                    }
                },
                error: function (xhr, status, error) {
                    alert("Error: " + error);
                }
            });
        }
    </script>
</asp:Content>
