using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Youtube.Models;

namespace Youtube.Services
{
    public interface ICommentData
    {
        IEnumerable<Comment> GetAllComments();
        IEnumerable<Comment> GetCommentsByUser(long id);
        IEnumerable<Comment> GetCommentsByVideo(long id);
        IEnumerable<Comment> Search(string searchText, string ascDesc);
        Comment GetById(long id);
        Comment Create(Comment comment);
        Comment Update(Comment comment);
        void Delete(Comment comment);

    }
}
