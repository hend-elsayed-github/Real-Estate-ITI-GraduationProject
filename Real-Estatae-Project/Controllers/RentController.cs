using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Real_Estatae_Project.DTO.Rent;
using Real_Estatae_Project.Repositories;
using System.Security.Claims;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(Roles = "renter")]
    [Authorize]

    public class RentController : ControllerBase
    {

        IRentRepositories  _rentRepository;
        public RentController(IRentRepositories rentRepository)
        {
            _rentRepository = rentRepository;
        }
        #region Renter
        [HttpGet("UnpaidRents")]
        [Authorize(Roles = "Renter")]
        public async Task<ActionResult<IEnumerable<RentInfoDTO>>> UnpaidRents()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

           
            var unpaidrents = await _rentRepository.UnpaidRentsAsync(userId);

            var rentInfoDTOs = unpaidrents.Select(r => new RentInfoDTO
            {

                RentId=r.id,
                RentStatus= "unpaid",
                RentValue=r.unit.price,
                DueDate=r.dueDate,

            }).ToList();


            return Ok(rentInfoDTOs);
        }
        
        [HttpGet("HistoryRents")]
        [Authorize(Roles = "Renter")]
        public async Task<ActionResult<IEnumerable<RentInfoDTO>>> HistoryRents()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var rents = await _rentRepository.HistoryRentsAsync(userId);

            var rentInfoDTOs = rents.Select(r => new RentHistoryfoDTO
            {
               
                
                RentStatus=r.IsPaid ? "paid" : "unpaid",
                RentValue=r.unit.price,
                DueDate=r.dueDate,
                PaymentDate = r.Payment?.PaymentDate,


            }).ToList();
            return Ok( rentInfoDTOs );
        }
        #endregion


        #region Owner
        [HttpGet("MonthRents")]
        [Authorize(Roles = "AllRents")]
        public async Task<ActionResult<IEnumerable<RentInfoDTO>>> MonthRents(int month,  int year)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();


            var AllRents = await _rentRepository.MonthRentsAsync(userId,  month,  year);

            var OwnerRentsDTO = AllRents.Select(r => new OwnerRentsDTOcs
            {
                
                
               RenterName= r.unit.renter?.firstName + r.unit.renter?.lastName,
                RentStatus = r.IsPaid ? "paid" : "unpaid",
                RentValue = r.Rentvalue,
                DueDate = r.dueDate,
                PaymentDate = r.Payment?.PaymentDate,
                

            }).ToList();


            return Ok(OwnerRentsDTO);
        }
        #endregion
    }
}
