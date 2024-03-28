using TesteMazza.Models;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI;
using EllipticCurve.Utils;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using TesteMazza;

namespace TesteMazza
{
    public partial class SiteMaster : MasterPage
    {
        static HttpClient client = new HttpClient();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
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

       


        public void ShowMessage(string TipoMensagem, string titulo, Enuns.MessageType type)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "swal('" + TipoMensagem + "','" + titulo + "','" + type + "');", true);
        }

    }
}