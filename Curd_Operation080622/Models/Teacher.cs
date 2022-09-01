using Microsoft.EntityFrameworkCore;
using Curd_Operation080622.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;


namespace Curd_Operation080622.Models
{
    public class Teacher
    {
        
        [Key]
        public int Teacher_Id { get; set; }
    
        public string Teacher_Name { get; set; }
        public int Teacher_Age { get; set; }
        public string Teacher_Address { get; set; }
        public int Teacher_Salary { get; set; }
        public string Teahching_Class { get; set; }
        public DateTime DOJ { get; set; } 
        public int Country { get; set; }
        public int State { get; set; }
        public int Gender { get; set; }
        public string Hobbie { get; set; }
        
      
        //public List<Country> countries { get; set; }
        //public List<Gender> lstGenders { get; set; }
        //public List<Hobbie> lstHobbies { get; set; }
    }

   

}
