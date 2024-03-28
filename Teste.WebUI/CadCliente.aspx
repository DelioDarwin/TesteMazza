<%@ Page Title="" Language="C#" MasterPageFile="~/Perfil.Master" AutoEventWireup="true" CodeBehind="CadCliente.aspx.cs" Inherits="TesteMazza.CadCliente" Async="true" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
        <link href="css/sweetalert.css" rel="stylesheet" />
        <script src="js/sweetalert.min.js"></script>

          <h5 class="mb-4 pt-sm-3">Cadastro de Clientes</h5>
          <br />
          <div class="row">
            <div class="col-md-6">
              <div class="form-group">
                <label for="account-fn">Nome</label>
                <input class="form-control" type="text" id="txtNome" runat="server" required>
              </div>
            </div>
            <div class="col-md-6">
              <div class="form-group">
                <label for="txtEmail">Email</label>
                <input class="form-control" type="email" id="txtEmail" runat="server">
              </div>
            </div>
 
          </div>
            <div class="row">
                <div class="col-md-12">
                    <hr class="mt-3 pb-3">
                    <div class="text-sm-right">
                        <button class="btn btn-success" runat="server" onserverclick="btnCadastrarCliente_Click" id="btnCadastrarCliente">Salvar Cliente</button>
                    </div>   
                </div>
            </div>

        <br />

         <div style="display: none" id="divEndereco" runat="server">

                <h5 class="mb-4 pt-sm-3">Endereço</h5>
                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtCEP">CEP</label>
                            <input class="form-control" type="text" id="txtCEP" runat="server" />
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtLogradouro">Logradouro</label>
                            <input class="form-control" type="text" id="txtLogradouro" runat="server" maxlength="200">
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtNumero">Numero</label>
                            <input class="form-control" type="text" id="txtNumero" runat="server">
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtComplemento">Complemento</label>
                            <input class="form-control" type="text" id="txtComplemento" runat="server" maxlength="50">
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtBairro">Bairro</label>
                            <input class="form-control" type="text" id="txtBairro" runat="server" maxlength="100">
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtCidade">Cidade</label>
                            <input class="form-control" type="text" id="txtCidade" runat="server" maxlength="100">
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtEstado">Estado</label>
                            <input class="form-control" type="text" id="txtEstado" runat="server" maxlength="2">
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label for="txtPais">País</label>
                            <input class="form-control" type="text" id="txtPais" runat="server" maxlength="30">
                        </div>
                    </div>
                </div>

                <hr class="mt-3 pb-3">
                <div class="text-sm-right">
                    <button class="btn btn-primary" runat="server" onserverclick="btnCadastrarEndereco_Click" id="btnCadastrarEndereco">Salvar Endereço</button>
                </div>       

            </div>
    <br /><br />




       <div class="table-responsive">  
       <div style="overflow-x:auto;width:1300px">
       <asp:GridView ID="grvEndereco" CssClass="table" Font-Size="12px" runat="server" AutoGenerateColumns="False" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" GridLines="Horizontal" OnRowCommand="grvEndereco_RowCommand">
           <AlternatingRowStyle BackColor="WhiteSmoke" />
           <PagerStyle CssClass="pagination-ys" />
           <Columns> 
               <asp:TemplateField HeaderText="Alterar">
                   <ItemTemplate>
                       <asp:ImageButton CausesValidation="false" ID="imbAlterar" CommandName="Alterar" CssClass="btn btn-pill btn-outline-link btn-sm mb-0 mr-0" CommandArgument='<%# Eval("IdEndereco") %>' runat="server"  ToolTip="Alterar Endereço" ImageUrl="img/edit.png" ></asp:ImageButton>
                   </ItemTemplate> 
                   <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px"/>
               </asp:TemplateField>                              
               <asp:TemplateField HeaderText="Excluir">  
                   <ItemTemplate>   
                       <asp:ImageButton ID="imbExcluir" CausesValidation="false" runat="server" CssClass="btn btn-pill btn-outline-link btn-sm mb-0 mr-0" OnClientClick="return deletealert(this.name, event)" CommandArgument='<%#Eval("IdEndereco")%>' CommandName="Excluir" ToolTip="Excluir Endereço" ImageUrl="img/delete.png"></asp:ImageButton>
                   </ItemTemplate> 
                   <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="30px"/>
               </asp:TemplateField>
               <asp:BoundField DataField="CEP" HeaderText="CEP" ItemStyle-VerticalAlign="Middle" />  
               <asp:BoundField DataField="Logradouro" HeaderText="Logradouro" ItemStyle-VerticalAlign="Middle" />  
               <asp:BoundField DataField="Numero" HeaderText="Numero" ItemStyle-VerticalAlign="Middle" />    
               <asp:BoundField DataField="Bairro" HeaderText="Bairro" ItemStyle-VerticalAlign="Middle" />    
               <asp:BoundField DataField="UF" HeaderText="UF" ItemStyle-VerticalAlign="Middle" />    
           </Columns>
       </asp:GridView>
       </div>
   </div>


    <script type="text/javascript">

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    bindcontrols();
                }
            });
        };

        function bindcontrols() {
            $("#<%=txtCEP.ClientID%>").mask('00000-000');
            $("#<%=txtNumero.ClientID%>").mask('000000');
        };

        $(document).ready(function ($) {
            $("#<%=txtCEP.ClientID%>").mask('00000-000');
            $("#<%=txtNumero.ClientID%>").mask('000000');
        });

    </script>

</asp:Content>
