using System;
using System.Collections.Generic;

namespace Task1
{
    public partial class Users
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual Companies Company { get; set; }
    }
}
