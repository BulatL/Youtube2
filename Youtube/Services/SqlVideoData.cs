using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Youtube.Data;
using Youtube.Models;

namespace Youtube.Services
{
    public class SqlVideoData : IVideoData
    {
        YoutubeDbContext _context;

        public SqlVideoData(YoutubeDbContext context)
        {
            _context = context;
        }
        public Video Create(Video video)
        {
            /*Video newVideo = new Video();
            newVideo.AllowComments = video.AllowComments;
            newVideo.AllowRaiting = video.AllowRaiting;
            newVideo.Blocked = false;
            newVideo.CreationDate = DateTime.Now.*/
            _context.Videos.Add(video);
            _context.SaveChanges();
            return video;

        }

        public void Delete(Video video)
        {
            video.Deleted = true;
            _context.Attach(video).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IEnumerable<Video> GetAllForAdmin()
        {
            IEnumerable<Video> videos = new List<Video>();
            videos = _context.Videos.Where(v => v.Deleted == false && v.Owner.Deleted == false)
                .OrderByDescending(v => v.NumberOfViews).ToList();
            /*var query = from video in _context.Videos
                        join user in _context.Users on video.Owner equals user
                        where video.Deleted == false && user.Deleted == false
                        select new { Video = video, User = user };*/
            return videos;
        }

        public IEnumerable<Video> GetAllForUser(long id)
        {
            IEnumerable<Video> videos = new List<Video>();
            videos = _context.Videos.Where(v => (v.Deleted == false && v.Owner.Deleted == false) &&
                                                (v.Owner.Id == id || v.Blocked == false) &&
                                                (v.Owner.Id == id || v.Owner.Blocked == false) &&
                                                (v.Visibility != Visibility.Unlisted))
                .OrderByDescending(v => v.NumberOfViews).ToList();
            return videos;
        }

        public IEnumerable<Video> GetAllNoUser()
        {
            IEnumerable<Video> videos = new List<Video>();
            videos = _context.Videos.Where(v => (v.Deleted == false && v.Owner.Deleted == false) && 
                                                 v.Owner.Blocked == false &&
                                                 v.Visibility == Visibility.Public &&
                                                 v.Blocked == false)
                .OrderByDescending(v => v.NumberOfViews).ToList();
            return videos;
        }

        public Video GetById(long id)
        {
            Video video = _context.Videos.Where(v => v.Deleted == false && v.Owner.Deleted == false)
                .OrderByDescending(v => v.NumberOfViews)
                .SingleOrDefault(v => v.Id == id);
            return video;
        }

        public IEnumerable<Video> GetByOwnerId(long ownerId)
        {
            IEnumerable<Video> videos = new List<Video>();
            videos = _context.Videos.Where(v => (v.Deleted == false && v.Owner.Deleted == false) &&
                                                (v.Visibility != Visibility.Unlisted) &&
                                                (v.Owner.Id == ownerId))
                .OrderByDescending(v => v.NumberOfViews).ToList();
            return videos;
        }

        public IEnumerable<Video> GetLikedVideos(User user)
        {
            IEnumerable<Video> likedVideos = new List<Video>();
            likedVideos = from videos in _context.Videos
                          join likeDislike in _context.LikeDislikeVideos
                          on videos.Id equals likeDislike.VideoId
                          where videos.Deleted == false && likeDislike.Deleted == false &&
                          likeDislike.LikeOrDislike == true && likeDislike.OwnerId == user.Id
                          select videos;

            return likedVideos;
        }

        public IEnumerable<Video> GetTop5(string userType, long id)
        {
            IEnumerable<Video> videos = new List<Video>();
            if (userType.Equals("Admin", StringComparison.InvariantCultureIgnoreCase))
            {
                videos = _context.Videos.Where(v => v.Deleted == false && v.Owner.Deleted == false)
                .OrderByDescending(v => v.NumberOfViews).Take(5).ToList();
            }
            else if (userType.Equals("User", StringComparison.InvariantCultureIgnoreCase))
            {
                IEnumerable<Video> videos2 = new List<Video>();
                videos = _context.Videos.Where(v => (v.Deleted == false && 
                                                    (v.Owner.Id == id || v.Blocked == false) &&
                                                    (v.Visibility == Visibility.Public || 
                                                    (v.Visibility == Visibility.Private  && v.OwnerId == id))))
                    .OrderByDescending(v => v.NumberOfViews).Take(5).ToList();
            }
            else
            {
                IEnumerable<Video> videos2 = new List<Video>();
                videos = _context.Videos.Where(v => (v.Deleted == false &&
                                                    (v.Owner.Id == id || v.Blocked == false) &&
                                                     v.Visibility == Visibility.Public))
                    .OrderByDescending(v => v.NumberOfViews).Take(5).ToList();
            }
            return videos;
        }

        public IEnumerable<Video> Search(string searchText, string searchBy)
        {
            IEnumerable<Video> videos = new List<Video>();
            if (searchBy != null)
            {
                if (searchBy.Equals("Name", StringComparison.InvariantCultureIgnoreCase))
                {
                    videos = _context.Videos.Where(v => v.Deleted == false && v.Owner.Deleted == false)
                        .Where(v => v.Name.Contains(searchText))
                        .OrderByDescending(v => v.Name).ToList();
                }
                else if (searchBy.Equals("Description", StringComparison.InvariantCultureIgnoreCase))
                {
                    videos = _context.Videos.Where(v => v.Deleted == false && v.Owner.Deleted == false)
                        .Where(v => v.Description.Contains(searchText))
                        .OrderByDescending(v => v.Description).ToList();
                }
                else if (searchBy.Equals("User", StringComparison.InvariantCultureIgnoreCase))
                {
                    videos = _context.Videos.Where(v => v.Deleted == false && v.Owner.Deleted == false)
                        .Where(v => v.Owner.Username.Contains(searchText))
                        .OrderByDescending(v => v.Owner.Id).ToList();
                }
                else if (searchBy.Equals("Date", StringComparison.InvariantCultureIgnoreCase))
                {
                    var dateCompare = DateTime.Parse(searchText);
                    videos = _context.Videos.Where(v => v.Deleted == false && v.Owner.Deleted == false)
                               .Where(v => v.CreationDate >= dateCompare)
                            .OrderByDescending(v => v.CreationDate).ToList();
                }
                else
                {
                    videos = _context.Videos.Where(v => v.Deleted == false && v.Owner.Deleted == false)
                        .Where(v => v.Name.Contains(searchText))
                        .OrderByDescending(v => v.Name).ToList();
                }
            }
            else
            {
                videos = _context.Videos.Where(v => v.Deleted == false && v.Owner.Deleted == false)
                            .Where(v => (v.Name.Contains(searchText)) ||
                            (v.Description.Contains(searchText)) ||
                            (v.Owner.Username.Contains(searchText)))
                            .OrderByDescending(v => v.Name).ToList();
            }
            return videos;
        }

        public Video Update(Video video)
        {
            _context.Attach(video).State = EntityState.Modified;
            _context.SaveChanges();
            return video;
        }
    }
}
