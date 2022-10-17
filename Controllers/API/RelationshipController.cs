using MVCIdentityBookRecords.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCIdentityBookRecords.Requests;

namespace MVCIdentityBookRecords.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Basic,Admin")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RelationshipController : BaseApiController
    {
        private readonly IEntityRelationShipManagerService _relationshipManagerService;
        public RelationshipController(IEntityRelationShipManagerService relationShipManagerService)
        {
            _relationshipManagerService = relationShipManagerService;
        }

        [HttpPost("addbookuser")]
        public async Task<ActionResult> AddBookToUserAsync(RelationshipRequest request)
        {
            var response = await _relationshipManagerService.AddBookToUserAsync(request);

            if (!response.Success)
            {
                return BadRequest(new { response.Success, response.Error, response.ErrorCode });
            }
            return Ok(response);
        }
        [HttpPost("removebookuser")]
        public async Task<ActionResult> RemoveBookFromUserAsync(RelationshipRequest request)
        {
            var response = await _relationshipManagerService.RemoveBookFromUserAsync(request);

            if (!response.Success)
            {
                return BadRequest(new { response.Success, response.Error, response.ErrorCode });
            }
            return Ok(response);
        }
        [HttpPost("addbookauthor")]
        public async Task<ActionResult> AddAuthorToBookAsync(RelationshipRequest request)
        {
            var response = await _relationshipManagerService.AddAuthorToBookAsync(request);

            if (!response.Success)
            {
                return BadRequest(new { response.Success, response.Error, response.ErrorCode });
            }
            return Ok(response);
        }
        [HttpPost("removebookauthor")]
        public async Task<ActionResult> RemoveAuthorFromBookAsync(RelationshipRequest request)
        {
            var response = await _relationshipManagerService.RemoveAuthorFromBookAsync(request);

            if (!response.Success)
            {
                return BadRequest(new { response.Success, response.Error, response.ErrorCode });
            }
            return Ok(response);
        }
        [HttpPost("addbookcategory")]
        public async Task<ActionResult> AddCategoryToBookAsync(RelationshipRequest request)
        {
            var response = await _relationshipManagerService.AddCategoryToBookAsync(request);

            if (!response.Success)
            {
                return BadRequest(new { response.Success, response.Error, response.ErrorCode });
            }
            return Ok(response);
        }
        [HttpPost("removebookcategory")]
        public async Task<ActionResult> RemoveCategoryFromBookAsync(RelationshipRequest request)
        {
            var response = await _relationshipManagerService.RemoveCategoryFromBookAsync(request);

            if (!response.Success)
            {
                return BadRequest(new { response.Success, response.Error, response.ErrorCode });
            }
            return Ok(response);
        }
    }
}
