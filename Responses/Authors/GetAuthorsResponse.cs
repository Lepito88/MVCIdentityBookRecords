using MVCIdentityBookRecords.Models;
namespace MVCIdentityBookRecords.Responses.Authors
{
    public class GetAuthorsResponse : BaseResponse
    {
        public List<Author> Authors { get; set; }
    }
}
