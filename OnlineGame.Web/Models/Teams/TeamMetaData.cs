using System.ComponentModel.DataAnnotations;
namespace OnlineGame.Web.Models
{
    public class TeamMetaData
    {
        [Display(Name = "Team Name")]
        public string Name { get; set; }
        // Here is the place you may add some extra code for the property
        // which is already in the auto-generate partail class.
    }
}