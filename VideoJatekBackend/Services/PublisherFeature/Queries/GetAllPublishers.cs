using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VideoJatekBackend.Dal;
using VideoJatekBackend.Dto;
using VideoJatekBackend.Models;

namespace VideoJatekBackend.Services.PublisherFeature.Queries
{
    public static class GetAllPublishers
    {
        public class Query : IRequest<List<PublisherDto>>
        {

        }

        public class Handler : IRequestHandler<Query, List<PublisherDto>>
        {
            private readonly IMapper _mapper;
            private readonly VideogameDbContext _dbContext;
            public Handler(IMapper mapper, VideogameDbContext dbContext)
            {
                _mapper = mapper;
                _dbContext = dbContext;
            }

            public async Task<List<PublisherDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var publishers = await _dbContext.Publishers
                    .Include(e => e.Videogames)
                    .ProjectTo<PublisherDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return publishers;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {

            }
        }
    }
}
