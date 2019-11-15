using System;

namespace KickSport.Data.Common
{
    public class BaseModel<TId>
    {
        public virtual TId Id { get; set; }
    }
}
