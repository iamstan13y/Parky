﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ParkyAPI.Models.Trail;

namespace ParkyAPI.Models.Dtos
{
    public class TrailDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        public DifficultyType Difficulty { get; set; }
        [Required]
        public int NationalParkId { get; set; }
        public NationalParkDto NationalPark { get; set; }
    }
}
