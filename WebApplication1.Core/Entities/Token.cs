using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Core.Entities
{
    public class Token : BaseEntity
    {
        public string token { get; set; }

        public Boolean isValid { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }


    }
}
