using System;
using System.ComponentModel.DataAnnotations;

namespace ParkyWeb.Models
{
    public class NationalPark
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string State { get; set; }
        public byte[] Picture { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime Established { get; set; }
    }
}
