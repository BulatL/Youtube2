using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Youtube.Models
{
    public class LikeDislikeComment
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
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
        public long CommentId { get; set; }
        public Boolean Deleted { get; set; }
    }
}
