using Microsoft.AspNetCore.Identity;

namespace SocialMedia.Models{
    public class User : IdentityUser
    {
        [PersonalData]
        public int DepartamentID { get; set; }
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        /*public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }


        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<UserFollow> FollowedUsers { get; set; }  // Usuarios que el usuario sigue
        public ICollection<UserFollow> Followers { get; set; }      // Usuarios que siguen al usuario
        */
    }
}
