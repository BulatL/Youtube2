using System.Collections.Generic;
using Youtube.Models;

namespace Youtube.Services
{
    public interface IUserData
    {
        IEnumerable<User> GetAll();
        User GetById(long id);
        User Add(User newUser);
        User Update(User user);
        User Login(string username, string password);
        bool ExistUsername(string username, long id);
        void Delete(User user);
        IEnumerable<User> Sort(string sortBy, string ascDes);
        IEnumerable<User> Search(string searchText, string searchBy, string ascDesc);

    }
}
