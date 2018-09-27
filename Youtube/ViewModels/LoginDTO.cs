using System;
using System.ComponentModel.DataAnnotations;

namespace Youtube.ViewModels
{
    public class LoginDTO
    {
        [Required, MaxLength(20)]
        public String Username { get; set; }
        [Required, MaxLength(30)]
        public String Password { get; set; }
    }
}
