using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Youtube.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required, MaxLength(250)]
        public String Description { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public long NumberOfLikes { get; set; }
        [Required]
        public long NumberOfDislikes { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public User User { get; set; }
        public long UserId { get; set; }
        [Required]
        [ForeignKey("VideoId")]
        public Video Video { get; set; }
        public long VideoId { get; set; }
        [Required]
        public Boolean Blocked { get; set; }
        [Required]
        public Boolean Deleted { get; set; }
    }
}
