using Microsoft.AspNetCore.Mvc;
using MyLittleBluRayThequeProject.Models;
using MyLittleBluRayThequeProject.Business;
using MyLittleBluRayThequeProject.Repositories;
using System.Diagnostics;
using Newtonsoft.Json;


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

        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            HttpClient client = new HttpClient();
            model.BluRays = brRepository.GetListeBluRay();
            foreach(var br in model.BluRays)
            {
                br.Acteurs = PersonneRepository.GetActeurs(br.Id);
                br.Scenariste = PersonneRepository.GetScenariste(br.Id);
                br.Realisateur = PersonneRepository.GetRealisateur(br.Id);
            }

            return View(model);
        }


        public IActionResult SelectedBluRay()
        {
            IndexViewModel model = new IndexViewModel();
            HttpClient client = new HttpClient();



            string apiPath = "https://localhost:7266/blurays";

            string url = Request.Path;
            string[] urlSplit = url.Split('/');
            int idBr = int.Parse(urlSplit[urlSplit.Length - 1]);

            string urlRequest = apiPath + idBr;



            Task<string> responses = client.GetStringAsync(urlRequest);
            DTOs.BluRay result = JsonConvert.DeserializeObject<DTOs.BluRay>(responses.Result);

            model.SelectedBluRay = result;
            model.SelectedBluRay.Acteurs = PersonneRepository.GetActeurs(result.Id);
            model.SelectedBluRay.Realisateur = PersonneRepository.GetRealisateur(result.Id);
            model.SelectedBluRay.Scenariste = PersonneRepository.GetScenariste(result.Id);

            return View(model);
        }

        public IActionResult RecupererAPIExterne(string url)
        {
            IndexViewModel model = new IndexViewModel();
            if(url != null)
            {
                
                string apiPath = "https://localhost:7266/blurays";
                CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddMinutes(15);
                Response.Cookies.Append("url", apiPath, option);
                model.BluRays = FetchDataFromExternalAPI(apiPath);
            }
            return View(model);
        }

        public IActionResult RenduBluRay()
        {
            return View();
        }

        public IActionResult EmprunterBluRayExterne()
        {
            IndexViewModel model = new IndexViewModel();
            string url = Request.Path;
            string[] urlSplit = url.Split('/');
            int idBr = int.Parse(urlSplit[urlSplit.Length - 1]);

            HttpClient client = new HttpClient();
            string urlFromCookies = Request.Cookies["url"] + "/" + idBr;
            Task<string> responses = client.GetStringAsync(urlFromCookies);
            DTOs.BluRay bluRayEmprunt = JsonConvert.DeserializeObject<DTOs.BluRay>(responses.Result);

            
            brRepository.enregistrerBluRay(bluRayEmprunt);
            brRepository.setProprietaire(idBr, Request.Cookies["url"]);

            EmprunterBluRayExterne(idBr);

            return View("RecupererAPIExterne", model);
        }

        [HttpPost]
        private ObjectResult EmprunterBluRayExterne(long idBr)
        {
            string url = Request.Cookies["url"] + "/" + idBr + "/emprunt";

            return new CreatedResult($"{idBr}", null);
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

        public IActionResult DeleteBluray()
        {
            IndexViewModel model = new IndexViewModel();
            string url = Request.Path;
            string[] urlSplit = url.Split('/');
            int idBr = int.Parse(urlSplit[urlSplit.Length - 1]);

            brRepository.deleteBluray(idBr);

            model.BluRays = brRepository.GetListeBluRay();
            foreach (var br in model.BluRays)
            {
                br.Acteurs = PersonneRepository.GetActeurs(br.Id);
                br.Scenariste = PersonneRepository.GetScenariste(br.Id);
                br.Realisateur = PersonneRepository.GetRealisateur(br.Id);
            }
            
            
            return View("Index",model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<DTOs.BluRay> FetchDataFromExternalAPI(string url)
        {
            HttpClient client = new HttpClient();
            Task<string> responses = client.GetStringAsync(url);
            List<DTOs.BluRay> fetchedBRs = JsonConvert.DeserializeObject<List<DTOs.BluRay>>(responses.Result);
            return fetchedBRs;
        }

        
    }
}