using EventList.Domain.Interfaces;

namespace EventList.Infrastructure.Database;

public class UnitOfWork(EventDbContext context, IEventRepository eRepos, IUserRepository uRepos) : IDisposable, IUnitOfWork
{

    private readonly EventDbContext context = context;
    private readonly IEventRepository eventRepos = eRepos;
    private readonly IUserRepository userRepos = uRepos;

    public IEventRepository Events
    {
        get
        {
            return eventRepos;
        }
    }
    public IUserRepository Users
    {
        get
        {
            return userRepos;
        }
    }
    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
    private bool isDisposed = false;
    public virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
            isDisposed = true;
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

}
