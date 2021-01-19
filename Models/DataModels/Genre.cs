using Library.Core.Common;
using System;

namespace Models.DataModels
{
    public class Genre : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Expire { get; set; }
        public string Code { get; set; }
        public string ImageUrl { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
