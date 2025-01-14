using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace StudentCrud.Models
{
    public class StudentModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z'’\- ]*$",ErrorMessage ="Invalid characters in name")]

        public string SName { get; set; }

        [Required]
        
        public int RollNo { get; set; }

        [Required]
        public string Dept { get; set; }

        [Required]
        public int DeptId { get; set; }

        [Required]
        public DateTime DOB{ get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        
        public string SAddress { get; set; }

        [Required]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string PhoneNum { get; set; }

    }
}
