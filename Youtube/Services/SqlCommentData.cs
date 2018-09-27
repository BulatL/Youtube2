using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Youtube.Data;
using Youtube.Models;

namespace Youtube.Services
{
    public class SqlCommentData : ICommentData
    {
        YoutubeDbContext _context;

        public SqlCommentData(YoutubeDbContext context)
        {
            _context = context;
        }
        public Comment Create(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return comment;
        }

        public void Delete(Comment comment)
        {
            comment.Deleted = true;
            _context.Attach(comment).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IEnumerable<Comment> GetAllComments()
        {
            IEnumerable<Comment> comments = new List<Comment>();
            comments = _context.Comments.Where(c => c.Deleted == false && c.User.Deleted == false).ToList();
            return comments;
        }

        public Comment GetById(long id)
        {
            Comment comment = _context.Comments.Where(c => c.Deleted == false)
                .SingleOrDefault(c => c.Id == id);
            return comment;
        }

        public IEnumerable<Comment> GetCommentsByUser(long id)
        {
            IEnumerable<Comment> comments = new List<Comment>();
            comments = _context.Comments.Where(c => c.Deleted == false && c.User.Deleted == false && c.UserId == id).ToList();
            return comments;
        }

        public IEnumerable<Comment> GetCommentsByVideo(long id)
        {
            IEnumerable<Comment> comments = new List<Comment>();
            comments = _context.Comments.Where(c => c.Deleted == false && c.User.Deleted == false && c.VideoId == id).ToList();
            return comments;
        }

        public IEnumerable<Comment> Search(string searchText, string ascDesc)
        {
            IEnumerable<Comment> comments = new List<Comment>();
            if (ascDesc.Equals("asc", StringComparison.InvariantCultureIgnoreCase))
            {
                 comments = _context.Comments.Where(u => u.Deleted == false && u.User.Deleted == false && u.Description.Contains(searchText))
                        .OrderBy(u => u.Id).ToList();
            }
            else
            {
                comments = _context.Comments.Where(u => u.Deleted == false && u.User.Deleted == false && u.Description.Contains(searchText))
                        .OrderByDescending(u => u.Id).ToList();
            }
            return comments;
        }

        public Comment Update(Comment comment)
        {
            _context.Attach(comment).State = EntityState.Modified;
            _context.SaveChanges();
            return comment;
        }
    }
}
