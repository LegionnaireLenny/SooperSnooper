using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SooperSnooper.Models.Validation
{
    public class SnoopModel
    {
        [Required]
        [StringLength(15, MinimumLength = 1, ErrorMessage =("Names must be 1 to 15 characters long."))]
        [RegularExpression(@"^[a-zA-Z_0-9]+$", ErrorMessage = "Only alphanumeric characters and underscores are allowed.")]
        public string Username { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Value must be a non-negative integer.")]
        public int Loops { get; set; }
    }
}