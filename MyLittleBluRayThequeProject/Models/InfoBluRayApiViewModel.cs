using MyLittleBluRayThequeProject.DTOs;

namespace MyLittleBluRayThequeProject.Models
{
    public class InfoBluRayApiViewModel
    {
        /// <summary>
        /// Identifiant technique
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Titre du film contenu sur le Bluray
        /// </summary>
        public string Titre { get; set; }

        /// <summary>
        /// Version du film contenu sur le Bluray
        /// </summary>
        public string Version { get; set; }

        public DateTime DateSortie { get; set; }

        public bool Disponible { get; set; }

        public TimeSpan Duree { get; set; }

        public static InfoBluRayApiViewModel ToModel(BluRay dto)
        {
            if (dto == null)
            {
                return null;
            }
            return new InfoBluRayApiViewModel { Id = dto.Id, Titre = dto.Titre, DateSortie = dto.DateSortie, Version = dto.Version, Disponible = dto.Disponible, Duree = dto.Duree };
        }
    }
}
