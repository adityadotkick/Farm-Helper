<%@ Page Title="Home" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="HarvestHub.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Hero Banner Section -->
    <section class="hero-banner text-white d-flex justify-content-center align-items-center">
        <div class="text-center z-2">
            <h1 class="display-4 fw-bold">Welcome to Harvest Hub!</h1>
            <p class="lead">Manage your crops and shop for supplies easily</p>
            <a href="MyCrop.aspx" class="btn btn-light btn-lg mt-3 crop-btn">Track Your Crops</a>
        </div>
    </section>

    <!-- About Harvest Hub -->
    <section class="container-fluid my-5 pt-3">
        <div class="row justify-content-center">
            <div class="col-md-10 text-center">
                <h2 class="mb-4">About Harvest Hub</h2>
                <p class="fs-5">
                    Harvest Hub is your all-in-one platform to manage indoor farming,
                    track crop growth, buy essential supplies, and connect with a community
                    of smart farmers. Join us and simplify your farming journey!
                </p>
            </div>
        </div>
    </section>

    <!-- Features / Highlights -->
    <section class="bg-light py-5 px-0 w-100">
        <div class="row text-center g-4 mx-0">
            <div class="col-md-3">
                <div class="card h-100 border-0 shadow-sm">
                    <div class="card-body">
                        <i class="bi bi-seedling fs-1 text-success"></i>
                        <h5 class="mt-3 fw-semibold">My Crop Management</h5>
                        <p>Track your crops with step-by-step growth insights and alerts.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card h-100 border-0 shadow-sm">
                    <div class="card-body">
                        <i class="bi bi-cart4 fs-1 text-success"></i>
                        <h5 class="mt-3 fw-semibold">Shop Essentials</h5>
                        <p>Seeds, fertilizers, and indoor tools available at one place.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card h-100 border-0 shadow-sm">
                    <div class="card-body">
                        <i class="bi bi-newspaper fs-1 text-success"></i>
                        <h5 class="mt-3 fw-semibold">Latest News</h5>
                        <p>Stay up-to-date with agri-tech updates and farming articles.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card h-100 border-0 shadow-sm">
                    <div class="card-body">
                        <i class="bi bi-chat-dots fs-1 text-success"></i>
                        <h5 class="mt-3 fw-semibold">Support & Advice</h5>
                        <p>Connect with experts to solve your crop queries instantly.</p>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Farming Tip Section -->
    <section class="container-fluid my-5">
        <div class="row justify-content-center">
            <div class="col-md-10 text-center">
                <h2 class="mb-4">Farming Tip of the Day</h2>
                <div class="p-4 bg-success text-white rounded">
                    <p class="fs-5 mb-0">
                        🌱 Water your indoor saffron plants lightly every 3–4 days to avoid fungal growth and ensure optimal flowering.
                    </p>
                </div>
            </div>
        </div>
    </section>

    <!-- Page Styles -->
    <style>
        body {
            overflow-x: hidden;
        }

        .hero-banner {
            width: 100vw;
            height: 100vh;
            margin-left: calc(-50vw + 50%);
            margin-right: calc(-50vw + 50%);
            background: url('images/home-banner.jpg') top center no-repeat;
            background-size: cover;
            background-color: #198754;
            position: relative;
            margin-top: -2.5rem;
        }

        .hero-banner::before {
            content: "";
            position: absolute;
            inset: 0;
            background-color: rgba(0, 0, 0, 0.4);
            z-index: 1;
        }

        .hero-banner > div {
            position: relative;
            z-index: 2;
        }

        .crop-btn:hover {
            background-color: #198754 !important;
            color: #fff !important;
            transition: all 0.3s ease;
        }

        .card h5 {
            min-height: 48px;
        }

        .card p {
            font-size: 0.95rem;
        }
    </style>

</asp:Content>
