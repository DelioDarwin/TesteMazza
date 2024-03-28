using TesteMazza.Models;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TesteMazza
{
    public partial class PerfilMaster : MasterPage
    {
        static HttpClient client = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }            
        }

        public bool IsMobileBrowser()
        {
            System.Web.HttpBrowserCapabilities myBrowserCaps = Request.Browser;
            if (((System.Web.Configuration.HttpCapabilitiesBase)myBrowserCaps).IsMobileDevice)
            {
                return true;
            }
            else
            {
                return false;
            }
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

        public void ShowMessage(string TipoMensagem, string titulo, Enuns.MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "swal('" + TipoMensagem + "','" + titulo + "','" + type + "');", true);
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

        private void CarregaCliente(Cliente Cliente)
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
