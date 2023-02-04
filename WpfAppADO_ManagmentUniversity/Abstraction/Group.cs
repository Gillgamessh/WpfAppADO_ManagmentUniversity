using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppADO_ManagmentUniversity.Abstraction
{
    public class Group
    {
        public Group() 
        {
            Students = new HashSet<Student>();
        } 
        public int groupId { get; set; }
        public string GroupName { get; set; }
        

        public ICollection<Student> Students { get; set; }
    }
}
