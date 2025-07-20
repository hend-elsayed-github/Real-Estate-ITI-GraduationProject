using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.DTO.Advertisement;
using Real_Estatae_Project.Repositories;
using Real_Estate_Project.Models;
using System.Security.Claims;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementRepository adsRepository;

        public AdvertisementController(IAdvertisementRepository _adsRepository)
        {
            this.adsRepository = adsRepository;
        }



        #region add advertisement
        [HttpPost]
        public async Task<IActionResult> AddAdvertisement([FromBody] AdvertisementDTO newAd)
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(ownerId) || !User.IsInRole("Owner"))
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Addvertisement newOne = new Addvertisement
            {
                title = newAd.title,
                description = newAd.description,
                userId = ownerId,
                unitId = newAd.unitId,
                isDeleted = false
            };
            var addedAd = await adsRepository.Add(newOne);

            if (addedAd == null)
            {
                return BadRequest(new { message = "Failed to add advertisement." });
            }

            return Ok(new  { message="new ad is added" , AdId=addedAd.id});
        }

        #endregion


        #region edit advertisement

        [HttpPut("{id}")]       
        public async Task<IActionResult> EditAdvertisement(int id, [FromBody] AdvertisementDTO updatedAd)
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(ownerId) || !User.IsInRole("Owner"))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Assuming you have a method in the repository to edit the advertisement
            var result = await adsRepository.Edit(id, updatedAd);
            if (!result)
            {
                return NotFound(new { message = "Advertisement not found or you do not have permission to edit it." });
            }
            return Ok(new { message = "Advertisement updated successfully." });
        }

        #endregion

        //////////////////

        #region get all
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Addvertisement> addvertisements = adsRepository.GetAll();
            return Ok(new { success = true, message = "return all successfully", data = addvertisements });
        }
        #endregion

        #region getby id
        [HttpGet("{id:int}")]
        public IActionResult GetbyId(int id)
        {
            Addvertisement addvertisement = adsRepository.GetById(id);
            if (addvertisement != null)
            {
                return Ok(new { success = true, message = "found addvertisement", data = addvertisement });
            }
            return NotFound(new { success = false, message = "notfound addvertisement" });
        }
        #endregion

        #region delete
        [Authorize(Roles = "Owner")]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            string ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isDelete = adsRepository.DeleteAds(id, ownerId);
            if (isDelete)
            {
                return Ok(new { success = true, message = "deleted" });
            }
            return BadRequest(new { success = false, message = "failed" });
        }
        #endregion

        #region GetLastTwo
        [HttpGet("LastTwo/{communityId:int}")]
        public IActionResult GetLastTwo(int communityId)
        {
            List<Addvertisement> addvertisements = adsRepository.GetLastTwoAdsByCommunityOwner(communityId);

            return Ok(new
            {
                success = true,
                message = "Returned last two ads by community owner.",
                data = addvertisements
            });
        }
        #endregion

    }
}
