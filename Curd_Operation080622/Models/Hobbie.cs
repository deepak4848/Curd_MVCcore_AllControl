using System.ComponentModel.DataAnnotations;

namespace Curd_Operation080622.Models
{
    public class Hobbie
    {
        [Key]
        public int Hobby_Id { get; set; }
        public string Hobby_Name { get; set; }
    }
}
