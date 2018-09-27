using System;
using System.Collections.Generic;
using Youtube.Models;

namespace Youtube.Services
{
    public interface ILikeVideoData
    {
        IEnumerable<LikeDislikeVideo> GetAllByUserId(long id);
        Boolean Check(Boolean likeDislike, long userId, long videoId);
        LikeDislikeVideo Add(LikeDislikeVideo likeDislike);
        void DeleteByUserId(long id);
        void Delete(long userId, long videoId);

    }
}
