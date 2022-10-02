using MediatR;
using VideoJatekBackend.Dal;

namespace VideoJatekBackend.Services
{
    public class TransactionBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
        where TRequest : ICommand<TResult>
    {
        private readonly VideogameDbContext _dbContext;

        public TransactionBehavior(VideogameDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
        {
            using var tran = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var result = await next();
                await tran.CommitAsync();
                return result;
            }
            catch (System.Exception)
            {
                await tran.RollbackAsync();
                throw;
            }
        }
    }
}
