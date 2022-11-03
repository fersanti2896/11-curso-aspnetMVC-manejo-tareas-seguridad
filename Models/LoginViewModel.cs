﻿using System.ComponentModel.DataAnnotations;

namespace ManejoTareas.Models {
    public class LoginViewModel {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress(ErrorMessage = "El campo debe ser un correo electrónico válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Recuérdamen")]
        public bool Recordar { get; set; }
    }
}