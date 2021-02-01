using _01_MiPrimeraApp.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _01_MiPrimeraApp.Server.Controllers
{
    [ApiController]
    public class ReservaController : Controller
    {
        private readonly BDBibliotecaContext _context;

        public ReservaController(BDBibliotecaContext context)
        {
            _context = context;
        }

        [HttpPut]
        [Route("api/Reserva/Delete/{ID}")]
        public async Task<int> DeleteById(int ID)
        {
            int respuesta = 0;

            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                Reserva reservaDb = await _context.Reserva.Where(reserva => reserva.Iidreserva == ID).FirstOrDefaultAsync();

                if (reservaDb != null)
                {
                    reservaDb.Bhabilitado = 0;
                    await _context.SaveChangesAsync();

                    Libro libroDb = await _context.Libro
                  .Where(libro => libro.Iidlibro == reservaDb.Iidlibro)
                  .FirstOrDefaultAsync();

                    if (libroDb != null)
                    {
                        libroDb.Stock =+ (int)reservaDb.Numlibros;
                        _context.Libro.Update(libroDb);
                        await _context.SaveChangesAsync();
                    }
                }
                await transaction.CommitAsync();
                respuesta = 1;
            }
            catch (Exception)
            {
                throw;
            }

            return respuesta;
        }

        [HttpGet]
        [Route("api/Reserva/GetByEstado/{idEstado?}")]
        public async Task<List<Shared.Reserva>> GetByEstadoAsync(int? idEstado)
        {
            List<Shared.Reserva> reservas = new List<Shared.Reserva>();

            reservas = await (from reservaDb in _context.Reserva
                              join libro in _context.Libro
                              on reservaDb.Iidlibro equals libro.Iidlibro
                              join usuario in _context.Usuario
                              on reservaDb.Iidusuario equals usuario.Iidusuario
                              join persona in _context.Persona
                              on usuario.Iidpersona equals persona.Iidpersona
                              where reservaDb.Bhabilitado == 1 &&
                              (idEstado == null || reservaDb.Iidestadoreserva == idEstado)
                              select new Shared.Reserva()
                              {
                                  Cantidad = reservaDb.Numlibros,
                                  FechaInicioCadena = ((DateTime)reservaDb.Dfechareserva).ToShortDateString(),
                                  FechaFinCadena = ((DateTime)reservaDb.Dfechafinreserva).ToShortDateString(),
                                  IDLibro = reservaDb.Iidlibro,
                                  ID = reservaDb.Iidreserva,
                                  IDUsuario = reservaDb.Iidusuario.ToString(),
                                  NombreLibro = libro.Titulo,
                                  NombreUsuario = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}"
                              })
                .ToListAsync();

            return reservas;
        }

        [HttpGet]
        [Route("api/Reserva/GetByUsuario/{idUsuario?}")]
        public async Task<List<Shared.Reserva>> GetByUsuarioAsync(int? idUsuario)
        {
            List<Shared.Reserva> reservas = new List<Shared.Reserva>();

            reservas = await (from reservaDb in _context.Reserva
                              join libro in _context.Libro
                              on reservaDb.Iidlibro equals libro.Iidlibro
                              join usuario in _context.Usuario
                              on reservaDb.Iidusuario equals usuario.Iidusuario
                              join persona in _context.Persona
                              on usuario.Iidpersona equals persona.Iidpersona
                              where reservaDb.Bhabilitado == 1 &&
                              reservaDb.Iidestadoreserva == 2 && // Pendiente
                              (idUsuario == null || reservaDb.Iidusuario == idUsuario)
                              select new Shared.Reserva()
                              {
                                  Cantidad = reservaDb.Numlibros,
                                  FechaInicioCadena = ((DateTime)reservaDb.Dfechareserva).ToShortDateString(),
                                  FechaFinCadena = ((DateTime)reservaDb.Dfechafinreserva).ToShortDateString(),
                                  IDLibro = reservaDb.Iidlibro,
                                  ID = reservaDb.Iidreserva,
                                  NombreLibro = libro.Titulo,
                                  NombreUsuario = $"{persona.Nombre} {persona.Appaterno} {persona.Apmaterno}"
                              })
                .ToListAsync();

            return reservas;
        }

        [HttpPost]
        [Route("api/Reserva/Nueva")]
        public async Task<int> NuevaAsync([FromBody] Shared.Reserva reserva)
        {
            int respuesta;
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                Reserva reservaDb = new Reserva()
                {
                    Bhabilitado = 1,
                    Dfechafinreserva = reserva.FechaFin,
                    Dfechareserva = reserva.FechaInicio,
                    Iidusuario = int.Parse(reserva.IDUsuario),
                    Numlibros = reserva.Cantidad,
                    Iidestadoreserva = 1,
                    Iidlibro = reserva.IDLibro
                };

                _context.Reserva.Add(reservaDb);
                await _context.SaveChangesAsync();

                Libro libroDb = await _context.Libro
                    .Where(libro => libro.Iidlibro == reserva.IDLibro)
                    .FirstOrDefaultAsync();

                if (libroDb != null)
                {
                    libroDb.Stock -= (int)reserva.Cantidad;
                    _context.Libro.Update(libroDb);
                    await _context.SaveChangesAsync();
                }

                ReservaHistorial reservaHistorial = new ReservaHistorial()
                {
                    Bhabilitado = 1,
                    Iidestado = 1,
                    Iidreserva = reservaDb.Iidreserva,
                    Iidusuario = reservaDb.Iidusuario,
                    Vobservacion = "Reserva registrada"
                };

                _context.ReservaHistorial.Add(reservaHistorial);
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

        [HttpPut]
        [Route("api/Reserva/Enviar/{idUsuario}/{idReserva}")]
        public async Task<int> EnviarAsync(int idUsuario, int idReserva)
        {
            int respuesta;
            int idEstadoPendiente = 2;
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                Reserva reservaDb = await _context.Reserva
                    .Where(reservaDb => reservaDb.Iidreserva == idReserva)
                    .FirstOrDefaultAsync();

                if (reservaDb != null)
                {
                    reservaDb.Iidestadoreserva = idEstadoPendiente; // Pendiente
                    _context.Reserva.Update(reservaDb);
                    await _context.SaveChangesAsync();
                }

                ReservaHistorial reservaHistorial = new ReservaHistorial()
                {
                    Bhabilitado = 1,
                    Iidestado = idEstadoPendiente,
                    Iidreserva = reservaDb.Iidreserva,
                    Iidusuario = idUsuario,
                    Vobservacion = "Reserva enviada"
                };

                _context.ReservaHistorial.Add(reservaHistorial);
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

        [HttpPut]
        [Route("api/Reserva/Recibir/{idReserva}/{idOperacion}")]
        public async Task<int> RecibirAsync(int idReserva, int idOperacion)
        {
            int respuesta;
            int idEstadoAceptada = 5;
            int idEstadoRechazada = 4;
            int idEstado;
            try
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                Reserva reservaDb = await _context.Reserva
                    .Where(reservaDb => reservaDb.Iidreserva == idReserva)
                    .FirstOrDefaultAsync();

                if (reservaDb != null)
                {
                    idEstado = idOperacion == 1 ? idEstadoAceptada : idEstadoRechazada;

                    reservaDb.Iidestadoreserva = idEstado; // Pendiente
                    _context.Reserva.Update(reservaDb);
                    await _context.SaveChangesAsync();

                    ReservaHistorial reservaHistorial = new ReservaHistorial()
                    {
                        Bhabilitado = 1,
                        Iidestado = idEstado,
                        Iidreserva = reservaDb.Iidreserva,
                        Iidusuario = reservaDb.Iidusuario,
                        Vobservacion = idOperacion == 1 ? "Reserva Aceptada" : "Reserva Rechazada"
                    };

                    _context.ReservaHistorial.Add(reservaHistorial);
                    await _context.SaveChangesAsync();
                }              

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