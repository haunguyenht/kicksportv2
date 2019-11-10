using System;

namespace KickSport.Data.Common
{
    public abstract class BaseModel<TId>
    {
        public virtual TId Id { get; set; }
    }
}
