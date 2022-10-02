using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VideoJatekBackend.Dto;
using VideoJatekBackend.Services.PublisherFeature.Commands;
using VideoJatekBackend.Services.PublisherFeature.Queries;

namespace VideoJatekBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IMediator mediator;

        public PublisherController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet]
        public async Task<List<PublisherDto>> GetAllPublisher()
        {
            var query = new GetAllPublishers.Query();
            return await mediator.Send(query);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<PublisherDto> GetPublisherById(int id)
        {
            var query = new GetPublisherById.Query()
            {
                Id = id
            };
            return await mediator.Send(query);
        }

        [HttpPost]
        public async Task<PublisherDto> AddPublisher([FromBody] PublisherDto publisherDto)
        {
            var command = new AddPublisher.Query()
            {
                Publisher = publisherDto
            };
            return await mediator.Send(command);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<PublisherDto> ModifyPublisher(int id, [FromBody] PublisherDto publisherDto)
        {
            publisherDto.Id = id;
            var command = new ModifyPublisher.Query()
            {
                Publisher = publisherDto
            };
            return await mediator.Send(command);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<bool> DeletePublisher(int id)
        {
            var command = new RemovePublisher.Query()
            {
                Id = id
            };
            return await mediator.Send(command);
        }
    }
}
