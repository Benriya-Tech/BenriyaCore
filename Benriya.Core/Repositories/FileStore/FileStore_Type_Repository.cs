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
using Benriya.Utils;
using Benriya.Share.Models.FileStore;
using Benriya.Core.Abstractions.FileStore;

namespace Benriya.Core.Repositories.FileStore
{
    public class FileStore_Type_Repository : RepositoryBase<FileStore_FileType>, IFileStore_Type_Repository
    {

        public async Task<bool> SetupFIleType()
        {
            var file_types = new List<FileStore_FileType>();
            // Images
            file_types.Add(new FileStore_FileType() {file_extension=".png",name="Image PNG",file_type = File_Types.Image });
            file_types.Add(new FileStore_FileType() {file_extension=".jpg",name="Image JPG",file_type = File_Types.Image });
            file_types.Add(new FileStore_FileType() {file_extension=".gif",name="Image GIF",file_type = File_Types.Image });
            file_types.Add(new FileStore_FileType() {file_extension=".bmp",name="Image BMP",file_type = File_Types.Image });

            // Documents
            file_types.Add(new FileStore_FileType() { file_extension = ".doc", name = "Mocrosoft Word", file_type = File_Types.Document });
            file_types.Add(new FileStore_FileType() { file_extension = ".docx", name = "Mocrosoft Word", file_type = File_Types.Document });
            file_types.Add(new FileStore_FileType() { file_extension = ".xls", name = "Mocrosoft Excel", file_type = File_Types.Document });
            file_types.Add(new FileStore_FileType() { file_extension = ".xlsx", name = "Mocrosoft Excel", file_type = File_Types.Document });
            file_types.Add(new FileStore_FileType() { file_extension = ".ptt", name = "Mocrosoft Power point", file_type = File_Types.Document });
            file_types.Add(new FileStore_FileType() { file_extension = ".pttx", name = "Mocrosoft Power point", file_type = File_Types.Document });
            file_types.Add(new FileStore_FileType() { file_extension = ".pdf", name = "PDF documents", file_type = File_Types.Document });

            // Files
            file_types.Add(new FileStore_FileType() { file_extension = ".json", name = "JSON data", file_type = File_Types.File });
            file_types.Add(new FileStore_FileType() { file_extension = ".txt", name = "Text file", file_type = File_Types.File });
            file_types.Add(new FileStore_FileType() { file_extension = ".csv", name = "CSV data", file_type = File_Types.File });

            await dbSet.AddRangeAsync(file_types);
            await storageContext.SaveChangesAsync();
            return true;
        }
        public async Task<FileStore_FileType> GetAsync(Expression<Func<FileStore_FileType, bool>> func)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(func);

            return data;
        }

        public async Task<List<FileStore_FileType>> ListAsync(Expression<Func<FileStore_FileType, bool>> func, int limit = 20)
        {
            var data = dbSet.Where(func).OrderBy(o => o.name);
            if (limit > 0)
                data.Take(limit);
            return await data.ToListAsync();
        }

        public async Task<FileStore_FileType> SaveChangeAsync(FileStore_FileType idata)
        {
            if (idata.id > 0)
                return await UpdateAsync(idata);
            else
                return await CreateAsync(idata);

        }
        public async Task<FileStore_FileType> CreateAsync(FileStore_FileType idata)
        {
            idata.file_extension = idata.file_extension.ToLower();
            await this.dbSet.AddAsync(idata);
            await storageContext.SaveChangesAsync();
            return idata;
        }

        public async Task<FileStore_FileType> UpdateAsync(FileStore_FileType idata)
        {
            if (idata.id > 0)
            {
                var data = await this.dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.id == idata.id);
                if (data == null)
                    throw new Exception($"Categoty id:{idata.id} is not found");

                data.name = idata.name;
                data.file_extension = idata.file_extension.ToLower();
                data.file_type = idata.file_type;
                this.dbSet.Update(data);
                await storageContext.SaveChangesAsync();
                return data;
            }
            return null;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
            if (data == null)
                throw new Exception($"Categoty id:{id} is not found");

            data.is_active = false;
            this.dbSet.Update(data);
            await storageContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
            if (data == null)
                throw new Exception($"Categoty id:{id} is not found");
            this.dbSet.Remove(data);
            return true;
        }

        public PagedList<FileStore_FileType> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var query = from data in this.dbSet
                        where data.is_active == true
                        orderby data.id descending
                        select new FileStore_FileType()
                        {
                            id = data.id,
                            name = data.name,
                            file_extension = data.file_extension,
                            file_type = data.file_type,
                            is_active = data.is_active,
                            //created = data.created,
                            //updated = data.updated
                        };
            if (condition != null)
            {
                if (!condition.txt.isNOEOW())
                {
                    query = query.Where(x =>
                    EF.Functions.Like(x.id.ToString(), $"%{condition.txt}%") ||
                    EF.Functions.Like(x.name, $"%{condition.txt}%") ||
                    EF.Functions.Like(x.file_extension, $"%{condition.txt}%")
                    );
                }
            }
            return new PagedList<FileStore_FileType>(query, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<IEnumerable<DropdownItem>> DropdownListAsync(string txt = null, int limit = 20)
        {
            var query = from data in this.dbSet
                        orderby data.name descending
                        select new DropdownItem()
                        {
                            value = data.id.ToString(),
                            id = data.id,
                            label = data.name,
                            description = data.file_extension
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

        public async Task<IEnumerable<FileStore_FileType>> ItemsAynce(string txt = null, int limit = 20)
        {
            var query = from data in this.dbSet
                        orderby data.name descending
                        select new FileStore_FileType()
                        {
                            id = data.id,
                            name = data.name,
                            file_extension = data.file_extension
                        };
            if (!txt.isNOEOW())
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.id.ToString(), $"%{txt}%") ||
                    EF.Functions.Like(x.name, $"%{txt}%") ||
                    EF.Functions.Like(x.file_extension, $"%{txt}%")
                );
            }
            if (limit > 0)
                query = query.Take(limit);
            return await query.ToListAsync();
        }
    }

}
