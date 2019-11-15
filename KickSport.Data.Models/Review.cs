using KickSport.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace KickSport.Data.Models
{
    public class Review : BaseModel<Guid>
    {
        public string Text { get; set; }

        public string CreatorId { get; set; }

        public ApplicationUser Creator { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public DateTime LastModified { get; set; }
    }
}
