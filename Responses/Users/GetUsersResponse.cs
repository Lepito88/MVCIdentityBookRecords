using MVCIdentityBookRecords.Models;

namespace MVCIdentityBookRecords.Responses.Users
{
    public class GetUsersResponse : BaseResponse
    {
        public List<ApplicationUser> Users { get; set; }
    }
}
