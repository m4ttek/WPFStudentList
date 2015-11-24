using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsList.model
{
    public class Student
    {
        public int StudentId { set; get; }
        [MaxLength(32)]
        public String FirstName { set; get; }
        [MaxLength(32)] 
        public String LastName { set; get; }
        [MaxLength(10)]
        public String IndexNo { set; get; }
        [Required]
        public virtual Group Group { get; set; }
    }
}
