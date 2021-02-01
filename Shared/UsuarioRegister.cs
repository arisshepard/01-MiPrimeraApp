﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace _01_MiPrimeraApp.Shared
{
    public class UsuarioRegister
{
        [Required(ErrorMessage = "El correo es obligatorio")]
        [MaxLength(100, ErrorMessage = "La longitud máxima debe ser 100")]
        public string Email { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
        public string FechaFormato { get; set; }
        public int ID { get; set; }
        public string IDTipoUsuario { get; set; }

        public string Nombre { get; set; }
        [Required(ErrorMessage = "Debe ingresar el nombre")]
        [MaxLength(100, ErrorMessage = "La longitud máxima debe ser 100")]
        [MinLength(2, ErrorMessage = "La longitud mínima debe ser 2")]
        public string NombreSimple { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "La contraseña debe tener mínimo 8 caracteres")]
        public string PasswordUsuario { get; set; }

        [Required(ErrorMessage = "Debe ingresar el primer apellido")]
        [MaxLength(150, ErrorMessage = "La longitud máxima debe ser 150")]
        [MinLength(2, ErrorMessage = "La longitud mínima debe ser 2")]
        public string PrimerApellido { get; set; }

        [Required(ErrorMessage = "Debe ingresar el segundo apellido")]
        [MaxLength(150, ErrorMessage = "La longitud máxima debe ser 150")]
        [MinLength(2, ErrorMessage = "La longitud mínima debe ser 2")]
        public string SegundoApellido { get; set; }

        [MaxLength(50, ErrorMessage = "La longitud máxima debe ser 50")]
        public string Telefono { get; set; }
    }
}
