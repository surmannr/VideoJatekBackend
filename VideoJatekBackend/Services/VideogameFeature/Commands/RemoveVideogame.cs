using AutoMapper;
using FluentValidation;
using MediatR;
using VideoJatekBackend.Dal.Seed;
using VideoJatekBackend.Dal;
using Microsoft.EntityFrameworkCore;

namespace VideoJatekBackend.Services.VideogameFeature.Commands
{
    public static class RemoveVideogame
    {
        public class Query : ICommand<bool>
        {
            public int Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, bool>
        {
            private readonly IMapper _mapper;
            private readonly VideogameDbContext _dbContext;
            public Handler(IMapper mapper, VideogameDbContext dbContext)
            {
                _mapper = mapper;
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
            {
                var videogame = await _dbContext.Videogames
                    .FirstOrDefaultAsync(e => e.Id == request.Id);

                _dbContext.Remove(videogame);
                await _dbContext.SaveChangesAsync(cancellationToken);

                VideogameJsonFileProcessor.SerializeList(_dbContext.Videogames.ToList());

                return true;
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
