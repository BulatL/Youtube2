using System;
using Youtube.Models;

namespace Youtube.ViewModels
{
    public class LikeCommentDTO
    {
        public long Id { get; set; }
        public String LikeOrDislike { get; set; }
        public String CreationDate { get; set; }
        public UserForVideoComment Owner { get; set; }
        public CommentForUserDTO Comment { get; set; }

        public static LikeCommentDTO ConvertCommentToDTO(LikeDislikeComment likeDislikeComment)
        {
            string likeDislike = "";
            if (likeDislikeComment.LikeOrDislike == true) likeDislike = "Like";
            else likeDislike = "false";
            LikeCommentDTO newDto = new LikeCommentDTO
            {
                Id = likeDislikeComment.Id,
                LikeOrDislike = likeDislike,
                CreationDate = likeDislikeComment.CreationDate.ToString("dd.mm.yyyy"),
                Owner = UserForVideoComment.ConvertUserForVideoComment(likeDislikeComment.Owner),
                Comment = CommentForUserDTO.SingleConvertCommentToDTO(likeDislikeComment.Comment)
            };
            return newDto;
        }
    }
}
