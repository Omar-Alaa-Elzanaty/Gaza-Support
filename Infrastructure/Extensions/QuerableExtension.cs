using Gaza_Support.Domains.Dtos.ResponseDtos;
using MongoDB.Driver.Linq;

namespace Infrastructure.Extensions
{
    public static class QuerableExtension
    {
        public static async Task<PaginatedResponse<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;
            int count = await source.CountAsync();
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return PaginatedResponse<T>.Create(items, count, pageNumber, pageSize);
        }
    }
}
