using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Responses;

namespace MVCIdentityBookRecords.Responses
{
    public class CategoryToBookResponse : BaseResponse
    {
        public int CatecoryId { get; set; }
        public int Bookid { get; set; }
        public string CategoryName { get; set; }
        public string BookName { get; set; }
    }
}
