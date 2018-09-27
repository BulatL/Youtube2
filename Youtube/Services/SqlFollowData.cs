using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Youtube.Data;
using Youtube.Models;

namespace Youtube.Services
{
    public class SqlFollowData : IFollowData
    {
        YoutubeDbContext _context;

        public SqlFollowData(YoutubeDbContext context)
        {
            _context = context;
        }

        public Follow Add(Follow follow)
        {
            _context.Follow.Add(follow);
            _context.SaveChanges();
            return follow;
        }

        public void Delete(Follow follow)
        {
            Follow deleteFollow = _context.Follow.Where(f => f.FollowerId == follow.Follower.Id)
                                 .SingleOrDefault(f => f.FollowingId == follow.Following.Id);
            if(deleteFollow != null)
            {
                _context.Remove(deleteFollow);
                _context.SaveChanges();
            }
        }

        public void DeleteByFollower(long followerId)
        {
            List<Follow> listForDelete = _context.Follow.Where(f => f.FollowerId == followerId).ToList();
            foreach (Follow f in listForDelete)
            {
                _context.Follow.Remove(f);
                //_context.Entry(f).State = EntityState.Deleted;
            }
            _context.SaveChanges();
        }

        public void DeleteByFollowing(long followingId)
        {
            List<Follow> listForDelete = _context.Follow.Where(f => f.FollowingId == followingId).ToList();
            foreach (Follow f in listForDelete)
            {
                _context.Follow.Remove(f);
            }
            _context.SaveChanges();
        }

        public bool Exist(long follower, long following)
        {
            Follow follow = _context.Follow.Where(f => f.FollowerId == follower)
                                 .SingleOrDefault(f => f.FollowingId == following);
            if (follow != null) return true;
            else return false;
        }

        public void Follow(Follow follow)
        {
            _context.Follow.Add(follow);
            _context.SaveChanges();
        }

        public IEnumerable<User> GetFollowers(long followingId)
        {
            IEnumerable<User> followers = new List<User>();
            followers = _context.Follow.Where(f => f.FollowingId == followingId && f.Following.Deleted ==false && f.Follower.Deleted == false)
                .Select(u => u.Follower).ToList();
            return followers;
        }

        public IEnumerable<User> GetFollowings(long followerId)
        {
            IEnumerable<User> following = new List<User>();
            following = _context.Follow.Where(f => f.FollowerId == followerId && f.Follower.Deleted == false && f.Following.Deleted == false)
                .Select(u => u.Following).ToList();
            return following;
        }
    }
}
