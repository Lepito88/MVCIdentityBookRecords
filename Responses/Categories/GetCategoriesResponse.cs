using MVCIdentityBookRecords.Models;

namespace MVCIdentityBookRecords.Responses.Categories
{
    public class GetCategoriesResponse : BaseResponse
    {
        public List<Category> Categories { get; set; }
    }
}
