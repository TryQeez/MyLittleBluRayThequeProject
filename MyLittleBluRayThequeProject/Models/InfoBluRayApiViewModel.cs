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

        public string Scenariste { get; set; }

        public string Realisateur { get; set; }

        public List<string> Acteur{ get; set; }

        public static InfoBluRayApiViewModel ToModel(BluRay dto)
        {
            List<string> acteurList = new List<string>();
            if (dto == null)
            {
                return null;
            }
            foreach(var acteur in dto.Acteurs)
            {
                acteurList.Add(acteur.Nom+" "+acteur.Prenom);
            }
            return new InfoBluRayApiViewModel { Id = dto.Id, Titre = dto.Titre, DateSortie = dto.DateSortie, Version = dto.Version, Disponible = dto.Disponible, Duree = dto.Duree, Realisateur = dto.Realisateur.Nom +" "+ dto.Realisateur.Prenom, Scenariste = dto.Scenariste.Nom +" "+dto.Scenariste.Prenom, Acteur = acteurList };
        }
    }
}
