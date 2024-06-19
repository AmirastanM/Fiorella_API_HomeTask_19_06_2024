using System.ComponentModel.DataAnnotations;

namespace FiorelloAPI.Models
{
    public class Expert : BaseEntity
    {
        public string FullName { get; set; }   
        public string Position { get; set; }
        public string Image { get; set; }
    }
}
