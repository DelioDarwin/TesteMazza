using TesteMazza.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Win32;
using Microsoft.Ajax.Utilities;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace TesteMazza
{
    public partial class CadCliente : System.Web.UI.Page
    {

        static HttpClient client = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Form.Enctype = "multipart/form-data";
            ScriptManager.GetCurrent(this).RegisterPostBackControl(this.btnCadastrarCliente);
            ScriptManager.GetCurrent(this).RegisterPostBackControl(this.btnCadastrarEndereco);
            ScriptManager.GetCurrent(this).RegisterPostBackControl(this.btnSelecionarFoto1);

            //Abertura (Sem Postback)
            if (!Page.IsPostBack)
            {
                if (Request["IdCliente"] != null)
                {
                    Cliente cliente = new Cliente();
                    cliente = RetornaDadosCliente(Convert.ToInt64(Request["IdCliente"])).Result;

                    if (cliente != null)
                    {

                        Session["Cliente"] = cliente;
                        txtNome.Value = cliente.Nome;
                        txtEmail.Value = cliente.Email;
                        CarregaFoto(cliente);
                        divEndereco.Style.Add("display", "block");
                        Session["ModoCliente"] = "A";
                        Session["ModoEndereco"] = "I";
                        btnCadastrarCliente.Attributes.Add("class", "btn btn-warning");
                        btnCadastrarCliente.InnerText = "Alterar";

                        CarregaEnderecos();
                    }
                }

            }
        }

        private void CarregaCliente(Cliente cliente)
        {
            Label lblNome = (Label)Page.Master.FindControl("lblNome");
            lblNome.Text = cliente.Nome;

            txtNome.Value = cliente.Nome;
            txtEmail.Value = cliente.Email;
        }

        public async Task<Cliente> RetornaDadosCliente(Int64 iIdCliente)
        {
            Cliente ClienteRetorno = null;

            Rotas endpoint = new Rotas();
            endpoint.sEndPoint_Cliente_RetornaDadosCliente = endpoint.sEndPoint_Cliente_RetornaDadosCliente + iIdCliente.ToString();

            HttpResponseMessage response = client.GetAsync(endpoint.sEndPoint_Cliente_RetornaDadosCliente).Result;

            if (response.IsSuccessStatusCode)
            {
                string sRetorno = response.Content.ReadAsStringAsync().Result;
                ClienteRetorno = JsonConvert.DeserializeObject<Cliente>(sRetorno);
            }
            else
            {
                ClienteRetorno = null;
            }

            return ClienteRetorno;
        }

        protected void btnCadastrarCliente_Click(object sender, EventArgs e)
        {
            if (Session["Cliente"] == null)
            {
                if (TrataCamposCliente())
                {

                    Task<Cliente> Cliente = IncluirCliente();

                    if (Cliente.Status.ToString() == "Faulted")
                    {
                        ShowMessage("Atenção", "É preciso executar a API", Enuns.MessageType.error);
                    }
                    else
                    {
                        Session["Cliente"] = RetornaDadosCliente(Cliente.Result.IdCliente).Result;
                        ShowMessage("Atenção", "Dados do cliente cadastrados", Enuns.MessageType.success);
                        divEndereco.Style.Add("display", "block");
                        Session["ModoCliente"] = "A";
                        Session["ModoEndereco"] = "I";
                        btnCadastrarCliente.Attributes.Add("class", "btn btn-warning");
                        btnCadastrarCliente.InnerText = "Alterar";
                    }

                }
            }
            else
            {
                Task<Cliente> cliente = Alterar();
                if (cliente.IsCompleted)
                {
                    ShowMessage("Atenção", "Dados do cliente alterados", Enuns.MessageType.success);
                    Session["ModoCliente"] = "A";
                }
            }
        }

        private async Task<Cliente> Alterar()
        {
            Cliente clienteRetorno = null;
            Cliente cliente = new Cliente();

            //Retorna os valores do usuário cadastrado
            Cliente clienteCadastrado = (Cliente)Session["Cliente"];

            cliente.IdCliente = clienteCadastrado.IdCliente;
            cliente.Nome = txtNome.Value.Trim();
            cliente.Email = txtEmail.Value.Trim();

            cliente.DataCadastro = Convert.ToDateTime(DateTime.UtcNow.ToString("o"));

            var serialized = JsonConvert.SerializeObject(cliente);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            Rotas endpoint = new Rotas();
            endpoint.sEndPoint_Cliente_AlterarDados = endpoint.sEndPoint_Cliente_AlterarDados + cliente.IdCliente.ToString();

            HttpResponseMessage result = client.PutAsync(endpoint.sEndPoint_Cliente_AlterarDados, content).Result;

            if (result.IsSuccessStatusCode)
            {
                string sRetorno = result.Content.ReadAsStringAsync().Result;
                clienteRetorno = JsonConvert.DeserializeObject<Cliente>(sRetorno);
            }
            else
            {
                clienteRetorno = null;
            }

            //Atribui endereço do usuário na sessão
            Session["Cliente"] = clienteRetorno;
            return clienteRetorno;

        }

        private bool TrataCamposCliente()
        {

            if (this.txtNome.Value == "")
            {
                ShowMessage("Atenção", "Favor informar o seu Nome", Enuns.MessageType.warning);
                return false;
            }

            if (this.txtEmail.Value == "")
            {
                ShowMessage("Atenção", "Favor informar a seu Email", Enuns.MessageType.warning);
                return false;
            }

            return true;
        }

        private void CarregaEndereco(Endereco endereco)
        {
            txtCEP.Value = endereco.CEP;
            txtLogradouro.Value = endereco.Logradouro;
            txtNumero.Value = endereco.Numero.ToString();
            txtComplemento.Value = endereco.Complemento;
            txtBairro.Value = endereco.Bairro;
            txtCidade.Value = endereco.Cidade;
            txtEstado.Value = endereco.UF;
            txtPais.Value = endereco.Pais;
        }

        private void LimparEndereco()
        {
            txtCEP.Value = ""; 
            txtLogradouro.Value = "";
            txtNumero.Value = "";
            txtComplemento.Value = "";
            txtBairro.Value = "";
            txtCidade.Value = "";
            txtEstado.Value = "";
            txtPais.Value = "";
        }

        public async Task<Endereco> RetornaEnderecoPorIdEndereco(Int64 iIdEndereco)
        {
            Endereco enderecoRetorno = null;

            Rotas endpoint = new Rotas();
            endpoint.sEndPoint_Endereco_GetByEnderecosPorIdEndereco = endpoint.sEndPoint_Endereco_GetByEnderecosPorIdEndereco + iIdEndereco.ToString();

            HttpResponseMessage response = client.GetAsync(endpoint.sEndPoint_Endereco_GetByEnderecosPorIdEndereco).Result;

            if (response.IsSuccessStatusCode)
            {
                string sRetorno = response.Content.ReadAsStringAsync().Result;
                enderecoRetorno = JsonConvert.DeserializeObject<Endereco>(sRetorno);
            }
            else
            {
                //Response.Write("alert('Não foi possível obter os dados:')" + response.StatusCode);
                enderecoRetorno = null;
            }

            return enderecoRetorno;
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Session["Cliente"] = null;
            Session["Endereço"] = null;
            Session["ModoCliente"] = "I";
            Session["ModoEndereco"] = "I";
            Response.Redirect("CadCliente");
        }

        protected void btnCadastrarEndereco_Click(object sender, EventArgs e)
        {
            if (TrataCamposCadastroEndereco())
            {
                //Verifica foi carregado o modeo de inclusão
                if (Session["ModoEndereco"].ToString() == "I")
                {
                    //Cadastra Endereço
                    Task<Endereco> endereco = IncluirEndereco();

                    if (endereco.IsCompleted)
                    {
                        ShowMessage("Atenção", "Endereço Cadastrado", Enuns.MessageType.success);
                        LimparEndereco();
                        CarregaEnderecos();
                    }
                }
                else //Modo de Alteração
                {
                    //Alterar Endereço
                    Task<Endereco> endereco = AlterarEndereco();

                    if (endereco.IsCompleted)
                    {
                        btnCadastrarEndereco.Attributes.Add("class", "btn btn-warning");
                        btnCadastrarEndereco.InnerText = "Cadastrar Endereço";
                        Session["ModoEndereco"] = "I";
                        LimparEndereco();
                        CarregaEnderecos();
                        ShowMessage("Atenção", "Endereço Alterado", Enuns.MessageType.success);
                    }
                }

            }
        }

        private async Task<Cliente> IncluirCliente()
        {
            Cliente clienteRetorno = null;
            Cliente cliente = new Cliente();

            cliente.CodigoCliente = Guid.NewGuid();
            cliente.Nome = txtNome.Value.ToString();
            cliente.Email = txtEmail.Value.Trim();
            cliente.DataCadastro = Convert.ToDateTime(DateTime.UtcNow.ToString("o"));
            cliente.Foto = "img/FotoPerfil.jpg";

            var serialized = JsonConvert.SerializeObject(cliente);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            Rotas endpoint = new Rotas();
            HttpResponseMessage result = client.PostAsync(endpoint.sEndPoint_Cliente, content).Result;

            if (result.IsSuccessStatusCode)
            {
                string sRetorno = result.Content.ReadAsStringAsync().Result;
                clienteRetorno = JsonConvert.DeserializeObject<Cliente>(sRetorno);
            }
            else
            {
                clienteRetorno = null;
            }

            return clienteRetorno;

        }

        protected void grvEndereco_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Alterar")
            {
                Endereco endereco = new Endereco();
                endereco = RetornaEnderecoPorIdEndereco(Convert.ToInt64(e.CommandArgument)).Result;

                btnCadastrarEndereco.Attributes.Add("class", "btn btn-warning");
                btnCadastrarEndereco.InnerText = "Alterar Endereço";
                Session["ModoEndereco"] = "A";

                if (endereco != null)
                {
                    Session["Endereco"] = endereco;
                    CarregaEndereco(endereco);
                }
            }
            else if (e.CommandName.ToString() == "Excluir")
            {

                Task<bool> retorno = ExcluirEndereco(Convert.ToInt64(e.CommandArgument.ToString()));

                if (retorno.IsCompleted)
                {
                    Session["Endereco"] = null;
                    CarregaEnderecos();
                    ShowMessage("Sucesso", "O endereço foi excluído.", Enuns.MessageType.success);

                }
            }
        }


        private async Task<bool> ExcluirEndereco(Int64 iIdEndereco)
        {
            Rotas endpoint = new Rotas();
            endpoint.sEndPoint_Endereco_ExcluirEndereco += iIdEndereco.ToString();
            HttpResponseMessage result = client.DeleteAsync(endpoint.sEndPoint_Endereco_ExcluirEndereco).Result;

            if (result.IsSuccessStatusCode)
            {
                string sRetorno = result.Content.ReadAsStringAsync().Result;

            }
            else
            {
                return false;
            }

            return true;
        }


        private async Task<Endereco>  IncluirEndereco()
        {
            Endereco enderecoRetorno = null;
            Cliente Cliente = (Cliente)Session["Cliente"];

            Endereco endereco = new Endereco();
            endereco.IdCliente = Cliente.IdCliente;

            endereco.CEP = txtCEP.Value.Replace("-", "");
            endereco.Logradouro = txtLogradouro.Value.Trim();
            endereco.Numero = Convert.ToInt32(txtNumero.Value.Trim());
            endereco.Complemento = txtComplemento.Value.Trim();
            endereco.Bairro = txtBairro.Value.Trim();
            endereco.Cidade = txtCidade.Value.Trim();
            endereco.UF = txtEstado.Value.Trim();
            endereco.Pais = txtPais.Value.Trim();

            var serialized = JsonConvert.SerializeObject(endereco);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            Rotas endpoint = new Rotas();
            HttpResponseMessage result = client.PostAsync(endpoint.sEndPoint_Endereco, content).Result;

            if (result.IsSuccessStatusCode)
            {
                string sRetorno = result.Content.ReadAsStringAsync().Result;
                enderecoRetorno = JsonConvert.DeserializeObject<Endereco>(sRetorno);
            }
            else
            {
                enderecoRetorno = null;
            }

            return enderecoRetorno;

        }

        private async Task<Endereco> AlterarEndereco()
        {
            Endereco enderecoRetorno = null;
            Endereco endereco = new Endereco();

            //Retorna os valores do usuário cadastrado
            Cliente Cliente = (Cliente)Session["Cliente"];

            //Retorna os valores do endereço cadastrado
            Endereco enderecoCadastrado = (Endereco)Session["Endereco"];

            endereco.IdCliente = Cliente.IdCliente;
            endereco.IdEndereco = enderecoCadastrado.IdEndereco;

            endereco.CEP = txtCEP.Value.Replace("-", "");
            endereco.Logradouro = txtLogradouro.Value.Trim();
            endereco.Numero = Convert.ToInt32(txtNumero.Value.Trim());
            endereco.Complemento = txtComplemento.Value.Trim();
            endereco.Bairro = txtBairro.Value.Trim();
            endereco.Cidade = txtCidade.Value.Trim();
            endereco.UF = txtEstado.Value.Trim();
            endereco.Pais = txtPais.Value.Trim();

            var serialized = JsonConvert.SerializeObject(endereco);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            Rotas endpoint = new Rotas();
            endpoint.sEndPoint_Endereco_AlterarEndereco = endpoint.sEndPoint_Endereco_AlterarEndereco + enderecoCadastrado.IdEndereco.ToString();

            HttpResponseMessage result = client.PutAsync(endpoint.sEndPoint_Endereco_AlterarEndereco, content).Result;

            if (result.IsSuccessStatusCode)
            {
                string sRetorno = result.Content.ReadAsStringAsync().Result;
                enderecoRetorno = JsonConvert.DeserializeObject<Endereco>(sRetorno);
            }
            else
            {
                enderecoRetorno = null;
            }

            //Atribui endereço do usuário na sessão
            Session["Endereco"] = enderecoRetorno;
            CarregaEndereco(enderecoRetorno);

            return enderecoRetorno;

        }

        public void CarregaEnderecos()
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();

            Cliente cliente = new Cliente();
            cliente = (Cliente)Session["Cliente"];

            if (cliente == null)
                Response.Redirect("CadCliente.aspx");

            con = new SqlConnection(Util.stringConexao);
            con.Open();

            string CommandText = "Select * from Endereco Where IdCliente = " + cliente.IdCliente.ToString();


            cmd = new SqlCommand(CommandText);
            cmd.Connection = con;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            adapter.Fill(dt);

            if (con.State == ConnectionState.Open)
                con.Close();

            if (dt.Rows.Count > 0)
            {
                //DivAlertaSemAnuncios.Style.Add("display", "none");

                grvEndereco.DataSource = dt;
                grvEndereco.DataBind();
            }
            else
            {
                grvEndereco.DataSource = null;
                grvEndereco.DataBind();
            }

        }

        private bool TrataCamposCadastroEndereco()
        {
            if (this.txtCEP.Value == "")
            {
                ShowMessage("Atenção", "Favor informar o CEP", Enuns.MessageType.warning);
                return false;
            }

            if (this.txtLogradouro.Value == "")
            {
                ShowMessage("Atenção", "Favor informar o Logradouro", Enuns.MessageType.warning);
                return false;
            }

            if (this.txtNumero.Value == "")
            {
                ShowMessage("Atenção", "Favor informar o Numero", Enuns.MessageType.warning);
                return false;
            }

            if (this.txtBairro.Value == "")
            {
                ShowMessage("Atenção", "Favor informar o Bairro", Enuns.MessageType.warning);
                return false;
            }

            if (this.txtCidade.Value == "")
            {
                ShowMessage("Atenção", "Favor informar a Cidade", Enuns.MessageType.warning);
                return false;
            }

            if (this.txtEstado.Value == "")
            {
                ShowMessage("Atenção", "Favor informar o Estado", Enuns.MessageType.warning);
                return false;
            }

            if (this.txtPais.Value == "")
            {
                ShowMessage("Atenção", "Favor informar o País", Enuns.MessageType.warning);
                return false;
            }

            return true;
        }

        public void ShowMessage(string TipoMensagem, string titulo, Enuns.MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "swal('" + TipoMensagem + "','" + titulo + "','" + type + "');", true);
        }



        protected void btnSelecionarFoto1_Click(object sender, EventArgs e)
        {
            if (TrataCamposCadastro(FileUpload1))
            {
                UploadFoto(FileUpload1);

                if (Session["CaminhoFotoPerfil"] != null)
                {
                    imgFoto.Src = Session["CaminhoFotoPerfil"].ToString();
                    imgFoto.Visible = true;
                }
            }
        }

        private async void UploadFoto(FileUpload fp)
        {
            if (fp.HasFile)
            {
                Cliente Cliente = new Cliente();
                Cliente = (Cliente)Session["Cliente"];

                string caminhoFoto = "~/foto_perfil/" + Cliente.IdCliente.ToString() + "/";
                //string caminhoAnuncio = "~/fotos_anuncios/1/";

                var savePath = Server.MapPath(caminhoFoto);
                string fileExtension = System.IO.Path.GetExtension(fp.FileName).ToLower();

                string nomeFotoOriginal = "original_" + Cliente.IdCliente + fileExtension;
                string nomeFoto = Cliente.IdCliente + "_" + DateTime.Now.ToString("ddMMyyyHHmmss") + fileExtension;
                //string nomeFoto = "1_" + incremento.ToString() + fileExtension;

                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                savePath += nomeFotoOriginal;
                fp.SaveAs(savePath);

                Char[] buffer;
                string sCaminhoDestino = Server.MapPath(caminhoFoto + nomeFoto);

                using (var sr = new StreamReader(savePath))
                {
                    buffer = new Char[(int)sr.BaseStream.Length];
                    sr.ReadAsync(buffer, 0, (int)sr.BaseStream.Length).Wait();

                    Compressimage(sr.BaseStream, sCaminhoDestino, nomeFoto);
                }

                if (System.IO.File.Exists(savePath))
                    System.IO.File.Delete(savePath);

                imgFoto.Src = caminhoFoto.Replace("~/", "") + nomeFoto;

                Session["CaminhoFotoPerfil"] = caminhoFoto.Replace("~/", "") + nomeFoto;
                Session["CaminhoFisicoFotoPerfil"] = sCaminhoDestino;

                Task<Cliente> anuncio = AtualizarFoto(Session["CaminhoFotoPerfil"].ToString());
            }

        }

        private bool TrataCamposCadastro(FileUpload fp)
        {
            //Verifica se o usuário incluiu imagens com extensão válidas
            if (fp.HasFile)
            {
                if (TrataImagens(fp) == false)
                {
                    ShowMessage("Atenção", "As fotos devem possuir a extensão: .gif, .jpeg, .jpg ou .png", Enuns.MessageType.warning);
                    return false;
                }
            }

            return true;
        }



        private bool TrataImagens(FileUpload fp)
        {
            bool bValido = false;
            string fileExtension = System.IO.Path.GetExtension(fp.FileName).ToLower();
            foreach (string ext in new string[] { ".gif", ".jpeg", ".jpg", ".png" })
            {
                if (fileExtension == ext)
                    bValido = true;
            }

            return bValido;
        }

        private void CarregaFoto(Cliente Cliente)
        {
            if (Cliente != null)
            {
                if (Cliente.Foto != null)
                {
                    var savePath = Server.MapPath(Cliente.Foto.ToString());

                    //Verifica se existe a imagem no diretório
                    if (System.IO.File.Exists(savePath))
                    {
                        if (Cliente.Foto == null)
                            imgFoto.Src = "img/FotoPerfil.jpg";
                        else
                            imgFoto.Src = Cliente.Foto.ToString();
                    }
                    else
                    {
                        imgFoto.Src = "img/FotoPerfil.jpg";
                    }
                }
                else
                {
                    imgFoto.Src = "img/FotoPerfil.jpg";
                }

            }
        }


        private async Task<Cliente> AtualizarFoto(string sCaminhoFoto)
        {
            Cliente ClienteRetorno = null;

            //Retorna os valores do usuário cadastrado
            Cliente Cliente = (Cliente)Session["Cliente"];

            Cliente.Foto = sCaminhoFoto;


            var serialized = JsonConvert.SerializeObject(Cliente);
            var content = new StringContent(serialized, Encoding.UTF8, "application/json");

            Rotas endpoint = new Rotas();
            endpoint.sEndPoint_Cliente_AtualizarFoto += Cliente.IdCliente.ToString();
            HttpResponseMessage result = client.PutAsync(endpoint.sEndPoint_Cliente_AtualizarFoto, content).Result;

            if (result.IsSuccessStatusCode)
            {
                string sRetorno = result.Content.ReadAsStringAsync().Result;
                ClienteRetorno = JsonConvert.DeserializeObject<Cliente>(sRetorno);
            }
            else
            {
                ClienteRetorno = null;
            }

            Session["Cliente"] = ClienteRetorno;

            return ClienteRetorno;
        }

        public static void Compressimage(Stream sourcePath, string targetPath, String filename)
        {
            try
            {
                using (var image = System.Drawing.Image.FromStream(sourcePath))
                {
                    float maxHeight = 900.0f;
                    float maxWidth = 900.0f;
                    int newWidth;
                    int newHeight;
                    string extension;
                    Bitmap originalBMP = new Bitmap(sourcePath);

                    if (Array.IndexOf(originalBMP.PropertyIdList, 274) > -1)
                    {
                        var orientation = (int)originalBMP.GetPropertyItem(274).Value[0];
                        switch (orientation)
                        {
                            case 1:
                                // No rotation required.
                                break;
                            case 2:
                                originalBMP.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                break;
                            case 3:
                                originalBMP.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                break;
                            case 4:
                                originalBMP.RotateFlip(RotateFlipType.Rotate180FlipX);
                                break;
                            case 5:
                                originalBMP.RotateFlip(RotateFlipType.Rotate90FlipX);
                                break;
                            case 6:
                                originalBMP.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                break;
                            case 7:
                                originalBMP.RotateFlip(RotateFlipType.Rotate270FlipX);
                                break;
                            case 8:
                                originalBMP.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                break;
                        }

                        // This EXIF data is now invalid and should be removed.
                        originalBMP.RemovePropertyItem(274);
                    }


                    int originalWidth = originalBMP.Width;
                    int originalHeight = originalBMP.Height;

                    if (originalWidth > maxWidth || originalHeight > maxHeight)
                    {
                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;
                        float ratioY = (float)maxHeight / (float)originalHeight;
                        float ratio = Math.Min(ratioX, ratioY);
                        newWidth = (int)(originalWidth * ratio);
                        newHeight = (int)(originalHeight * ratio);
                    }
                    else
                    {
                        newWidth = (int)originalWidth;
                        newHeight = (int)originalHeight;

                    }

                    Bitmap bitMAP1 = new Bitmap(originalBMP, newWidth, newHeight);
                    Graphics imgGraph = Graphics.FromImage(bitMAP1);

                    extension = Path.GetExtension(targetPath);
                    if (extension == ".png" || extension == ".gif")
                    {
                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        //imgGraph.RotateTransform(-90);
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);

                        //bitMAP1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        bitMAP1.Save(targetPath, image.RawFormat);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();
                    }
                    else if (extension == ".jpg" || extension == ".jpeg")
                    {
                        imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                        imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);
                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                        myEncoderParameters.Param[0] = myEncoderParameter;

                        bitMAP1.Save(targetPath, jpgEncoder, myEncoderParameters);

                        bitMAP1.Dispose();
                        imgGraph.Dispose();
                        originalBMP.Dispose();

                    }
                }

            }
            catch (Exception)
            {
                throw;

            }
        }


        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

    }
}