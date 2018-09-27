using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Youtube.Models
{
    public class LikeDislikeVideo
    {
        public long Id { get; set; }
        [Required]
        public Boolean LikeOrDislike { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        [ForeignKey("OwnerId")]
        public User Owner { get; set; }
        public long OwnerId { get; set; }
        [Required]
        [ForeignKey("VideoId")]
        public Video Video { get; set; }
        public long VideoId { get; set; }
        public Boolean Deleted { get; set; }
    }
}
