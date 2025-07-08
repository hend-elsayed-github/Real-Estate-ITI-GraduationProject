using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.DTO;
using Real_Estatae_Project.Repositories;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitController : ControllerBase
    {
        IUnitRepository  unitRepo;
        public UnitController(IUnitRepository _unitRepo)
        {
            unitRepo= _unitRepo;
        }


        #region Update
        [HttpPut("{id}")]
        public IActionResult Update(int id, UnitDTO updatingRef)
        {
            if (ModelState.IsValid) 
            {
                unitRepo.Update(id,updatingRef);
                unitRepo.Save();
                return Ok("updated");
            }
            return BadRequest("Invalid data");
        }


        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (unitRepo.Delete(id) == true)
            {
                unitRepo.Save();
                return Ok("Deleted");
            }
            return BadRequest("Not Found");
        } 
        #endregion
    }
}
