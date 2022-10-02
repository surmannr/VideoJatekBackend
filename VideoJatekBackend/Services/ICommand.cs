using MediatR;

namespace VideoJatekBackend.Services
{
    public interface ICommand<TResult> : IRequest<TResult>
    {
    }
}
