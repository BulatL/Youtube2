using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Youtube.Data;
using Youtube.Models;

namespace Youtube.Services
{
    public class SqlUserData : IUserData
    {
        YoutubeDbContext _context;
        
        public SqlUserData(YoutubeDbContext context)
        {
            _context = context;
        }
        public User Add(User newUser)
        {
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;
        }

        public User GetById(long id)
        {
            User user = _context.Users.Where(u => u.Deleted == false)
                .SingleOrDefault(u => u.Id == id);
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            IEnumerable<User> users = new List<User>();
            users = _context.Users.Where(u => u.Deleted == false)
                .OrderBy(u => u.Id);
            return users;
        }

        public User Update(User user)
        {
            _context.Attach(user).State = EntityState.Modified;
            _context.SaveChanges();
            return user;
        }

        public void Delete(User user)
        {
            user.Deleted = true;
            _context.Attach(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IEnumerable<User> Sort(string sortBy, string ascDesc)
        {
            IEnumerable <User>  users = new List<User>();
            if (ascDesc.Equals("asc")) {
                switch (sortBy)
                {
                    case "Username":
                        users =_context.Users.Where(u => u.Deleted == false)
                               .OrderBy(u => u.Id).ToList();
                        break;
                    case "FirstName":
                        users = _context.Users.Where(u => u.Deleted == false)
                               .OrderBy(u => u.FirstName).ToList();
                        break;
                    case "LastName":
                        users = _context.Users.Where(u => u.Deleted == false)
                               .OrderBy(u => u.LastName).ToList();
                        break;
                    case "Email":
                        users = _context.Users.Where(u => u.Deleted == false)
                               .OrderBy(u => u.Email).ToList();
                        break;
                    case "Description":
                        users = _context.Users.Where(u => u.Deleted == false)
                               .OrderBy(u => u.Description).ToList();
                        break;
                    case "RegistrationDate":
                        users = _context.Users.Where(u => u.Deleted == false)
                               .OrderBy(u => u.RegistrationDate).ToList();
                        break;
                    case "Role":
                        users = _context.Users.Where(u => u.Deleted == false)
                               .OrderBy(u => u.Role).ToList();
                        break;
                    default:
                        users = _context.Users.Where(u => u.Deleted == false)
                               .OrderBy(u => u.Id).ToList();
                        break;
                }
            } else if (ascDesc.Equals("desc")) {
                switch (sortBy)
                {
                    case "Username":
                        users = _context.Users.Where(u => u.Deleted == false)
                               .OrderByDescending(u => u.Id).ToList();
                        break;
                    case "FirstName":
                        users = _context.Users.Where(u => u.Deleted == false)
                               .OrderByDescending(u => u.FirstName).ToList();
                        break;
                    case "LastName":
                        users = _context.Users.Where(u => u.Deleted == false)
                               .OrderByDescending(u => u.LastName).ToList();
                        break;
                    case "Email":
                        users = _context.Users.Where(u => u.Deleted == false)
                               .OrderByDescending(u => u.Email).ToList();
                        break;
                    case "Description":
                        users = _context.Users.Where(u => u.Deleted == false)
                               .OrderByDescending(u => u.Description).ToList();
                        break;
                    case "RegistrationDate":
                        users = _context.Users.Where(u => u.Deleted == false)
                               .OrderByDescending(u => u.RegistrationDate).ToList();
                        break;
                    case "Role":
                        users = _context.Users.Where(u => u.Deleted == false)
                               .OrderByDescending(u => u.Role).ToList();
                        break;
                    default:
                        users = _context.Users.Where(u => u.Deleted == false)
                           .OrderByDescending(u => u.Id).ToList();
                        break;
                }
            }
            return users;
        }

        public IEnumerable<User> Search(string searchText, string searchBy, string ascDesc)
        {
            IEnumerable<User> users = new List<User>();
            if (ascDesc.Equals("asc", StringComparison.InvariantCultureIgnoreCase))
            {
                if (searchBy != null)
                {
                    if (searchBy.Equals("Username", StringComparison.InvariantCultureIgnoreCase))
                    {
                        users = _context.Users.Where(u => u.Deleted == false)
                            .Where(u => u.Username.Contains(searchText))
                            .OrderBy(u => u.Id).ToList();
                    }
                    else if (searchBy.Equals("FirstName", StringComparison.InvariantCultureIgnoreCase))
                    {
                        users = _context.Users.Where(u => u.Deleted == false)
                                   .Where(u => u.FirstName.Contains(searchText))
                                .OrderBy(u => u.FirstName).ToList();
                    }
                    else if (searchBy.Equals("LastName", StringComparison.InvariantCultureIgnoreCase))
                    {
                        users = _context.Users.Where(u => u.Deleted == false)
                               .Where(u => u.LastName.Contains(searchText))
                            .OrderBy(u => u.LastName).ToList();
                    }
                    else if (searchBy.Equals("Email", StringComparison.InvariantCultureIgnoreCase))
                    {
                        users = _context.Users.Where(u => u.Deleted == false)
                                   .Where(u => u.Email.Contains(searchText))
                                .OrderBy(u => u.Email).ToList();
                    }
                    else if (searchBy.Equals("Description", StringComparison.InvariantCultureIgnoreCase))
                    {
                        users = _context.Users.Where(u => u.Deleted == false)
                               .Where(u => u.Description.Contains(searchText))
                            .OrderBy(u => u.Description).ToList();
                    }
                    else if (searchBy.Equals("RegistrationDate", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var dateCompare = DateTime.Parse(searchText);
                        users = _context.Users.Where(u => u.Deleted == false)
                                   .Where(u => u.RegistrationDate >= dateCompare)
                                .OrderBy(u => u.RegistrationDate).ToList();
                    }
                    else
                    {
                        users = _context.Users.Where(u => u.Deleted == false)
                               .Where(u => u.Username.Contains(searchText))
                            .OrderBy(u => u.Id).ToList();
                    }
                }
                else
                {
                    users = _context.Users.Where(u => u.Deleted == false)
                                .Where(u => (u.Username.Contains(searchText)) ||
                                (u.FirstName.Contains(searchText)) ||
                                (u.LastName.Contains(searchText)) ||
                                (u.Email.Contains(searchText)) ||
                                (u.Description.Contains(searchText)))
                                .OrderBy(u => u.Id).ToList();
                }
            }
            else
            {
                if (searchBy != null)
                {
                    if (searchBy.Equals("Username", StringComparison.InvariantCultureIgnoreCase))
                    {
                        users = _context.Users.Where(u => u.Deleted == false)
                            .Where(u => u.Username.Contains(searchText))
                            .OrderByDescending(u => u.Id).ToList();
                    }
                    else if (searchBy.Equals("FirstName", StringComparison.InvariantCultureIgnoreCase))
                    {
                        users = _context.Users.Where(u => u.Deleted == false)
                                   .Where(u => u.FirstName.Contains(searchText))
                                .OrderByDescending(u => u.FirstName).ToList();
                    }
                    else if (searchBy.Equals("LastName", StringComparison.InvariantCultureIgnoreCase))
                    {
                        users = _context.Users.Where(u => u.Deleted == false)
                               .Where(u => u.LastName.Contains(searchText))
                            .OrderByDescending(u => u.LastName).ToList();
                    }
                    else if (searchBy.Equals("Email", StringComparison.InvariantCultureIgnoreCase))
                    {
                        users = _context.Users.Where(u => u.Deleted == false)
                                   .Where(u => u.Email.Contains(searchText))
                                .OrderByDescending(u => u.Email).ToList();
                    }
                    else if (searchBy.Equals("Description", StringComparison.InvariantCultureIgnoreCase))
                    {
                        users = _context.Users.Where(u => u.Deleted == false)
                               .Where(u => u.Description.Contains(searchText))
                            .OrderByDescending(u => u.Description).ToList();
                    }
                    else if (searchBy.Equals("RegistrationDate", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var dateCompare = DateTime.Parse(searchText);
                        users = _context.Users.Where(u => u.Deleted == false)
                                   .Where(u => u.RegistrationDate >= dateCompare)
                                .OrderByDescending(u => u.RegistrationDate).ToList();
                    }
                    else
                    {
                        users = _context.Users.Where(u => u.Deleted == false)
                               .Where(u => u.Username.Contains(searchText))
                            .OrderByDescending(u => u.Id).ToList();
                    }
                }
                else
                {
                    users = _context.Users.Where(u => u.Deleted == false)
                                .Where(u => (u.Username.Contains(searchText)) ||
                                (u.FirstName.Contains(searchText)) ||
                                (u.LastName.Contains(searchText)) ||
                                (u.Email.Contains(searchText)) ||
                                (u.Description.Contains(searchText)))
                                .OrderByDescending(u => u.Id).ToList();
                }
            }
            return users;
        }

        public User Login(string username, string password)
        {
            User user = _context.Users.Where(u => u.Username == username && u.Password == password)
                .SingleOrDefault(u => u.Deleted == false);

            return user;
        }

        public bool ExistUsername(string username, long id)
        {
            User exist = _context.Users.Where(u => u.Username == username && u.Id != id)
                .SingleOrDefault(u => u.Deleted == false);

            if (exist == null) return false;
            else return true;
        }

    }
}
