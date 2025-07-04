using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate_Project.Models
{
    public class VerificationCode
    {
        public int id {  get; set; }

        public string code { get; set; }

        public DateTime createAt { get; set; }= DateTime.Now;

        public string activationPeriod { get; set; }

        // user-verificationCode 1-m(verificationCode)
        [ForeignKey("user")]
        public string userId { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}
