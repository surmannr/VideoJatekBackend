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
    public static class GetPublisherById
    {
        public class Query : IRequest<PublisherDto>
        {
            public int Id { get; set; }
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
                    .Where(p => p.Id == request.Id)
                    .Include(e => e.Videogames)
                    .ProjectTo<PublisherDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                return publisher;
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
