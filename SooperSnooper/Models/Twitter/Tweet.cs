using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SooperSnooper.Models.Twitter
{
    public class Tweet
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Username { get; set; }

        public User User { get; set; }

        [Required]
        public string MessageBody { get; set; }

        //[Required]
        //public DateTime PostDate { get; set; }
    }
}