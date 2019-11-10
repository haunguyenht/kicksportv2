using KickSport.Data.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace KickSport.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<UsersLikes> ProductsLiked { get; set; }
    }
}
