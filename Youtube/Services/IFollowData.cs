using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Youtube.Models;

namespace Youtube.Services
{
    public interface IFollowData
    {
        IEnumerable<User> GetFollowers(long followingId);
        IEnumerable<User> GetFollowings(long followerId);
        Boolean Exist(long follower, long following);
        Follow Add(Follow follow);
        void Follow(Follow follow);
        void DeleteByFollower(long followerId);
        void DeleteByFollowing(long followingId);
        void Delete(Follow follow);
    }
}
