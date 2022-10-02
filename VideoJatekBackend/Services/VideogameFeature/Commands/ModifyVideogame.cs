using AutoMapper;
using FluentValidation;
using MediatR;
using VideoJatekBackend.Dal.Seed;
using VideoJatekBackend.Dal;
using VideoJatekBackend.Dto;
using Microsoft.EntityFrameworkCore;

namespace VideoJatekBackend.Services.VideogameFeature.Commands
{
    public static class ModifyVideogame
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
                var videogame = await _dbContext.Videogames
                    .FirstOrDefaultAsync(e => e.Id == request.Videogame.Id);
                videogame.Name = request.Videogame.Name;
                videogame.Developer = request.Videogame.Developer;
                videogame.Category = request.Videogame.Category;
                videogame.PublisherId = request.Videogame.PublisherId;

                await _dbContext.SaveChangesAsync(cancellationToken);

                VideogameJsonFileProcessor.SerializeList(_dbContext.Videogames.ToList());

                return _mapper.Map<VideogameDto>(videogame);
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Videogame.Id).NotEmpty().NotNull().WithMessage("Az Id értéke nem lehet null.");
                RuleFor(x => x.Videogame.Name).NotEmpty().NotNull().WithMessage("A Név értéke nem lehet null.");
                RuleFor(x => x.Videogame.Developer).NotEmpty().NotNull().WithMessage("A Fejlesztő értéke nem lehet null.");
                RuleFor(x => x.Videogame.Category).NotEmpty().NotNull().WithMessage("A Kategória értéke nem lehet null.");
            }
        }
    }
}
