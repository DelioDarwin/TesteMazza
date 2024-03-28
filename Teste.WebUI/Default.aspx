<%@ Page Title="TesteMazza - História e Comércio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TesteMazza._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .imgFundoHistoria{
            background-image: url(img/fundoHistoria.jpg); 
            width: 400px;
            height: 400px;
        }
    </style>

    <!-- Hero section-->
    <section class="container-fluid pt-grid-gutter">
        <div class="row">
            <div class="col-md-12 imgFundoHistoria" style="height: 280px;  width: 100%">
                <div class="px-5">
                    <h2 class="h3 mb-4 pt-4" id="lblAreaHistoria" runat="server" style="text-align:center">Teste MazzaTech</h2>
                </div>
            </div>
        </div>
    </section>

</asp:Content>
