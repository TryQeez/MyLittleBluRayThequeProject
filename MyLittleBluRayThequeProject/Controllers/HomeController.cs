using Microsoft.AspNetCore.Mvc;
using MyLittleBluRayThequeProject.Models;
using MyLittleBluRayThequeProject.Repositories;
using System.Diagnostics;

namespace MyLittleBluRayThequeProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly BluRayRepository brRepository;

        private readonly PersonneRepository personneRepository;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            brRepository = new BluRayRepository();
            personneRepository = new PersonneRepository();
        }

        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            model.BluRays = brRepository.GetListeBluRay();
            return View(model);
        }

        public IActionResult SelectedBluRay([FromRoute]long idBr)
        {
            IndexViewModel model = new IndexViewModel();
            model.BluRays = brRepository.GetListeBluRay();
            model.SelectedBluRay = model.BluRays.FirstOrDefault(x => x.Id == idBr);
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
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}