using _01_MiPrimeraApp.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _01_MiPrimeraApp.Server.Controllers
{
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly BDBibliotecaContext _context;

        public UsuarioController(BDBibliotecaContext context)
        {
            _context = context;
        }


        #region "Métodos GET"
        [HttpGet]
        [Route("api/Usuarios/GetPaginas/{ID}")]
        public async Task<List<Shared.Pagina>> GetPaginasPorRolAsync(int ID)
        {
            List<Shared.Pagina> paginas = new List<Shared.Pagina>();

            int iDTipoUsuario = await _context.Usuario
                .Where(usuarioDb => usuarioDb.Iidusuario == ID)
                .Select(usuarioDb => (int)usuarioDb.Iidtipousuario)
                .FirstAsync();

            paginas = await (from paginatipousu in _context.PaginaTipoUsuario
                             join pagina in _context.Pagina 
                             on paginatipousu.Iidpagina equals pagina.Iidpagina
                             where paginatipousu.Iidtipousuario == iDTipoUsuario &&
                             pagina.Bhabilitado == 1 &&
                             paginatipousu.Bhabilitado == 1 &&
                             pagina.Bvisible == 1
                             select new Shared.Pagina()
                             {
                                 Accion = $"{pagina.Accion}/{pagina.Iidpagina}",
                                 ID = pagina.Iidpagina,
                                 Mensaje = pagina.Mensaje
                             }
                             ).ToListAsync();

            return paginas;
        }


        [HttpGet]
        [Route("api/Usuario/GetAll")]
        public async Task<List<Shared.Usuario>> GetAllAsync()
        {
            return await(from usuario in _context.Usuario
                    join persona in _context.Persona
                    on usuario.Iidpersona equals persona.Iidpersona
                    where usuario.Bhabilitado == 1 &&
                    persona.Bhabilitado == 1
                    select new Shared.Usuario()
                    { 
                        ID = usuario.Iidusuario,
                        Nombre = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}"
                    })
                    .ToListAsync();
        }
        #endregion

        #region "Métodos POST"
        [HttpPost]
        [Route("api/Usuarios/Login")]
        public async Task<int> Login([FromBody] Shared.Usuario usuario)
        {
            int respuesta = 0;

            try
            {
                // Encriptado
                string clave = usuario.Password;
                SHA256Managed sha = new SHA256Managed();
                byte[] buffer = Encoding.Default.GetBytes(clave);
                byte[] passwordCifrada = sha.ComputeHash(buffer);

                var usuarioDb = await _context.Usuario
                    .Where(usuarioDb => usuarioDb.Nombreusuario == usuario.Nombre &&
                    usuarioDb.Contra == BitConverter.ToString(passwordCifrada).Replace("-", "")
                    && usuarioDb.Token == null)
                    .FirstOrDefaultAsync();

                if (usuarioDb != null)
                {
                    respuesta = usuarioDb.Iidusuario;
                }
                else
                {
                    // puede ser que el token no esté activado o que directamente no
                    // exista el usuario
                    bool tokenNoActivado = await _context.Usuario
                    .Where(usuarioDb => usuarioDb.Nombreusuario == usuario.Nombre &&
                    usuarioDb.Contra == BitConverter.ToString(passwordCifrada).Replace("-", "")
                    && usuarioDb.Token != null)
                    .AnyAsync();

                    if (tokenNoActivado)
                    {
                        respuesta = -1;
                    }
                }
            }
            catch (System.Exception)
            {

                respuesta = 0;
            }

            return respuesta;
        }

        [HttpGet]
        [Route("api/Usuarios/Token/{token}")]
        public async Task<int> Token(string token)
        {
            int respuesta = 0;

            try
            {
                Usuario usuarioDb = await _context.Usuario
                    .Where(usuario => usuario.Token == token)
                    .FirstOrDefaultAsync();

                if (usuarioDb != null)
                {
                    usuarioDb.Token = null;
                    _context.Usuario.Update(usuarioDb);
                    await _context.SaveChangesAsync();
                    respuesta = 1;
                }
            }
            catch (System.Exception)
            {

                respuesta = 0;
            }

            return respuesta;
        }

        #endregion

        #region "Métodos PUT"
        #endregion
    }
}