using MyLittleBluRayThequeProject.DTOs;

namespace MyLittleBluRayThequeProject.Repositories
{
    public class BluRayRepository
    {
        /// <summary>
        /// Consctructeur par défaut
        /// </summary>
        public BluRayRepository()
        {

        }

        public List<BluRay> GetListeBluRay()
        {
            return new List<BluRay>
            { 
                new BluRay
                {
                    Id = 0,
                    Titre = "My Little film 1",
                    DateSortie = DateTime.Now,
                    Version = "Courte",
                    Scenariste = new Personne
                    {
                        Id = 1000,
                        Nom = "Nom Scenar 1",
                        Prenom = "Prenom Scenar 1",
                        Nationalite = "Fr",
                        DateNaissance = DateTime.Now,
                        Professions = new List<string>{"Scénariste"}
                    },
                    Realisateur = new Personne
                    {
                        Id = 1001,
                        Nom = "Nom Réal 1",
                        Prenom = "Prenom Réal 1",
                        Nationalite = "Fr",
                        DateNaissance = DateTime.Now,
                        Professions = new List<string>{"Réalisateur"}
                    },
                    Acteurs = new List<Personne>
                    {
                        new Personne
                        {
                            Id = 0,
                            Nom = "Per",
                            Prenom = "Sonne",
                            Nationalite = "Fr",
                            DateNaissance = DateTime.Now,
                            Professions = new List<string>{"Acteur"}
                        }
                    }
                },
                new BluRay
                {
                    Id = 1,
                    Titre = "My Little film 2",
                    DateSortie = DateTime.Now,
                    Version = "Longue",
                    Scenariste = new Personne
                    {
                        Id = 1002,
                        Nom = "Nom Scenar 2",
                        Prenom = "Prenom Scenar 2",
                        Nationalite = "Fr",
                        DateNaissance = DateTime.Now,
                        Professions = new List<string>{"Scénariste"}
                    },
                    Realisateur = new Personne
                    {
                        Id = 1003,
                        Nom = "Nom Réal 2",
                        Prenom = "Prenom Réal 2",
                        Nationalite = "Fr",
                        DateNaissance = DateTime.Now,
                        Professions = new List<string>{"Réalisateur"}
                    },
                    Acteurs = new List<Personne>
                    {
                        new Personne
                        {
                            Id = 0,
                            Nom = "Per",
                            Prenom = "Sonne",
                            Nationalite = "Fr",
                            DateNaissance = DateTime.Now,
                            Professions = new List<string>{"Acteur"}
                        }
                    }
                }
            };
        }
    }
}
