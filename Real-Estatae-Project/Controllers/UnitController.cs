using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.DTO.Unit;
using Real_Estatae_Project.Images;
using Real_Estatae_Project.Repositories;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        IUnitRepository unitRepo;
        ICloudinaryRepository cloudinaryRepository;
        public UnitController(IUnitRepository _unitRepo, ICloudinaryRepository _cloudinaryRepository)
        {
            unitRepo = _unitRepo;
            cloudinaryRepository = _cloudinaryRepository;

        }
        #region GetAll
        [HttpGet]
        public IActionResult GetAll()
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<AllUnit> uniits = unitRepo.GetAll(ownerId);
            return Ok(uniits);

        }
        #endregion

        #region GetById
        [HttpGet("{id:int}")]
        [RequestSizeLimit(104_857_600)] // 100 MB
        [RequestFormLimits(MultipartBodyLengthLimit = 104_857_600)]
        public IActionResult GetById(int id)
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Unit unit = unitRepo.GetById(id, ownerId);
            return Ok(unit);
        }
        #endregion

        #region AddUnit
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddUnitsDTO unitDTO)
        {
            string _ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int _communityId = unitRepo.GetCommunityId(_ownerId);


            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

                string? imageFromReq1 = await GetImageName.GetImageNameFn(unitDTO.image1);
                string? imageFromReq2 = await GetImageName.GetImageNameFn(unitDTO.image2);
                string? imageFromReq3 = await GetImageName.GetImageNameFn(unitDTO.image3);
                //string? imageFromReq1 = await cloudinaryRepository.UploadImageAsync(unitDTO.image1);
                //string? imageFromReq2 = await cloudinaryRepository.UploadImageAsync(unitDTO.image2);
                //string? imageFromReq3 = await cloudinaryRepository.UploadImageAsync(unitDTO.image3);


                Unit unit = new Unit
        {
            status = unitDTO.status,
            price = unitDTO.price,
            description = unitDTO.description,
            type = unitDTO.type,
            street = unitDTO.street,
            city = unitDTO.city,
            image1 = imageFromReq1,
            image2 = imageFromReq2,
            image3 = imageFromReq3,
            area = unitDTO.area,
            renterSSN = unitDTO.renterSSN,
            buildingNumber = unitDTO.buildingNumber,
            flatNumber = unitDTO.flatNumber,
            electricityNum = unitDTO.electricityNum,
            waterNum = unitDTO.waterNum,
            gasNum = unitDTO.gasNum,
            ownerId = _ownerId,
            communityId = _communityId,
            isDeleted = false,

        };



        Unit result = unitRepo.Add(unit);

        return CreatedAtAction("GetById", new { id = result.id }, result);

    

    

}


#endregion

        #region filterbytype and by status
        [HttpGet("filter")]
        public IActionResult FilterByType([FromQuery] string type, [FromQuery] string status)
        {
             string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
             List<Unit> units=unitRepo.Filter(type,status,userId);
             return Ok(units);
        }
        #endregion
 
        #region Search
        [HttpGet("Search")]
        public IActionResult Search([FromQuery] string searchTerm)
        {
             string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
             List<Unit> units = unitRepo.Search(searchTerm, ownerId);
             return Ok(units);
 
        }
 
        #endregion

       

        #region Update
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update( int id, [FromForm] UnitDTO updatingRef)
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                await unitRepo.Update(ownerId, id, updatingRef);
               
                return Ok(new { success = true });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return BadRequest(errors);
        }


        #endregion

        #region Delete
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (unitRepo.Delete(ownerId, id) == true)
            {
                unitRepo.Save();
                return Ok(new { success = true, message="Deleted" });
            }
            return BadRequest(new { success = false, message="Bad Request" });
        }
        #endregion
    }
}
