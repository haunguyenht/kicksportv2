using System;
using System.Collections.Generic;
using System.Text;

namespace KickSport.Data.Models
{
    public class UsersLikes
    {
        public Guid Id { get; set; }

        public string ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }
    }
}
