using Microsoft.AspNetCore.Identity;

namespace Api.Entities
{
    public class AppUser:IdentityUser<int>
    {
        //- estes campos não são mais necessários por causa do 'IdentityUser'
        // public int Id { get; set; }   
        // public string UserName { get; set; } 
        // public byte[] PasswordHash { get; set; }
        // public byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; }= DateTime.UtcNow;
        public DateTime LastActive { get; set; }= DateTime.UtcNow;
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }

        //-relação de muitos para muitos
        public List<UserLike> LikedByUsers{ get; set; }
        public List<UserLike> LikedUsers{ get; set; }

        //-relação de muitos para muitos
        public List<Message> MessagesSent { get; set; }
        public List<Message> MessagesReceived { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; }
        
    }
}