using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TesteMazza.Api.Data;
using TesteMazza.Api.Models;
using System.Net;
using System.Net.Http;

namespace TesteMazza.Api.Controllers
{
    [Route("v1/enderecos")]
    [ApiController]
    public class EnderecoController : ControllerBase
    {

        [HttpGet]
        [Route(template: "GetByEnderecoPorIdCliente/{IdCliente:long}")]
        public async Task<ActionResult<Endereco>> GetByEnderecoPorUsuario([FromServices] TesteDataContext context,
                                                                          [FromRoute] Int64 IdCliente)
        {
            var endereco = await context.Endereco
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.IdCliente == IdCliente);

            return endereco;
        }

        [HttpGet]
        [Route(template: "GetByEnderecosPorIdEndereco/{IdEndereco:long}")]
        public async Task<ActionResult<Endereco>> GetByEnderecosPorIdEndereco([FromServices] TesteDataContext context, Int64 IdEndereco)
        {
            var enderecos = await context.Endereco
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdEndereco == IdEndereco);
            return enderecos;
        }


        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Endereco>> Post(
            [FromServices] TesteDataContext context,
            [FromBody] Endereco model)
        {
            if (ModelState.IsValid)
            {
                context.Endereco.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route(template: "AlterarEndereco/{IdEndereco:long}")]
        public async Task<ActionResult<Endereco>> AlterarEndereco([FromServices] TesteDataContext context,
                                                      [FromRoute] Int64 IdEndereco,
                                                      [FromBody] Endereco model)
        {
            if (ModelState.IsValid)
            {
                var end = context.Endereco.FirstOrDefault(e => e.IdEndereco == IdEndereco);
                if (end == null)
                {
                    return BadRequest("Endereco inexistente!");
                }
                else
                {
                    end.CEP = model.CEP;
                    end.Logradouro = model.Logradouro;
                    end.Numero = model.Numero;
                    end.Complemento = model.Complemento; ;
                    end.Bairro = model.Bairro;
                    end.Cidade = model.Cidade;
                    end.UF = model.UF;
                    end.Pais = model.Pais;

                    await context.SaveChangesAsync();
                    return end;
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
