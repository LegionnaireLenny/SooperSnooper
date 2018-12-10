using System;
using System.ComponentModel.DataAnnotations;

namespace SooperSnooper.Models.Twitter
{
    public class User
    {
        [Key]
        [StringLength(15, MinimumLength = 1, ErrorMessage = ("Names must be 1 to 15 characters long."))]
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