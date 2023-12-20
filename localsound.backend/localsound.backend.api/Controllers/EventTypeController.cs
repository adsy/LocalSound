using localsound.backend.api.Queries.EventType;
using localsound.backend.Domain.Model.Dto.Entity;
using Microsoft.AspNetCore.Mvc;

namespace localsound.backend.api.Controllers
{
    [Route("api/event-type")]
    [ApiController]
    public class EventTypeController : BaseApiController
    {
        [HttpGet]
        [Route("search-event-type/{name}")]
        public async Task<ActionResult<List<EventTypeDto>>> SearchEventTypes(string name, CancellationToken token)
        {
            var result = await Mediator.Send(new SearchEventTypeQuery
            {
                Name = name
            }, token);

            if (result.IsSuccessStatusCode)
            {
                return Ok(result.ReturnData);
            }

            return StatusCode((int)result.StatusCode);
        }

        [HttpGet]
        [Route("get-event-types")]
        public async Task<ActionResult<List<EventTypeDto>>> GetEventTypes(CancellationToken token)
        {
            var result = await Mediator.Send(new GetEventTypesQuery(), token);

            if (result.IsSuccessStatusCode)
            {
                return Ok(result.ReturnData);
            }

            return StatusCode((int)result.StatusCode);
        }
    }
}
