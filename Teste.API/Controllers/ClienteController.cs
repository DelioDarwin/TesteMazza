using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Microsoft.EntityFrameworkCore;
using TesteMazza.Api.Data;
using TesteMazza.Api.Models;

namespace TesteMazza.Api.Controllers
{
    [Route("v1/Clientes")]
    [ApiController]
     
    public class ClienteController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Cliente>>> Get([FromServices] TesteDataContext context)
        {
            var Clientes = await context.Cliente.ToListAsync();
            return Clientes;
        }

        [Route(template: "RetornaDadosCliente/{IdCliente:long}")]
        public async Task<ActionResult<Cliente>> RetornaDadosCliente(
                                                    [FromServices] TesteDataContext context,
                                                    [FromRoute] Int64 IdCliente)
        {
            var Cliente = await context.Cliente
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IdCliente == IdCliente);

            return Cliente;
        }

        [HttpGet]
        [Route(template: "GetClientePorGuid/{CodigoCliente:guid}")]
        public async Task<ActionResult<Cliente>> GetClientePorGuid([FromServices] TesteDataContext context,
                                                                   [FromRoute] System.Guid CodigoCliente)
        {
            var Cliente = await context.Cliente
             .AsNoTracking()
             .FirstOrDefaultAsync(x => x.CodigoCliente == CodigoCliente);

            return Cliente;
        }


        [HttpGet]
        [Route(template: "RetornaClientePorEmail/{email}")]
        public async Task<ActionResult<Cliente>> RetornaClientePorEmail(
                                                           [FromServices] TesteDataContext context,
                                                           [FromRoute] string email)
        {
            var Cliente = await context.Cliente
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);

            return Cliente;
        }

            [HttpGet]
            [Route(template: "VerificaClienteExistente/{email}/{cpf}")]
            public async Task<ActionResult<Cliente>> VerificaClienteExistente(
                                                                [FromServices] TesteDataContext context,
                                                                [FromRoute] string email)
            {
                var Cliente = await context.Cliente
                    .AsNoTracking()
                    .Where(x => x.Email == email)
                    .FirstOrDefaultAsync();

                return Cliente;
            }

            [HttpPost]
            [Route("")]
            public async Task<ActionResult<Cliente>> Post(
                                                        [FromServices] TesteDataContext context,
                                                        [FromBody] Cliente model)
            {
                if (ModelState.IsValid)
                {
                    context.Cliente.Add(model);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                    return model;
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }

            [HttpPut]
            [Route(template: "AlterarDados/{IdCliente:long}")]
            public async Task<ActionResult<Cliente>> AlterarDados([FromServices] TesteDataContext context,
                                                  [FromRoute] Int64 IdCliente,
                                                  [FromBody] Cliente model)
            {
                if (ModelState.IsValid)
                {
                    var usr = context.Cliente.FirstOrDefault(e => e.IdCliente == IdCliente);
                    if (usr == null)
                    {
                        return BadRequest("Usuário inexistente!");
                    }
                    else
                    {
                        usr.Nome = model.Nome;
                        usr.Email = model.Email;

                        await context.SaveChangesAsync();
                        return usr;
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }

            [Route(template: "VerificaClienteValidaEmail/{email}/{codigo}")]
            public async Task<ActionResult<Cliente>> VerificaClienteValidaEmail(
                                                                [FromServices] TesteDataContext context,
                                                                [FromRoute] string email,
                                                                [FromRoute] System.Guid codigo)
            {
                var Cliente = await context.Cliente
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Email == email || e.CodigoCliente == codigo);

                return Cliente;
            }


            [HttpPut]
            [Route(template: "AtualizarFoto/{IdCliente:long}")]
            public async Task<ActionResult<Cliente>> AtualizarFoto([FromServices] TesteDataContext context,
                                        [FromRoute] Int64 IdCliente,
                                        [FromBody] Cliente model)
            {
                if (ModelState.IsValid)
                {
                    var Cliente = context.Cliente.FirstOrDefault(e => e.IdCliente == IdCliente);
                    if (Cliente == null)
                    {
                        return BadRequest("Anúncio inexistente!");
                    }
                    else
                    {
                        Cliente.Foto = model.Foto;

                        await context.SaveChangesAsync();
                        return Cliente;
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }


        [HttpDelete]
        [Route(template: "ExcluirCliente/{IdCliente:long}")]
        public async Task<ActionResult<Cliente>> ExcluirCliente(
                                                                   [FromServices] TesteDataContext context,
                                                                   [FromRoute] Int64 IdCliente)
        {
            if (ModelState.IsValid)
            {
                //Exclir corpp da Historia
                var endereco = context.Endereco.Where(c => c.IdCliente == IdCliente).AsEnumerable();

                foreach (var x in endereco)
                {
                    var m = x;
                    context.Endereco.Remove(m);
                }

                await context.SaveChangesAsync();



                //Excluir Cabeçalho da História
                var cliente = context.Cliente.Where(c => c.IdCliente == IdCliente).AsEnumerable();

                foreach (var x in cliente)
                {
                    var m = x;
                    context.Cliente.Remove(m);
                }

                await context.SaveChangesAsync();


                var clienteRet = context.Cliente
                    .AsNoTracking()
                    .Where(c => c.IdCliente == IdCliente);

                return Ok(clienteRet);

            }
            else
            {
                return BadRequest(ModelState);
            }
        }


    }

}

