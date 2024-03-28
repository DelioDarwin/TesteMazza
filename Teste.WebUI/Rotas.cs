using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security.Policy;
using System.Web;

namespace TesteMazza
{
    public class Rotas
    {

        //URL Debug
        public const string URLApi = "http://localhost:5000/";

        //### Endpoints Cliente
        public string sEndPoint_Cliente { get; set; }
        public string sEndPoint_Cliente_RetornaLogin { get; set; } 
        public string sEndPoint_Cliente_VerificaClienteExistente { get; set; }
        public string sEndPoint_Cliente_RetornaDadosCliente { get; set; }
        public string sEndPoint_Cliente_AlterarDados { get; set; }
        public string sEndPoint_Cliente_AtualizarFoto { get; set; }
        public string sEndPoint_Cliente_VerificaClienteValidaEmail { get; set; }
        public string sEndPoint_Cliente_ValidaEmail { get; set; }
        public string sEndPoint_Cliente_RetornaClientePorEmail { get; set; }
        public string sEndPoint_Cliente_ExcluirCliente { get; set; }
        




        //### Endpoints Endereco
        public string sEndPoint_Endereco { get; set; }
        public string sEndPoint_Endereco_GetByEnderecosPorIdEndereco { get; set; }
        public string sEndPoint_Endereco_AlterarEndereco { get; set; }
        public string sEndPoint_Endereco_ExcluirEndereco { get; set; }

        


        public Rotas()
        {
            //### Endpoints Cliente
            sEndPoint_Cliente = URLApi + "v1/Clientes";
            sEndPoint_Cliente_RetornaDadosCliente = URLApi + "v1/Clientes/RetornaDadosCliente/";
            sEndPoint_Cliente_AlterarDados = URLApi + "v1/Clientes/AlterarDados/";
            sEndPoint_Cliente_AtualizarFoto = URLApi + "v1/Clientes/AtualizarFoto/";
            sEndPoint_Cliente_RetornaClientePorEmail = URLApi + "v1/Clientes/RetornaClientePorEmail/";
            sEndPoint_Cliente_ExcluirCliente = URLApi + "v1/Clientes/ExcluirCliente/";


            //### Endpoints Endereco
            sEndPoint_Endereco = URLApi + "v1/enderecos";
            sEndPoint_Endereco_GetByEnderecosPorIdEndereco = URLApi + "v1/enderecos/GetByEnderecosPorIdEndereco/";
            sEndPoint_Endereco_AlterarEndereco = URLApi + "v1/enderecos/AlterarEndereco/";
            sEndPoint_Endereco_ExcluirEndereco = URLApi + "v1/enderecos/ExcluirEndereco/";

            


        }

}
}