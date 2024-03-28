<%@ Page Title="" Language="C#" MasterPageFile="~/Perfil.Master" AutoEventWireup="true" CodeBehind="ListaClientes.aspx.cs" Inherits="TesteMazza.ListaClientes" Async="true" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">

    <link href="css/sweetalert.css" rel="stylesheet" />
    <script src="js/sweetalert.min.js"></script>

    <h5 class="mb-4 pt-sm-3">Clientes</h5>
    <br />
    <div class="table-responsive">  
    <div style="overflow-x:auto;width:1300px">
    <asp:GridView ID="grvCliente" CssClass="table" Font-Size="12px" runat="server" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" GridLines="Horizontal" OnRowCommand="grvCliente_RowCommand">
        <AlternatingRowStyle BackColor="WhiteSmoke" />
        <PagerStyle CssClass="pagination-ys" />
        <Columns> 
            <asp:TemplateField HeaderText="Alterar">
                <ItemTemplate>
                    <asp:ImageButton CausesValidation="false" ID="imbAlterar" CommandName="Alterar" CssClass="btn btn-pill btn-outline-link btn-sm mb-0 mr-0" CommandArgument='<%# Eval("IdCliente") %>' runat="server"  ToolTip="Alterar Cliente" ImageUrl="img/edit.png" ></asp:ImageButton>
                </ItemTemplate> 
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px"/>
            </asp:TemplateField>                              
            <asp:TemplateField HeaderText="Excluir">  
                <ItemTemplate>   
                    <asp:ImageButton ID="imbExcluir" CausesValidation="false" runat="server" CssClass="btn btn-pill btn-outline-link btn-sm mb-0 mr-0" OnClientClick="return deletealert(this.name, event)" CommandArgument='<%#Eval("IdCliente")%>' CommandName="Excluir" ToolTip="Excluir Cliente" ImageUrl="img/delete.png"></asp:ImageButton>
                </ItemTemplate> 
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px"/>
            </asp:TemplateField>
            <asp:BoundField DataField="Nome" HeaderText="Nome" ItemStyle-VerticalAlign="Middle" />  
            <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-VerticalAlign="Middle" />  
        </Columns>
    </asp:GridView>
    </div>
</div>
</asp:Content>
