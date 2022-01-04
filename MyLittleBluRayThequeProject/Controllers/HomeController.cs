using Microsoft.AspNetCore.Mvc;
using MyLittleBluRayThequeProject.Models;
using MyLittleBluRayThequeProject.Business;
using MyLittleBluRayThequeProject.Repositories;
using System.Diagnostics;

namespace MyLittleBluRayThequeProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly BluRayRepository brRepository;

        private readonly PersonneRepository personneRepository;

        private readonly BluRayBusiness brBusiness;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            brRepository = new BluRayRepository();
            personneRepository = new PersonneRepository();
            brBusiness = new BluRayBusiness();
        }

        public IActionResult Index(long? id)
        {
            IndexViewModel model = new IndexViewModel();
            model.BluRays = brRepository.GetListeBluRay();
            if (id != null)
            {
                model.SelectedBluRay = brBusiness.GetBluRay(id.Value);
            }
            return View(model);
        }

        public ActionResult PrintMessageToConsole()
        {
            Console.WriteLine("Test");
            return View("Index");
        }

        public IActionResult SelectedBluRay()
        {
            string url = Request.Path;
            string[] urlSplit = url.Split('/');
            int idBr = int.Parse(urlSplit[urlSplit.Length - 1]);
            IndexViewModel model = new IndexViewModel();
            model.SelectedBluRay = brRepository.GetBluRay(idBr);
            return View(model);
        }


        public IActionResult EnregistrerBluRay(string titre, string version, List<int> acteur, int realisateur, int scenariste, DateTime date, int duree)
        {
            IndexViewModel model = new IndexViewModel();
            List<DTOs.Personne> acteurList = new List<DTOs.Personne>();
            DTOs.Personne realisateurPersonne = null;
            DTOs.Personne scenaristePersonne = null;
            model.Personnes = personneRepository.GetListePersonne();
            int idNewBluRay = brRepository.GetListeBluRay().Count();


            if (titre != null)
            {
                DTOs.BluRay bluRay = new DTOs.BluRay
                {
                    Id = idNewBluRay,
                    Titre = titre,
                    Version = version,
                    DateSortie = date,
                    Duree = TimeSpan.FromMinutes(duree)

                };
                brRepository.enregistrerBluRay(bluRay);
                foreach (var i in acteur)
                {
                    personneRepository.enregistrerActeur(i, idNewBluRay);
                }
                personneRepository.enregistrerScenariste(scenariste, idNewBluRay);
                personneRepository.enregistrerRealisateur(realisateur, idNewBluRay);

            }
            return View(model);
        }

        public IActionResult EnregistrerPersonne(string nom, string prenom, DateTime dateNaissance, string nationalite)
        {
            DTOs.Personne personne = new DTOs.Personne
                    {
                        Nom = nom,
                        Prenom = prenom,
                        Nationalite = nationalite,
                        DateNaissance = dateNaissance
                    };
            if (nom != null)
            {
                personneRepository.enregistrerPersonne(personne);
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}