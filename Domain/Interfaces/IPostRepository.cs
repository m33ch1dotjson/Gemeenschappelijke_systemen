using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPostRepository
    {
        Task<Post?> GetByTitleAsync(string title, CancellationToken ct = default);
        Task AddAsync(Post post, CancellationToken ct = default);
        Task UpdateContent(Post post, CancellationToken ct = default);

    }
}
