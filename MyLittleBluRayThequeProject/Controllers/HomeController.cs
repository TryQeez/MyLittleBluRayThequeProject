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

        public IActionResult SelectedBluRay([FromRoute]long idBr)
        {
            IndexViewModel model = new IndexViewModel();
            model.BluRays = brRepository.GetListeBluRay();
            model.SelectedBluRay = brRepository.GetBluRay(idBr);
            return View(model);
        }


        public IActionResult EnregistrerBluRay(string titre, string version, List<int> acteur, int realisateur, int scenariste, DateTime date)
        {
            IndexViewModel model = new IndexViewModel();
            List<DTOs.Personne> acteurList = new List<DTOs.Personne>();
            DTOs.Personne realisateurPersonne = null;
            DTOs.Personne scenaristePersonne = null;
            Console.WriteLine(acteur);
            model.Personnes = personneRepository.GetListePersonne();
            foreach(var i in acteur)
            {
                acteurList.Add(model.Personnes[i]);
            }
            if (realisateur != null)
            {
                realisateurPersonne = model.Personnes[realisateur];
                scenaristePersonne = model.Personnes[scenariste];
            }
            
            model.NewBluRay = new DTOs.BluRay
            {   
                Titre = titre,
                Version = version,
                Acteurs = acteurList,
                Realisateur = realisateurPersonne,
                Scenariste = scenaristePersonne,
                DateSortie = date
            };

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