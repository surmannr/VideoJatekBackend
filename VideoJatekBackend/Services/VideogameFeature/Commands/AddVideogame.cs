using AutoMapper;
using FluentValidation;
using MediatR;
using VideoJatekBackend.Dal.Seed;
using VideoJatekBackend.Dal;
using VideoJatekBackend.Dto;
using VideoJatekBackend.Models;

namespace VideoJatekBackend.Services.VideogameFeature.Commands
{
    public static class AddVideogame
    {
        public class Query : ICommand<VideogameDto>
        {
            public VideogameDto Videogame { get; set; }
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
                var videogame = _mapper.Map<Videogame>(request.Videogame);

                var result = _dbContext.Videogames.Add(videogame);
                await _dbContext.SaveChangesAsync(cancellationToken);

                VideogameJsonFileProcessor.SerializeList(_dbContext.Videogames.ToList());

                return _mapper.Map<VideogameDto>(result.Entity);
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Videogame.Name).NotEmpty().NotNull().WithMessage("Az Név értéke nem lehet null.");
                RuleFor(x => x.Videogame.Developer).NotEmpty().NotNull().WithMessage("A Fejlesztő értéke nem lehet null.");
                RuleFor(x => x.Videogame.Category).NotEmpty().NotNull().WithMessage("A Kategória értéke nem lehet null.");
            }
        }
    }
}
