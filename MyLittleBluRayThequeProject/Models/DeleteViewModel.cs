
using MyLittleBluRayThequeProject.DTOs;

namespace MyLittleBluRayThequeProject.Models
{
    public class DeleteViewModel
    {
        public List<BluRay> BluRays { get; set; }

        public BluRay SelectedBluRay { get; set; }
    }
}
