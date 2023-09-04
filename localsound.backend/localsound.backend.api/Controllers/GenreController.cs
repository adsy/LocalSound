using localsound.backend.api.Queries.Genre;
using localsound.backend.Domain.Model.Dto.Entity;
using Microsoft.AspNetCore.Mvc;

namespace localsound.backend.api.Controllers
{
    [Route("api/genre")]
    [ApiController]
    public class GenreController : BaseApiController
    {
        [HttpGet]
        [Route("search-genre/{name}")]
        public async Task<ActionResult<List<GenreDto>>> SearchGenreTypes(string name, CancellationToken token)
        {
            var result = await Mediator.Send(new SearchGenreQuery
            {
                Name = name
            }, token);

            if (result.IsSuccessStatusCode)
            {
                return Ok(result.ReturnData);
            }

            return StatusCode((int)result.StatusCode);
        }
    }
}
