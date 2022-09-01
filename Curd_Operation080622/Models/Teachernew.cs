using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Curd_Operation080622.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace Curd_Operation080622.Models
{
    public class Teachernew
    {
        //internal DateTime DOJ;

        public int Teacher_Id { get; set; }
        public string Teacher_Name { get; set; }
        public int Teacher_Age { get; set; }
        public string Teacher_Address { get; set; }
        public int Teacher_Salary { get; set; }
        public string Teahching_Class { get; set; }
        public DateTime DOJ { get; set; }
        public int Country { get; set; }
        public int Gender { get; set; }
        public int Hobbie { get; set; }
        public int State { get; set; }

        public List<Country> countries { get; set; }
       public List<TblState> tblstates { get; set; }
        public List<Gender> lstGenders { get; set; }
        public List<HobbieDetail> lstHobbies { get; set; }

    }

    public class HobbieDetail
    {
        public int Hobby_Id { get; set; }
        public string Hobby_Name { get; set; }
        public bool ischecked { get; set; }


    }
}
