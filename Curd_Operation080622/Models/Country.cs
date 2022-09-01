using Microsoft.EntityFrameworkCore;
using Curd_Operation080622.Models;
using System.ComponentModel.DataAnnotations;

namespace Curd_Operation080622.Models
{
    public class Country
    {
        [Key]
        public int  CId { get; set; }
        public string CName { get; set; }
    }
}
