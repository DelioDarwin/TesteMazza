using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteMazza.Api.Models
{
    public class Cliente
    {
        [Key]
        public Int64 IdCliente { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Email { get; set; }

        public string Nome { get; set; }

        public DateTime DataCadastro { get; set; }

        public string Foto { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public System.Guid CodigoCliente { get; set; }

        [ForeignKey("IdCliente")]
        public IEnumerable<Endereco> Enderecos { get; set; }

     
    }
}
