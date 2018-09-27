using System;
using System.Collections.Generic;
using System.Linq;
using Youtube.Data;
using Youtube.Models;

namespace Youtube.Services
{
    public class SqlLikeCommentData : ILikeCommentData
    {
        YoutubeDbContext _context;

        public SqlLikeCommentData(YoutubeDbContext context)
        {
            _context = context;
        }

        public LikeDislikeComment Add(LikeDislikeComment likeDislike)
        {
            _context.Add(likeDislike);
            _context.SaveChanges();
            return likeDislike;
        }

        public bool Check(bool likeDislike, long userId, long commentId)
        {
            LikeDislikeComment exist = _context.LikeDislikeComments.Where(ld => ld.Deleted == false &&
                                                                    ld.Owner.Id == userId &&
                                                                    ld.Comment.Id == commentId).
                                                                    SingleOrDefault(ld => ld.LikeOrDislike == likeDislike);
            if (exist == null) return false;
            else return true;
        }

        public void Delete(long userId, long commentId)
        {
            (from ld in _context.LikeDislikeComments
             where ld.Owner.Id == userId && ld.Comment.Id == commentId
             select ld).ToList().ForEach(x => x.Deleted = true);

            _context.SaveChanges();
        }

        public void DeleteByUserId(long id)
        {
            (from ld in _context.LikeDislikeComments
             where ld.Owner.Id == id
             select ld).ToList().ForEach(x => x.Deleted = true);

            _context.SaveChanges();
        }

        IEnumerable<LikeDislikeComment> ILikeCommentData.GetAllByUserId(long id)
        {
            IEnumerable<LikeDislikeComment> likeDislikeComments = new List<LikeDislikeComment>();
                likeDislikeComments = _context.LikeDislikeComments
                                        .Where(ld => ld.Deleted == false &&
                                                     ld.Owner.Id == id);
            return likeDislikeComments;
        }
    }
}
