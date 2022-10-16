using MVCIdentityBookRecords.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVCIdentityBookRecords.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RelationshipController : ControllerBase
    {
        private readonly IEntityRelationShipManagerService _relationshipManagerService;
        public RelationshipController(IEntityRelationShipManagerService relationShipManagerService)
        {
            _relationshipManagerService = relationShipManagerService;
        }

        [HttpPost("/addbookuser")]
        public async Task<ActionResult> AddBookToUserAsync(string Iduser, int Idbook)
        {
            var response = await _relationshipManagerService.AddBookToUserAsync(Iduser, Idbook);

            if (!response.Success)
            {
                return BadRequest(new {Success = response.Success, Error = response.Error, ErrorCode = response.ErrorCode });
            }
            return Ok(response);
        }
        [HttpPost("/removebookuser")]
        public async Task<ActionResult> RemoveBookFromUserAsync(string Iduser, int Idbook)
        {
            var response = await _relationshipManagerService.RemoveBookFromUserAsync(Iduser, Idbook);

            if (!response.Success)
            {
                return BadRequest(new { Success = response.Success, Error = response.Error, ErrorCode = response.ErrorCode });
            }
            return Ok(response);
        }
        [HttpPost("/addbookauthor")]
        public async Task<ActionResult> AddAuthorToBookAsync(int Idbook, int Idauthor)
        {
            var response = await _relationshipManagerService.AddAuthorToBookAsync(Idbook, Idauthor);

            if (!response.Success)
            {
                return BadRequest(new { Success = response.Success, Error = response.Error, ErrorCode = response.ErrorCode });
            }
            return Ok(response);
        }
        [HttpPost("/removebookauthor")]
        public async Task<ActionResult> RemoveAuthorFromBookAsync(int Idbook, int Idauthor)
        {
            var response = await _relationshipManagerService.RemoveAuthorFromBookAsync(Idbook, Idauthor);

            if (!response.Success)
            {
                return BadRequest(new { Success = response.Success, Error = response.Error, ErrorCode = response.ErrorCode });
            }
            return Ok(response);
        }
        [HttpPost("/addbookcategory")]
        public async Task<ActionResult> AddCategoryToBookAsync(int Idbook, int Idcategory)
        {
            var response = await _relationshipManagerService.AddCategoryToBookAsync(Idbook, Idcategory);

            if (!response.Success)
            {
                return BadRequest(new { Success = response.Success, Error = response.Error, ErrorCode = response.ErrorCode });
            }
            return Ok(response);
        }
        [HttpPost("/removebookcategory")]
        public async Task<ActionResult> RemoveCategoryFromBookAsync(int Idbook, int Idcategory)
        {
            var response = await _relationshipManagerService.RemoveCategoryFromBookAsync(Idbook, Idcategory);

            if (!response.Success)
            {
                return BadRequest(new { Success = response.Success, Error = response.Error, ErrorCode = response.ErrorCode });
            }
            return Ok(response);
        }
    }
}
