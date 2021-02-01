using _01_MiPrimeraApp.Server.Models;
using _01_MiPrimeraApp.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _01_MiPrimeraApp.Server.Controllers
{
    [ApiController]
    public class PaginaTipoUsuarioController : Controller
    {
        private readonly BDBibliotecaContext _context;
        private readonly IEntityMappingService<PaginaTipoUsuario, Shared.PaginaTipoUsuario> _mappingService;

        public PaginaTipoUsuarioController(BDBibliotecaContext context, IEntityMappingService<PaginaTipoUsuario, Shared.PaginaTipoUsuario> mappingService)
        {
            _context = context;
            _mappingService = mappingService;
        }

        [HttpGet]
        [Route("api/PaginasTipoUsuario/Filter/{IDtipoUsuario?}")]
        public async Task<List<Shared.PaginaTipoUsuario>> FilterAsync(string IDtipoUsuario)
        {
            List<Shared.PaginaTipoUsuario> paginasTipoUsuario = new List<Shared.PaginaTipoUsuario>();
            paginasTipoUsuario = await (from paginaUsuario in _context.PaginaTipoUsuario
                                        join pagina in _context.Pagina
                                        on paginaUsuario.Iidpagina equals pagina.Iidpagina
                                        join tipoUsuario in _context.TipoUsuario
                                        on paginaUsuario.Iidtipousuario equals tipoUsuario.Iidtipousuario
                                        where paginaUsuario.Bhabilitado == 1 &&
                                        (string.IsNullOrEmpty(IDtipoUsuario) || tipoUsuario.Iidtipousuario == int.Parse(IDtipoUsuario))
                                        select new Shared.PaginaTipoUsuario()
                                        {
                                            ID = paginaUsuario.Iidpaginatipousuario,
                                            Nombre = pagina.Mensaje,
                                            NombreTipoUsuario = tipoUsuario.Nombre
                                        })
                                          .ToListAsync();

            return paginasTipoUsuario;
        }

        [HttpGet]
        [Route("api/PaginasTipoUsuario/GetAll")]
        public async Task<List<Shared.PaginaTipoUsuario>> GetAllAsync()
        {
            List<Shared.PaginaTipoUsuario> paginasTipoUsuario = new List<Shared.PaginaTipoUsuario>();
            paginasTipoUsuario = await (
                                        from paginaUsuario in _context.PaginaTipoUsuario
                                        join pagina in _context.Pagina
                                        on paginaUsuario.Iidpagina equals pagina.Iidpagina
                                        join tipoUsuario in _context.TipoUsuario
                                        on paginaUsuario.Iidtipousuario equals tipoUsuario.Iidtipousuario
                                        where paginaUsuario.Bhabilitado == 1
                                        select new Shared.PaginaTipoUsuario()
                                        {
                                            ID = paginaUsuario.Iidpaginatipousuario,
                                            Nombre = pagina.Mensaje,
                                            NombreTipoUsuario = tipoUsuario.Nombre
                                        })
                                          .ToListAsync();

            return paginasTipoUsuario;
        }

        [HttpGet]
        [Route("api/PaginasTipoUsuario/GetButtonsAll")]
        public async Task<List<Shared.Boton>> GetButtonsAllAsync()
        {
            List<Shared.Boton> botones = new List<Shared.Boton>();
            botones = await _context.Button
                .Where(button => button.Bhabilitado == 1)
                .Select(buttonDb => new Shared.Boton()
                {
                    ID = buttonDb.Iidbutton,
                    Nombre = buttonDb.Nombrebutton
                })
                .ToListAsync();

            return botones;
        }

        [HttpGet]
        [Route("api/PaginasTipoUsuario/GetById/{ID}")]
        public async Task<Shared.PaginaTipoUsuario> GetByIDAsync(string ID)
        {
            //PaginaTipoUsuario paginaTipoUsuarioDb = await _context.PaginaTipoUsuario
            //    .Where(paginaTipoUsuarioDb => paginaTipoUsuarioDb.Iidpaginatipousuario.ToString() == ID)
            //    .FirstOrDefaultAsync();

            PaginaTipoUsuario paginaTipoUsuarioDb = await _context.PaginaTipoUsuario
                .Where(paginaTipoUsuarioDb => paginaTipoUsuarioDb.Iidpaginatipousuario.ToString() == ID)
                .Include(paginaTipoUsuarioDb => paginaTipoUsuarioDb.IidpaginaNavigation)
                .Include(paginaTipoUsuarioDb => paginaTipoUsuarioDb.IidtipousuarioNavigation)
                .FirstOrDefaultAsync();

            List<PaginaTipoUsuButton> botones = await _context.PaginaTipoUsuButton
                .Where(boton => boton.Iidpaginatipousuario == paginaTipoUsuarioDb.Iidpaginatipousuario &&
                boton.Bhabilitado == 1).ToListAsync();

            paginaTipoUsuarioDb.PaginaTipoUsuButton = botones;

            return _mappingService.Map(paginaTipoUsuarioDb);
        }

        [HttpGet]
        [Route("api/PaginasTipoUsuario/GetButtons/{IDUsuario}/{IDPagina}")]
        public async Task<List<int>> GetButtonsAsync(int IDUsuario, int IDPagina)
        {
            List<int> botones = new List<int>();

            int IDTipoUsuario = await _context.Usuario
                .Where(usuario => usuario.Iidusuario == IDUsuario)
                .Select(usuario => (int)usuario.Iidtipousuario)
                .FirstOrDefaultAsync();

            int IDPaginaTipoUSuario = await _context.PaginaTipoUsuario
                .Where(pagina => pagina.Iidtipousuario == IDTipoUsuario &&
                pagina.Iidpagina == IDPagina)
                .Select(pagina => pagina.Iidpaginatipousuario)
                .FirstOrDefaultAsync();

            var listabotones = await _context.PaginaTipoUsuButton
                .Where(pagina => pagina.Iidpaginatipousuario == IDPaginaTipoUSuario &&
                pagina.Bhabilitado == 1).ToListAsync();

            botones = listabotones.Select(boton => (int)boton.Iidbutton).ToList();

            return botones;
        }

        [HttpPut]
        [Route("api/PaginasTipoUsuario/UpdateById/{ID}")]
        public async Task<int> UpdateByIdASync(int ID, [FromBody] Shared.PaginaTipoUsuario paginaTipoUsuario)
        {
            int respuesta = 0;

            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                PaginaTipoUsuario paginaTipoUsuarioDb = await _context.PaginaTipoUsuario
                   .Where(paginaTipoUsuarioDb => paginaTipoUsuarioDb.Iidpaginatipousuario == ID)
                   .Include(paginaTipoUsuarioDb => paginaTipoUsuarioDb.PaginaTipoUsuButton)
                   //.Include(paginaTipoUsuarioDb => paginaTipoUsuarioDb.IidpaginaNavigation)
                   //.Include(paginaTipoUsuarioDb => paginaTipoUsuarioDb.IidtipousuarioNavigation)
                   .FirstOrDefaultAsync();

                // Paginas que antes estaban
                paginaTipoUsuarioDb.PaginaTipoUsuButton
                    .Where(boton => boton.Iidpaginatipousuario == paginaTipoUsuarioDb.Iidpaginatipousuario)
                    .Select(boton =>
                    {
                        boton.Bhabilitado = paginaTipoUsuario.Buttons.Contains((int)boton.Iidbutton) ? 1 : 0;
                        return boton;
                    }).ToList();

                // botones nuevos
                foreach (var numero in paginaTipoUsuario.Buttons)
                {
                    if (!_context.PaginaTipoUsuButton.Any(boton => boton.Iidpaginatipousuario == ID && boton.Iidbutton == numero))
                    {
                        _context.PaginaTipoUsuButton.Add(new PaginaTipoUsuButton()
                        {
                            Iidbutton = numero,
                            Iidpaginatipousuario = paginaTipoUsuarioDb.Iidpaginatipousuario,
                            Bhabilitado = 1
                        });
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                respuesta = 1;
            }
            catch (Exception)
            {
                throw;
            }

            return respuesta;
        }
    }
}