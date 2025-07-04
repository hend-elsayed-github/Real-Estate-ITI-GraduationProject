using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace Real_Estate_Project.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string firstName {  get; set; }
        public string lastName { get; set; }

        public string? image {  get; set; }

        public bool? isActive {  get; set; }

        public DateTime createdAt { get; set; }= DateTime.Now;

        //verificationCode 1-m(verificationCode) -  all  - has
        public virtual List<VerificationCode>? VerificationCodes { get; set; }

        //Notifications 1-m(Notifications) -  renter,owner  -  has
        public virtual List<Notification>? Notifications { get; set; }

        //Reviews 1-m(Reviews) -  renter  -  add
        public virtual List<Review>? Reviews { get; set; }

        //CommunityPost 1-m(CommunityPost) -  renter,owner  -  post
        public virtual List<CommunityPost>? CommunityPosts { get; set; }

        //Comment 1-m(Comment) -  renter,owner  -  add
        public virtual List<Comment>? Comments { get; set; }

        //Payments 1-m(Payments) -  renter  -  pay  ------------------------------------------?????????
        public virtual List<Payment>? Payments { get; set; }

        //Units 1-m(Units) -  renter  -  rent
        public virtual List<Unit>? Units { get; set; }

        //Addvertisement 1-m(Addvertisement) -  owner  -  add,  [visitor & renter m-m  see]----------???
        public virtual List<Addvertisement>? Addvertisements { get; set; }

        //BankAccount 1-m(BankAccount) -  renter-owner  -  has
        public virtual List<BankAccount>? BankAccounts { get; set; }

    }
}
