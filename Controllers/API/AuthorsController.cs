using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCIdentityBookRecords.Data;
using MVCIdentityBookRecords.Models;
using MVCIdentityBookRecords.Interfaces;
using MVCIdentityBookRecords.Responses.Authors;
using Microsoft.AspNetCore.Authorization;

namespace MVCIdentityBookRecords.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Basic,Admin")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AuthorsController : BaseApiController
    {
        
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var getAuthorsResponse = await _authorService.GetAuthorsAsync();
            if (!getAuthorsResponse.Success)
            {
                return UnprocessableEntity(getAuthorsResponse);
            }
            var authorsResponse = getAuthorsResponse.Authors.ConvertAll(o => new AuthorResponse {
                AuthorId = o.AuthorId,
                Firstname = o.Firstname, 
                Lastname = o.Lastname
            });

            return Ok(authorsResponse);
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);

            if (!author.Success)
            {
                return UnprocessableEntity(author);
            }

            return Ok(new AuthorResponse
            {
                AuthorId = author.AuthorId,
                Firstname=author.Firstname,
                Lastname=author.Lastname,
                Books = author.Books
            });

        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if (id != author.AuthorId)
            {
                return BadRequest();
            }

            var authorResponse = await _authorService.UpdateAuthorAsync(id, author);

            if (!authorResponse.Success)
            {
                return BadRequest(author);
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            var authorResponse = await _authorService.CreateAuthorAsync(author);

            if (!authorResponse.Success)
            {
                return UnprocessableEntity(author);
            }

            return CreatedAtAction("GetAuthor", new { id = author.AuthorId }, author);
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {

            var authorResponse = await _authorService.DeleteAuthorAsync(id);
            if (!authorResponse.Success)
            {
                return BadRequest();
            }

            return NoContent();
        }

    }
}
