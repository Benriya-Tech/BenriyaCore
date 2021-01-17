using Benriya.Share.Models.FileStore;
using Benriya.Utils.Models;
using Benriya.Utils.Pagingation;
using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Benriya.Core.Abstractions.FileStore
{
    public interface IFileStore_Type_Repository : IRepository
    {
        Task<FileStore_FileType> CreateAsync(FileStore_FileType idata);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20);
        Task<FileStore_FileType> GetAsync(Expression<Func<FileStore_FileType, bool>> func);
        PagedList<FileStore_FileType> GetList(PagingParams pagingParams, SearchOptions condition = null);
        Task<IEnumerable<FileStore_FileType>> ItemsAynce(string txt = null, int limit = 20);
        Task<List<FileStore_FileType>> ListAsync(Expression<Func<FileStore_FileType, bool>> func, int limit = 20);
        Task<bool> RemoveAsync(int id);
        Task<FileStore_FileType> SaveChangeAsync(FileStore_FileType idata);
        Task<FileStore_FileType> UpdateAsync(FileStore_FileType idata);
        Task<bool> SetupFIleType();
    }
}