using Microsoft.AspNetCore.Mvc;
using MyLittleBluRayThequeProject.DTOs;
using MyLittleBluRayThequeProject.Models;
using MyLittleBluRayThequeProject.Repositories;

namespace MyLittleBluRayThequeProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BluRaysController
    {
        private readonly ILogger<BluRaysController> _logger;
        private readonly BluRayRepository _brRepository;

        public BluRaysController(ILogger<BluRaysController> logger)
        {
            _logger = logger;
            _brRepository = new BluRayRepository();
        }

        [HttpGet("/blurays")]
        public ObjectResult Get()
        {
            List<BluRay> br = _brRepository.GetListeBluRay();
            foreach(BluRay brItem in br)
            {
                brItem.Realisateur = PersonneRepository.GetRealisateur(brItem.Id);
                brItem.Scenariste = PersonneRepository.GetScenariste(brItem.Id);
                brItem.Acteurs = PersonneRepository.GetActeurs(brItem.Id);
            }
            List<InfoBluRayApiViewModel> bluRays = br.ConvertAll(InfoBluRayApiViewModel.ToModel);
            return new OkObjectResult(bluRays);
        }

        [HttpGet("/blurays/{id}")]
        public ObjectResult GetSpe(long id)
        {
            BluRay bluRay = _brRepository.GetBluRay(id);
            bluRay.Realisateur = PersonneRepository.GetRealisateur(id);
            bluRay.Scenariste = PersonneRepository.GetScenariste(id);
            bluRay.Acteurs = PersonneRepository.GetActeurs(id);
            List<BluRay> br = new List<BluRay> { bluRay };
            List<InfoBluRayApiViewModel> infoBr = br.ConvertAll(InfoBluRayApiViewModel.ToModel);
            InfoBluRayApiViewModel targetBr = infoBr[0];
            return new OkObjectResult(targetBr);
        }

        [HttpPost("/blurays/{idBluray}/emprunt")]
        public ObjectResult EmprunterBluRay(long idBluray)
        {
            // Vérifier que le livre existe, qu'il est disponible et qu'il n'est pas emprunté
            _brRepository.SetBluRayEmprunte(idBluray);

            // Passer le livre en emprunté = true et disponible = false
            return new CreatedResult($"{idBluray}", null);
        }

        [HttpDelete("/blurays/{idBluray}/emprunt")]
        public ObjectResult RenduBluRay(long idBluRay)
        {
            // On rend le bluray
            _brRepository.SetBluRayRendu(idBluRay);

            return new CreatedResult($"{idBluRay}", null);
        }
    }
}
