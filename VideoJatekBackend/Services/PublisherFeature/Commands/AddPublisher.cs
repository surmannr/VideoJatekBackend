using AutoMapper;
using FluentValidation;
using MediatR;
using VideoJatekBackend.Dal;
using VideoJatekBackend.Dal.Seed;
using VideoJatekBackend.Dto;
using VideoJatekBackend.Models;

namespace VideoJatekBackend.Services.PublisherFeature.Commands
{
    public static class AddPublisher
    {
        public class Query : ICommand<PublisherDto>
        {
            public PublisherDto Publisher { get; set; }
        }

        public class Handler : IRequestHandler<Query, PublisherDto>
        {
            private readonly IMapper _mapper;
            private readonly VideogameDbContext _dbContext;
            public Handler(IMapper mapper, VideogameDbContext dbContext)
            {
                _mapper = mapper;
                _dbContext = dbContext;
            }

            public async Task<PublisherDto> Handle(Query request, CancellationToken cancellationToken)
            {
                var publisher = _mapper.Map<Publisher>(request.Publisher);

                var result = _dbContext.Publishers.Add(publisher);
                await _dbContext.SaveChangesAsync(cancellationToken);

                PublisherJsonFileProcessor.SerializeList(_dbContext.Publishers.ToList());

                return _mapper.Map<PublisherDto>(result.Entity);
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Publisher.Name).NotEmpty().NotNull().WithMessage("Az Név értéke nem lehet null.");
                RuleFor(x => x.Publisher.Address).NotEmpty().NotNull().WithMessage("Az Cím értéke nem lehet null.");
                RuleFor(x => x.Publisher.FoundationDate).NotEmpty().NotNull().WithMessage("Az Alapítás dátum értéke nem lehet null.");
            }
        }
    }
}
