using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _01_MiPrimeraApp.Client.Services
{
    public class Autenticacion : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //var identity = new ClaimsIdentity(new[]
            //{
            //    new Claim(ClaimTypes.Name, "Maria Lopez")
            //}, "auth");

            //var user = new ClaimsPrincipal(identity);
            //return Task.FromResult(new AuthenticationState(user));

            // simular usuario no logueado
            // equivale a un cerrar sesión
            // cuando se inicia, debe ser no autenticado
            // var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();

            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        public void Entrar(string IDUsuario)
        {
            var identity = new ClaimsIdentity(new[]
           {
                new Claim(ClaimTypes.Name, IDUsuario),
            }, "auth");

            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void CerrarSesion()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
