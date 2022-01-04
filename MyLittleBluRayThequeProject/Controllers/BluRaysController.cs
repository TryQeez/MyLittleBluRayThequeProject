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
            List<InfoBluRayApiViewModel> bluRays = br.ConvertAll(InfoBluRayApiViewModel.ToModel);
            return new OkObjectResult(bluRays);
        }

        [HttpGet("/blurays/{id}")]
        public ObjectResult GetSpe(long id)
        {
            List<BluRay> br = new List<BluRay> { _brRepository.GetBluRay(id) };
            List<InfoBluRayApiViewModel> infoBr = br.ConvertAll(InfoBluRayApiViewModel.ToModel);
            InfoBluRayApiViewModel targetBr = infoBr[0];
            return new OkObjectResult(targetBr);
        }
    }
}
