<%@ Page Title="News" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="News.aspx.cs" Inherits="HarvestHub.News" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div class="container my-5">
    <h2 class="mb-4 text-center fw-bold">Latest Agriculture News</h2>
    <div class="row" id="newsContainer"></div>
</div>

<script>
    fetch('news.json')
        .then(response => response.json())
        .then(news => {
            let html = '';
            news.forEach(n => {
                html += `
                <div class="col-md-4 mb-4">
                    <div class="card h-100 shadow-sm">
                        <img src="${n.ImageUrl}" class="card-img-top" alt="News Image">
                        <div class="card-body">
                            <h6 class="text-muted mb-1">${n.Source}</h6>
                            <h5 class="card-title text-truncate-2">${n.Title}</h5>
                            <p class="card-text text-truncate-4">${n.Description}</p>
                            <a href="${n.Link}" target="_blank" class="btn btn-sm btn-success">Read More</a>
                        </div>
                    </div>
                </div>`;
            });
            document.getElementById("newsContainer").innerHTML = html;
        });
</script>

<style>
    .card-img-top {
        height: 150px;
        object-fit: cover;
    }
    .card-body {
        min-height: 180px;
    }
    .text-truncate-2,
    .text-truncate-4 {
        overflow: hidden;
        display: -webkit-box;
        -webkit-box-orient: vertical;
    }
    .text-truncate-2 {
        -webkit-line-clamp: 2;
    }
    .text-truncate-4 {
        -webkit-line-clamp: 4;
    }
    h6.text-muted {
        font-size: 0.9rem;
        font-weight: 500;
    }
</style>

</asp:Content>
