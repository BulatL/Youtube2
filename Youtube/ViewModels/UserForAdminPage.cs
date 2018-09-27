using System;
using System.Collections.Generic;
using Youtube.Models;

namespace Youtube.ViewModels
{
    public class UserForAdminPage
    {
        public long Id { get; set; }
        public String Username { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public Boolean Blocked { get; set; }
        public String profilePictureUrl { get; set; }
        public String Role { get; set; }

        public static List<UserForAdminPage> ConvertDTO(IEnumerable<User> users)
        {
            List<UserForAdminPage> usersDTO = new List<UserForAdminPage>();
            foreach (User user in users)
            {
                UserForAdminPage userForAdminPage = new UserForAdminPage();
                userForAdminPage.Id = user.Id;
                userForAdminPage.Username = user.Username;
                userForAdminPage.FirstName = user.FirstName;
                userForAdminPage.LastName = user.LastName;
                userForAdminPage.Email = user.Email;
                userForAdminPage.Blocked = user.Blocked;
                userForAdminPage.profilePictureUrl = user.ProfilePictureUrl;
                RoleType role = (RoleType)user.Role;
                userForAdminPage.Role = role.ToString();

                usersDTO.Add(userForAdminPage);
            }
            

            return usersDTO;
        }
    }
}
