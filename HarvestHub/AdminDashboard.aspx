<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="HarvestHub.AdminDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container-fluid my-5">

    <h2 class="mb-4 text-center">Manage Shop Products</h2>

    <button type="button" class="btn btn-success mb-3" data-bs-toggle="modal" data-bs-target="#addProductModal">
      Add New Product
    </button>

    <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" CssClass="table table-striped table-hover"
                  DataKeyNames="ProductID" OnRowCommand="gvProducts_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="ID">
                <ItemTemplate>
                    <%# Eval("ProductID") %>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                    <%# Eval("ProductName") %>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Description">
                <ItemTemplate>
                    <%# Eval("ProductDescription") %>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Price">
                <ItemTemplate>
                    ₹ <%# Eval("ProductPrice", "{0:N2}") %>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Image">
                <ItemTemplate>
                    <%# Eval("ProductImage") %>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <button type="button" class="btn btn-warning btn-sm me-2" data-bs-toggle="modal" data-bs-target="#editProductModal"
                        onclick="editProduct('<%# Eval("ProductID") %>', '<%# Eval("ProductName") %>', '<%# Eval("ProductDescription") %>', '<%# Eval("ProductPrice") %>', '<%# Eval("ProductImage") %>')">
                        Edit
                    </button>
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger btn-sm" CommandName="DeleteProduct" CommandArgument='<%# Eval("ProductID") %>' OnClientClick="return confirm('Are you sure you want to delete this product?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</div>

<!-- Add Product Modal -->
<div class="modal fade" id="addProductModal" tabindex="-1" aria-labelledby="addProductModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">

      <div class="modal-header">
        <h5 class="modal-title" id="addProductModalLabel">Add New Product</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>

      <div class="modal-body text-start">
        <asp:TextBox ID="txtAddName" runat="server" CssClass="form-control mb-3" Placeholder="Product Name"></asp:TextBox>
        <asp:TextBox ID="txtAddDescription" runat="server" CssClass="form-control mb-3" Placeholder="Product Description"></asp:TextBox>
        <asp:TextBox ID="txtAddPrice" runat="server" CssClass="form-control mb-3" Placeholder="Product Price"></asp:TextBox>
        <!--<asp:TextBox ID="txtAddImage" runat="server" CssClass="form-control mb-3" Placeholder="Image File Name"></asp:TextBox>-->
          <asp:FileUpload ID="fuAddImage" runat="server" CssClass="form-control mb-3" />
          <asp:Label ID="lblImageMessage" runat="server" CssClass="text-danger d-block mb-2" Visible="false" />

      </div>

      <div class="modal-footer">
        <asp:Button ID="btnSaveNewProduct" runat="server" Text="Save Product" CssClass="btn btn-success" OnClick="btnSaveNewProduct_Click" />
      </div>

    </div>
  </div>
</div>

<!-- Edit Product Modal -->
<div class="modal fade" id="editProductModal" tabindex="-1" aria-labelledby="editProductModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">

      <div class="modal-header">
        <h5 class="modal-title" id="editProductModalLabel">Edit Product</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>

      <div class="modal-body text-start">
        <asp:HiddenField ID="hfEditProductID" runat="server" />
        <asp:TextBox ID="txtEditName" runat="server" CssClass="form-control mb-3" Placeholder="Product Name"></asp:TextBox>
        <asp:TextBox ID="txtEditDescription" runat="server" CssClass="form-control mb-3" Placeholder="Product Description"></asp:TextBox>
        <asp:TextBox ID="txtEditPrice" runat="server" CssClass="form-control mb-3" Placeholder="Product Price"></asp:TextBox>
        <asp:TextBox ID="txtEditImage" runat="server" CssClass="form-control mb-3" Placeholder="Image File Name"></asp:TextBox>
      </div>

      <div class="modal-footer">
        <asp:Button ID="btnUpdateProduct" runat="server" Text="Update Product" CssClass="btn btn-warning" OnClick="btnUpdateProduct_Click" />
      </div>

    </div>
  </div>
</div>

<script>
    function editProduct(id, name, description, price, image) {
        document.getElementById('<%= hfEditProductID.ClientID %>').value = id;
        document.getElementById('<%= txtEditName.ClientID %>').value = name;
        document.getElementById('<%= txtEditDescription.ClientID %>').value = description;
        document.getElementById('<%= txtEditPrice.ClientID %>').value = price;
        document.getElementById('<%= txtEditImage.ClientID %>').value = image;
    }
</script>

</asp:Content>
