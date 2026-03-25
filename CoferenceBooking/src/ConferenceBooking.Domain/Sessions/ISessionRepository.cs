using System;
using System.Threading.Tasks;
using System.Threading;
using Volo.Abp.Domain.Repositories;

namespace ConferenceBooking.Domain.Sessions
{
    public interface ISessionRepository : IRepository<Session, Guid>
    {
        Task<Session> FindByNameAsync(string title, CancellationToken cancellationToken = default);
    }
}