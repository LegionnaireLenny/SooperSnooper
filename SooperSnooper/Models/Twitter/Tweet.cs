using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SooperSnooper.Models.Twitter
{
    public class Tweet
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [ForeignKey("User")]
        [StringLength(15)]
        [RegularExpression(@"^[a-zA-Z_0-9]+$", ErrorMessage = "Only alphanumeric characters and underscore are allowed.")]
        public string Username { get; set; }

        [Required]
        public string MessageBody { get; set; }

        [Required]
        public DateTime PostDate { get; set; }
    }
}