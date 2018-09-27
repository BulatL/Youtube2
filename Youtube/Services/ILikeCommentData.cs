using System;
using System.Collections.Generic;
using Youtube.Models;

namespace Youtube.Services
{
    public interface ILikeCommentData
    {
        IEnumerable<LikeDislikeComment> GetAllByUserId(long id);
        Boolean Check(Boolean likeDislike, long userId, long commentId);
        LikeDislikeComment Add(LikeDislikeComment likeDislike);
        void DeleteByUserId(long id);
        void Delete(long userId, long commentId);
    }
}
