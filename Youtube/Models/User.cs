using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Youtube.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required, MaxLength(20)]
        public String Username { get; set; }
        [Required, MaxLength(30)]
        public String Password { get; set; }
        [Required, MaxLength(25)]
        public String FirstName { get; set; }
        [Required, MaxLength(40)]
        public String LastName { get; set; }
        [Required, EmailAddress]
        public String Email { get; set; }
        public String Description { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; }
        public String ProfilePictureUrl { get; set; }
        [Required]
        public RoleType Role { get; set; }
        [Required]
        public Boolean Blocked { get; set; }
        [Required]
        public Boolean Deleted { get; set; }

        [NotMapped]
        public virtual IEnumerable<User> Followers { get; set; } = new List<User>();
        [NotMapped]
        public virtual IEnumerable<User> Following { get; set; } = new List<User>();
        [NotMapped]
        public virtual IEnumerable<Video> UserVideos { get; set; } = new List<Video>();
        [NotMapped]
        public virtual IEnumerable<Video> LikedVideos { get; set; } = new List<Video>();
        [NotMapped]
        public virtual IEnumerable<Comment> UserComments { get; set; } = new List<Comment>();
        [NotMapped]
        public virtual IEnumerable<Comment> LikedComments { get; set; } = new List<Comment>();
    }
}
