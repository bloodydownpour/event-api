using System.Runtime.CompilerServices;

namespace WebApplication1.Structure.Database
{
    public class UnitOfWork(EventDbContext context, EventRepos eRepos, UserRepos uRepos) : IDisposable
    {

        private readonly EventDbContext context = context;
        private readonly EventRepos eventRepos = eRepos;
        private readonly UserRepos userRepos = uRepos;

        public EventRepos Events
        {
            get
            {
                return eventRepos;
            }
        }
        public UserRepos Users
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
}
