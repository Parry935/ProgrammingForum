using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Interfaces.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        ITopicRepository Topic { get; }
        IPostRepository Post { get; }
        IUserRepository User { get; }

        Task SaveAsync();
    }
}
