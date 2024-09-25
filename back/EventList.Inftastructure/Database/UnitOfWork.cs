namespace EventList.Infrastructure.Database;

public class UnitOfWork(EventDbContext context, EventRepository eRepos, UserRepository uRepos) : IDisposable
{

    private readonly EventDbContext context = context;
    private readonly EventRepository eventRepos = eRepos;
    private readonly UserRepository userRepos = uRepos;

    public EventRepository Events
    {
        get
        {
            return eventRepos;
        }
    }
    public UserRepository Users
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
        if (!this.isDisposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
            this.isDisposed = true;
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

}
