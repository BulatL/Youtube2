using System;
using System.Collections.Generic;
using System.Linq;
using Youtube.Data;
using Youtube.Models;

namespace Youtube.Services
{
    public class SqlLikeVideoData : ILikeVideoData
    {
        YoutubeDbContext _context;

        public SqlLikeVideoData(YoutubeDbContext context)
        {
            _context = context;
        }
        public LikeDislikeVideo Add(LikeDislikeVideo likeDislike)
        {
            _context.Add(likeDislike);
            _context.SaveChanges();
            return likeDislike;
        }

        public bool Check(bool likeDislike, long userId, long videoId)
        {
            LikeDislikeVideo exist =_context.LikeDislikeVideos.Where(ld => ld.Deleted == false &&
                                                               ld.Owner.Id == userId &&
                                                               ld.Video.Id == videoId)
                                                               .SingleOrDefault(ld => ld.LikeOrDislike == likeDislike);
            if (exist == null) return false;
            else return true;
        }

        public void Delete(long userId, long videoId)
        {
            (from ld in _context.LikeDislikeVideos
             where ld.Owner.Id == userId && ld.Video.Id == videoId
             select ld).ToList().ForEach(x => x.Deleted = true);

            _context.SaveChanges();
        }

        public void DeleteByUserId(long id)
        {
            (from ld in _context.LikeDislikeVideos
             where ld.Owner.Id == id
             select ld).ToList().ForEach(x => x.Deleted = true);

            _context.SaveChanges();
        }

        public IEnumerable<LikeDislikeVideo> GetAllByUserId(long id)
        {
            return _context.LikeDislikeVideos.Where(ld => ld.Deleted == false && ld.Owner.Id == id).ToList();
        }
    }
}
