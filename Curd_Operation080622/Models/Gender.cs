
using System.ComponentModel.DataAnnotations;

namespace Curd_Operation080622.Models
{
    public class Gender
    {
        [Key]
        public int Gender_Id { get; set; }
        public string Gender_Name { get; set; }
    }
}
