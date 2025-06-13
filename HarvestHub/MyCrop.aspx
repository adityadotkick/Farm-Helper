<%@ Page Title="Home" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="MyCrop.aspx.cs" Inherits="HarvestHub.MyCrop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container my-5">

    <h2 class="text-center mb-5 fw-bold">Discover and Track Your Crops</h2>

    <!-- Saffron Section -->
    <div class="row align-items-center mb-5 py-5 px-4 shadow rounded bg-light">
        <div class="col-md-6">
            <img src="images/saffron.jpg" class="img-fluid rounded crop-image" alt="Saffron">

        </div>
        <div class="col-md-6 text-start">
            <h3 class="fw-bold mb-3">Saffron</h3>
            <p class="text-muted">
                Known as the <strong>"Red Gold"</strong>, saffron is the world's most precious spice. Cultivated with care, saffron blooms into delicate purple flowers that require a controlled, nurturing environment.
                With Harvest Hub, track your saffron bulbs from planting to harvest, ensuring you get the purest strands of saffron right from your home farm.
            </p>
            <asp:Button ID="btnTrackSaffron" runat="server" Text="Start Tracking" CssClass="btn btn-success mt-3" OnClick="btnTrackSaffron_Click" />
        </div>
    </div>

    <!-- Mushroom Section -->
    <div class="row align-items-center mb-5 py-5 px-4 shadow rounded bg-white flex-md-row-reverse">
        <div class="col-md-6">
            <img src="images/mushroom.jpg" class="img-fluid rounded crop-image" alt="Mushroom">

        </div>
        <div class="col-md-6 text-start">
            <h3 class="fw-bold mb-3">Mushroom</h3>
            <p class="text-muted">
                Mushrooms are nature's miracle — fast-growing, nutrient-rich, and perfect for indoor farming.
                From oyster mushrooms to button mushrooms, track every stage of your fungal farming journey.
                Our smart tracking ensures you manage moisture, temperature, and growth phases for a successful mushroom harvest!
            </p>
            <asp:Button ID="btnTrackMushroom" runat="server" Text="Start Tracking" CssClass="btn btn-success mt-3" OnClick="btnTrackMushroom_Click" />
        </div>
    </div>

</div>

</asp:Content>
