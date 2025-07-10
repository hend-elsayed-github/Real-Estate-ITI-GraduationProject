using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.DTO;
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
        public UnitController(IUnitRepository _unitRepo)
        {
            unitRepo = _unitRepo;
        }
        #region GetAll
        [HttpGet]
        public IActionResult GetAll()
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Unit> uniits = unitRepo.GetAll(ownerId);
            return Ok(uniits);

        }
        #endregion

        #region GetById
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Unit unit = unitRepo.GetById(id, ownerId);
            return Ok(unit);
        }
        #endregion

        #region AddUnit
        [HttpPost]
        public async Task<IActionResult> Add(AddUnitsDTO unitDTO)
        {
            string _ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int _communityId = unitRepo.GetCommunityId(_ownerId);

            string? imageFromReq1 = await GetImageName.GetImageNameFn(unitDTO.image1);
            string? imageFromReq2 = await GetImageName.GetImageNameFn(unitDTO.image2);
            string? imageFromReq3 = await GetImageName.GetImageNameFn(unitDTO.image3);

            Unit unit = new Unit
            {
                status = unitDTO.status,
                price = unitDTO.price,
                description = unitDTO.description,
                type = unitDTO.type,
                street = unitDTO.street,
                image1 = imageFromReq1,
                image2 = imageFromReq2,
                image3 = imageFromReq3,
                area = unitDTO.area,
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

        #region filterbytype
        [HttpGet("type")]
        public IActionResult FilterByType([FromQuery] string type)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string role = User.FindFirstValue(ClaimTypes.Role);
            List<Unit> units = unitRepo.GetByType(type, userId, role);
            return Ok(units);
        }
        #endregion

        #region filterbystatus
        [HttpGet("status")]
        public IActionResult FilterByStatus([FromQuery] string status)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string role = User.FindFirstValue(ClaimTypes.Role);
            List<Unit> units = unitRepo.GetByType(status, userId, role);
            return Ok(units);
        }
        #endregion

        #region Update
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UnitDTO updatingRef)
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                unitRepo.Update(ownerId, id, updatingRef);
                unitRepo.Save();
                return Ok("updated");
            }
            return BadRequest("Invalid data");
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
                return Ok("Deleted");
            }
            return BadRequest("Not Found");
        }
        #endregion
    }
}
