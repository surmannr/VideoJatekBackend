using AutoMapper;
using FluentValidation;
using MediatR;
using VideoJatekBackend.Dal.Seed;
using VideoJatekBackend.Dal;
using VideoJatekBackend.Dto;
using VideoJatekBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace VideoJatekBackend.Services.PublisherFeature.Commands
{
    public static class ModifyPublisher
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
                var publisher = await _dbContext.Publishers
                    .FirstOrDefaultAsync(e => e.Id == request.Publisher.Id);
                publisher.Name = request.Publisher.Name;
                publisher.Address = request.Publisher.Address;
                publisher.FoundationDate = request.Publisher.FoundationDate;

                await _dbContext.SaveChangesAsync(cancellationToken);

                PublisherJsonFileProcessor.SerializeList(_dbContext.Publishers.ToList());

                return _mapper.Map<PublisherDto>(publisher);
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Publisher.Id).NotEmpty().NotNull().WithMessage("Az Id értéke nem lehet null.");
                RuleFor(x => x.Publisher.Name).NotEmpty().NotNull().WithMessage("Az Név értéke nem lehet null.");
                RuleFor(x => x.Publisher.Address).NotEmpty().NotNull().WithMessage("Az Cím értéke nem lehet null.");
                RuleFor(x => x.Publisher.FoundationDate).NotEmpty().NotNull().WithMessage("Az Alapítás dátum értéke nem lehet null.");
            }
        }
    }
}
