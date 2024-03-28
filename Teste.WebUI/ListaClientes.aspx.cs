using TesteMazza.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;

namespace TesteMazza
{
    public partial class ListaClientes : System.Web.UI.Page
    {
        static HttpClient client = new HttpClient();          

        protected void Page_Load(object sender, EventArgs e)
        {
             
            if (!Page.IsPostBack)
            {
                CarregaClientes();
            }
        }

        public void CarregaClientes()
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            DataTable dt = new DataTable();

            con = new SqlConnection(Util.stringConexao);
            con.Open();

            string CommandText = "Select * from Cliente ";


            cmd = new SqlCommand(CommandText);
            cmd.Connection = con;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            adapter.Fill(dt);

            if (con.State == ConnectionState.Open)
                con.Close();

            if (dt.Rows.Count > 0)
            {
                //DivAlertaSemAnuncios.Style.Add("display", "none");

                grvCliente.DataSource = dt;
                grvCliente.DataBind();
            }
            else
            {
                grvCliente.DataSource = null;
                grvCliente.DataBind();
            }

        }

        protected void grvCliente_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "Alterar")
            {
                Response.Redirect("CadCliente?IdCliente=" + e.CommandArgument.ToString());
            }
            else if (e.CommandName.ToString() == "Excluir")
            {
                Task<bool> retorno = ExcluirCliente(Convert.ToInt64(e.CommandArgument.ToString()));

                if (retorno.IsCompleted)
                {
                    CarregaClientes();
                    ShowMessage("Sucesso", "O cliente foi excluído.", Enuns.MessageType.success);

                }
            }
        }        


        private async Task<bool> ExcluirCliente(Int64 iIdCliente)
        {
            Rotas endpoint = new Rotas();
            endpoint.sEndPoint_Cliente_ExcluirCliente += iIdCliente.ToString();
            HttpResponseMessage result = client.DeleteAsync(endpoint.sEndPoint_Cliente_ExcluirCliente).Result;

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

        public void ShowMessage(string TipoMensagem, string titulo, Enuns.MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "swal('" + TipoMensagem + "','" + titulo + "','" + type + "')", true);
        }


    }
}