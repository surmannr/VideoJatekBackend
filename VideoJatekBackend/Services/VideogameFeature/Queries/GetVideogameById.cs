using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VideoJatekBackend.Dal;
using VideoJatekBackend.Dto;

namespace VideoJatekBackend.Services.VideogameFeature.Queries
{
    public static class GetVideogameById
    {
        public class Query : IRequest<VideogameDto>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, VideogameDto>
        {
            private readonly IMapper _mapper;
            private readonly VideogameDbContext _dbContext;
            public Handler(IMapper mapper, VideogameDbContext dbContext)
            {
                _mapper = mapper;
                _dbContext = dbContext;
            }

            public async Task<VideogameDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var videogame = await _dbContext.Videogames
                    .Where(p => p.Id == request.Id)
                    .ProjectTo<VideogameDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                return videogame;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Az Id értéke nem lehet null.");
            }
        }
    }
}
