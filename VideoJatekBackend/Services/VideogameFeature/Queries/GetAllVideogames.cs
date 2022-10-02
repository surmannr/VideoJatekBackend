using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VideoJatekBackend.Dal;
using VideoJatekBackend.Dto;

namespace VideoJatekBackend.Services.VideogameFeature.Queries
{
    public static class GetAllVideogames
    {
        public class Query : IRequest<List<VideogameDto>>
        {

        }

        public class Handler : IRequestHandler<Query, List<VideogameDto>>
        {
            private readonly IMapper _mapper;
            private readonly VideogameDbContext _dbContext;
            public Handler(IMapper mapper, VideogameDbContext dbContext)
            {
                _mapper = mapper;
                _dbContext = dbContext;
            }

            public async Task<List<VideogameDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var videogames = await _dbContext.Videogames
                    .ProjectTo<VideogameDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return videogames;
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
