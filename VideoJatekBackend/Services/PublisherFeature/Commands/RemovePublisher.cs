using AutoMapper;
using FluentValidation;
using MediatR;
using VideoJatekBackend.Dal.Seed;
using VideoJatekBackend.Dal;
using VideoJatekBackend.Dto;
using Microsoft.EntityFrameworkCore;

namespace VideoJatekBackend.Services.PublisherFeature.Commands
{
    public static class RemovePublisher
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
                var publisher = await _dbContext.Publishers
                    .FirstOrDefaultAsync(e => e.Id == request.Id);

                _dbContext.Remove(publisher);
                await _dbContext.SaveChangesAsync(cancellationToken);

                PublisherJsonFileProcessor.SerializeList(_dbContext.Publishers.ToList());

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
