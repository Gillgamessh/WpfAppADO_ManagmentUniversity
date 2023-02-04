using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppADO_ManagmentUniversity.Abstraction
{
    public  class Student
    {
        [Key]
        public int studId { get; set; }        
        public string FullName { get; set; }
        public int CurGroupId { get; set; }
        public int Marks { get; set; }

        public string ContactInfo { get; set; }
        public Group Group { get; set; }
        
        

    }
}
