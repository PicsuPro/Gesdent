using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VsProject.Models
{
    public class UserModel
    {
        public SqlGuid Id { get; set; }
        public string UserName { get; set; }
        public string Hash { get; set; }
        public string Email { get; set; }
       
    }
}