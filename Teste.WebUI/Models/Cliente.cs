using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteMazza.Models
{
    public class Cliente
    {
        [Key]
        public Int64 IdCliente { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public DateTime DataCadastro { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public System.Guid CodigoCliente { get; set; }

        public string Foto { get; set; }

        [ForeignKey("IdUsuario")]
        public IEnumerable<Endereco> Enderecos { get; set; }

     
    }
}
