using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoJatekBackend.Dto;
using VideoJatekBackend.Services.PublisherFeature.Commands;
using VideoJatekBackend.Services.PublisherFeature.Queries;
using VideoJatekBackend.Services.VideogameFeature.Commands;
using VideoJatekBackend.Services.VideogameFeature.Queries;

namespace VideoJatekBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideogameController : ControllerBase
    {
        private readonly IMediator mediator;

        public VideogameController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet]
        public async Task<List<VideogameDto>> GetAllVideogame()
        {
            var query = new GetAllVideogames.Query();
            return await mediator.Send(query);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<VideogameDto> GetVideogameById(int id)
        {
            var query = new GetVideogameById.Query()
            {
                Id = id
            };
            return await mediator.Send(query);
        }

        [HttpPost]
        public async Task<VideogameDto> AddVideogame([FromBody] VideogameDto videogame)
        {
            var command = new AddVideogame.Query()
            {
                Videogame = videogame
            };
            return await mediator.Send(command);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<VideogameDto> ModifyVideogame(int id, [FromBody] VideogameDto videogame)
        {
            videogame.Id = id;
            var command = new ModifyVideogame.Query()
            {
                Videogame = videogame
            };
            return await mediator.Send(command);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<bool> DeleteVideogame(int id)
        {
            var command = new RemoveVideogame.Query()
            {
                Id = id
            };
            return await mediator.Send(command);
        }
    }
}
