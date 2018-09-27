using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Youtube.Models;

namespace Youtube.Services
{
    public interface IVideoData
    {
        IEnumerable<Video> GetTop5(string userType, long id);
        IEnumerable<Video> GetAllForAdmin();
        IEnumerable<Video> GetAllForUser(long id);
        IEnumerable<Video> GetAllNoUser();
        IEnumerable<Video> GetByOwnerId(long ownerId);
        IEnumerable<Video> GetLikedVideos(User user);
        IEnumerable<Video> Search(string searchText, string searchBy);
        Video GetById(long id);
        void Delete(Video video);
        Video Create(Video video);
        Video Update(Video video);

    }
}
