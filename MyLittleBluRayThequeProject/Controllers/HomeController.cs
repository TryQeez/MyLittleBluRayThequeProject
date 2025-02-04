﻿using Microsoft.AspNetCore.Mvc;
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



            string url = Request.Path;
            string[] urlSplit = url.Split('/');
            int idBr = int.Parse(urlSplit[urlSplit.Length - 1]);


            model.SelectedBluRay = brRepository.GetBluRay(idBr);
            model.SelectedBluRay.Acteurs = PersonneRepository.GetActeurs(idBr);
            model.SelectedBluRay.Realisateur = PersonneRepository.GetRealisateur(idBr);
            model.SelectedBluRay.Scenariste = PersonneRepository.GetScenariste(idBr);

            return View(model);
        }

        public IActionResult RecupererAPIExterne(string url)
        {
            IndexViewModel model = new IndexViewModel();
            if(url != null)
            {
                CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddMinutes(15);
                string buildURL = "https://" + url + "/blurays";
                Response.Cookies.Append("url", buildURL, option);
                model.ExternalBluRays = FetchDataFromExternalAPI(buildURL);
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
            DTOs.ExternalBluRay bluRayEmprunt = JsonConvert.DeserializeObject<DTOs.ExternalBluRay>(responses.Result);


            DTOs.BluRay builtBR = new DTOs.BluRay();
            builtBR.Id = bluRayEmprunt.Id;
            builtBR.Titre = bluRayEmprunt.Titre;
            builtBR.Version = bluRayEmprunt.Version;
            builtBR.Disponible = bluRayEmprunt.Disponible;
            builtBR.Duree = bluRayEmprunt.Duree;
            builtBR.DateSortie = bluRayEmprunt.DateSortie;

            brRepository.enregistrerBluRay(builtBR);
            brRepository.setProprietaire(idBr, Request.Cookies["url"]);

            string urlEmprunt = Request.Cookies["url"] + "/" + idBr + "/emprunt";
            client.PostAsync(urlEmprunt, null);

            return View("RecupererAPIExterne", model);
        }

        private IActionResult RenduRequete(long idBr)
        {
            IndexViewModel model = new IndexViewModel();
            HttpClient client = new HttpClient();
            string url = Request.Cookies["url"] + "/" + idBr + "/emprunt";
            client.DeleteAsync(url);
            brRepository.deleteBluray((int) idBr);
            model.BluRays = brRepository.GetListeBluRay();
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

        private List<DTOs.ExternalBluRay> FetchDataFromExternalAPI(string url)
        {
            HttpClient client = new HttpClient();
            Task<string> responses = client.GetStringAsync(url);
            List<DTOs.ExternalBluRay> fetchedBRs = JsonConvert.DeserializeObject<List<DTOs.ExternalBluRay>>(responses.Result);
            return fetchedBRs;
        }

        
    }
}