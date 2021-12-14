using MyLittleBluRayThequeProject.DTOs;

namespace MyLittleBluRayThequeProject.Models
{
    public class IndexViewModel
    {
        public List<BluRay> BluRays { get; set; }

        public BluRay SelectedBluRay { get; set; }

        public BluRay NewBluRay { get; set; }

        public List<Personne> Personnes { get; set; }

        public Personne NewPersonne { get; set; }
    }
}
