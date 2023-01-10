using System.ComponentModel.DataAnnotations;
using System;
using Exemple.Domain.Models;

namespace Example.Api.Models
{
    public class InputCos
    {
        [Required]
        [RegularExpression(IdComanda.Pattern)]
        public string RegistrationNumber { get; set; }

        [Required]
        [Range(1, 10000)]
        public decimal Cant { get; set; }

        [Required]
        [Range(1, 10000)]
        public decimal Pretb { get; set; }
        [Required]
        [RegularExpression(AdresaPlata.Pattern)]
        public string Adresa { get; set; }
    }
}
