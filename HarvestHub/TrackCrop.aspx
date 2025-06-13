<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="TrackCrop.aspx.cs" Inherits="HarvestHub.TrackCrop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container my-5">
    <h2 class="text-center mb-4 fw-bold">Track My Crops</h2>

    <!-- Crop and Yield Selection -->
    <div class="mb-4 text-center">
        <label class="form-label fw-semibold">Select Crop:</label>
        <asp:DropDownList ID="ddlAvailableCrops" runat="server" CssClass="form-select w-25 d-inline-block" AutoPostBack="true" OnSelectedIndexChanged="ddlAvailableCrops_SelectedIndexChanged" />

        <label class="form-label fw-semibold ms-4">Select Yield:</label>
        <asp:DropDownList ID="ddlYield" runat="server" CssClass="form-select w-25 d-inline-block" AutoPostBack="true" OnSelectedIndexChanged="ddlYield_SelectedIndexChanged" />

        <asp:Button ID="btnStartTracking" runat="server" Text="Start Tracking" CssClass="btn btn-success ms-3" OnClick="btnStartTracking_Click" />
    </div>

    <!-- Horizontal Stepper UI -->
    <div class="stepper-container mb-5">
        <div class="step done">
            <div class="circle"><i class="bi bi-check-lg"></i></div>
            <p>Planting</p>
        </div>
        <div class="step active">
            <div class="circle">2</div>
            <p>Watering</p>
        </div>
        <div class="step">
            <div class="circle">3</div>
            <p>Pesticide</p>
        </div>
        <div class="step">
            <div class="circle">4</div>
            <p>Harvest</p>
        </div>
    </div>

    <!-- Activity Tracker -->
    <h4 class="mt-5 mb-3 fw-semibold">Tracking Progress</h4>
    <asp:Repeater ID="rptCropActivities" runat="server" OnItemCommand="rptCropActivities_ItemCommand">
        <ItemTemplate>
            <div class="card mb-4 shadow-sm tracker-card">
                <div class="card-header d-flex justify-content-between align-items-center bg-light">
                    <h5 class="mb-0"><%# Eval("StepName") %></h5>
                    <span class="badge 
                        <%# Eval("Status").ToString() == "Completed" ? "bg-success" : 
                             Eval("Status").ToString() == "InProgress" ? "bg-primary" : "bg-secondary" %>">
                        <%# Eval("Status") %>
                    </span>
                </div>
                <div class="card-body">
                    <p><strong>Temperature Required:</strong> <%# Eval("TemperatureInfo") %></p>
                    <p><strong>Notes:</strong> <%# Eval("Notes") %></p>
                    <p><strong>Due Date:</strong> <%# Eval("DueDate", "{0:dd MMM yyyy}") %></p>

                    <asp:Button ID="btnMarkComplete" runat="server" Text="Mark as Completed" CommandName="MarkCompleted" CommandArgument='<%# Eval("ActivityID") %>' CssClass="btn btn-outline-success btn-sm mt-2"
                        Visible='<%# Eval("Status").ToString() != "Completed" %>' />
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

<!-- Internal CSS -->
<style>
    .tracker-card {
        border-left: 5px solid #198754;
        transition: transform 0.3s ease;
    }
    .tracker-card:hover {
        transform: translateY(-5px);
    }
    .card-header {
        background-color: #f8f9fa;
    }
    .badge {
        font-size: 0.9rem;
        padding: 8px 12px;
    }
    .card-body p {
        margin-bottom: 8px;
        font-size: 15px;
    }
    .stepper-container {
        display: flex;
        justify-content: space-around;
        align-items: center;
        margin-top: 20px;
    }
    .step {
        text-align: center;
        position: relative;
        flex: 1;
    }
    .step:not(:last-child)::after {
        content: '';
        position: absolute;
        top: 20px;
        right: -50%;
        width: 100%;
        height: 2px;
        background: #ccc;
        z-index: 0;
    }
    .circle {
        width: 40px;
        height: 40px;
        border-radius: 50%;
        line-height: 40px;
        background-color: #ddd;
        color: #000;
        margin: auto;
        font-weight: bold;
        z-index: 1;
    }
    .step.active .circle {
        background-color: #ff7f50;
        color: white;
    }
    .step.done .circle {
        background-color: #0d6efd;
        color: white;
    }
    .step p {
        margin-top: 8px;
        font-size: 14px;
    }
</style>

</asp:Content>
