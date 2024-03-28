using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteMazza.Models
{
    public class Endereco
    {       

        [Key]
        public Int64 IdEndereco { get; set; }


        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int Numero { get; set; }

        public string Complemento { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string UF { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Pais { get; set; }

        public Int64 IdCliente { get; set; }
        //public Usuario Usuario { get; set; }

    }
}
