using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Youtube.Models
{
    public class Follow
    {
        [ForeignKey("FollowerId")]
        public User Follower { get; set; }
        public long FollowerId { get; set; }
        [ForeignKey("FollowingId")]
        public User Following { get; set; }
		public long FollowingId { get; set; }
    }
}