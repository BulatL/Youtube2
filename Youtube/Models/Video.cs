using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Youtube.Models
{
    public class Video
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public String VideoUrl { get; set; }
        [Required]
        public String PhotoUrl { get; set; }
        [Required, MaxLength(80)]
        public String Name { get; set; }
        [Required, MaxLength(250)]
        public String Description { get; set; }
        [Required]
        public Visibility Visibility { get; set; }
        [Required]
        public Boolean AllowComments { get; set; }
        [Required]
        public Boolean AllowRaiting { get; set; }
        [Required]
        public long NumberOfLikes { get; set; }
        [Required]
        public long NumberOfDislikes { get; set; }
        [Required]
        public int NumberOfViews { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public Boolean Blocked { get; set; }
        [Required]
        public Boolean Deleted { get; set; }
        [ForeignKey("OwnerId")]
        [NotMapped]
        public User Owner { get; set; }
        public long OwnerId { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
    }
}
