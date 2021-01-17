using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Benriya.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Benriya.Utils.Pagingation;
using Benriya.Utils.Models;
using Benriya.Core.Abstractions.FileStore;
using Benriya.Share.Models.FileStore;
using Benriya.Utils;

namespace Benriya.Core.Repositories.FileStore
{
    public class FileStore_Images_Repository : RepositoryBase<FileStore_Images>, IFileStore_Images_Repository
    {
        private FileStore_Images SetClient(FileStore_Images idata, bool is_new = false)
        {
            if (is_new)
            {
                idata.created_by = Client.CurrentUser.id;
                idata.created = DateTime.Now;
            }
            else
            {
                idata.updated = DateTime.Now;
                idata.updated_by = Client.CurrentUser.id;
            }
            if (idata.FileType == null || idata.file_type_id < 1)
            {
                var file_type_context = storageContext.Set<FileStore_FileType>();
                idata.file_type_id = file_type_context.FirstOrDefault(x => x.file_type == File_Types.Image).id;
            }
            return idata;
        }

        public async Task<FileStore_Images> GetAsync(Expression<Func<FileStore_Images, bool>> func)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(func);

            return data;
        }

        public async Task<List<FileStore_Images>> ListAsync(Expression<Func<FileStore_Images, bool>> func, int limit = 20)
        {
            var data = dbSet.Where(func).OrderBy(o => o.name);
            if (limit > 0)
                data.Take(limit);
            return await data.ToListAsync();
        }

        public async Task<FileStore_Images> SaveChangeAsync(FileStore_Images idata)
        {
            if (idata.id != Guid.Empty)
                return await UpdateAsync(idata);
            else
                return await CreateAsync(idata);

        }
        public async Task<FileStore_Images> CreateAsync(FileStore_Images idata)
        {
            idata = SetClient(idata, true);
            await this.dbSet.AddAsync(idata);
            await storageContext.SaveChangesAsync();
            return idata;
        }

        public async Task<FileStore_Images> UpdateAsync(FileStore_Images idata)
        {
            if (idata.id != Guid.Empty)
            {
                var data = await this.dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.id == idata.id);
                if (data == null)
                    throw new Exception($"Categoty id:{idata.id} is not found");
                idata = SetClient(idata);
                data.name = idata.name;
                data.module = idata.module;
                data.file_type_id = idata.file_type_id;
                this.dbSet.Update(data);
                await storageContext.SaveChangesAsync();
                return data;
            }
            return null;
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
            if (data == null)
                throw new Exception($"Categoty id:{id} is not found");
            data = SetClient(data);
            data.is_active = false;
            this.dbSet.Update(data);
            await storageContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
            if (data == null)
                throw new Exception($"Categoty id:{id} is not found");
            this.dbSet.Remove(data);
            return true;
        }

        public PagedList<FileStore_Images> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var query = from data in this.dbSet
                        where data.is_active == true
                        orderby data.id descending
                        select new FileStore_Images()
                        {
                            id = data.id,
                            name = data.name,
                            file_type_id = data.file_type_id,
                            FileType = data.FileType,
                            is_active = data.is_active,
                            created = data.created,
                            updated = data.updated
                        };
            if (condition != null)
            {
                if (!condition.txt.isNOEOW())
                {
                    query = query.Where(x =>
                    EF.Functions.Like(x.id.ToString(), $"%{condition.txt}%") ||
                    EF.Functions.Like(x.name, $"%{condition.txt}%") //||
                    //EF.Functions.Like(x.file_type, $"%{condition.txt}%")
                    );
                }
            }
            return new PagedList<FileStore_Images>(query, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20)
        {
            var query = from data in this.dbSet
                        orderby data.name descending
                        select new DropdownItem()
                        {
                            value = data.id.ToString(),
                            uid = data.id,
                            label = data.name,
                            description = data.FileType.file_extension
                        };
            if (!txt.isNOEOW())
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.value, $"%{txt}%") ||
                    EF.Functions.Like(x.label, $"%{txt}%") ||
                    EF.Functions.Like(x.description, $"%{txt}%")

                );
            }
            if (limit > 0)
                query = query.Take(limit);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<FileStore_Images>> ItemsAynce(string txt = null, int limit = 20)
        {
            var query = from data in this.dbSet
                        orderby data.name descending
                        select new FileStore_Images()
                        {
                            id = data.id,
                            name = data.name
                        };
            if (!txt.isNOEOW())
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.id.ToString(), $"%{txt}%") ||
                    EF.Functions.Like(x.name, $"%{txt}%")
                //EF.Functions.Like(x.description, $"%{txt}%")
                );
            }
            if (limit > 0)
                query = query.Take(limit);
            return await query.ToListAsync();
        }
    }

}
