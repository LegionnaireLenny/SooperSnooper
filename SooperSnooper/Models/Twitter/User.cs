using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SooperSnooper.Models.Twitter
{
    public class User
    {
        [Key]
        [StringLength(15)]
        [RegularExpression(@"^[a-zA-Z_0-9]+$", ErrorMessage = "Only alphanumeric characters and underscores are allowed.")]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }

        public string Location { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

    }
}