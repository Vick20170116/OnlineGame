using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineGame.Web.Models
{
    public class GamerMetaData
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [Display(Name = "Team")]
        public int? TeamId { get; set; }
    }
}
