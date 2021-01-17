using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Benriya.Utils.Extensions;
using Benriya.Modules.CMS.Entities.Abstractions;
using Benriya.Modules.CMS.Share.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Benriya.Utils.Pagingation;
using Benriya.Utils.Models;
using Benriya.Share.Models.CoreTags;
using Benriya.Share.Models;

namespace Benriya.Modules.CMS.Entities.Repositories
{
    public class Category_Repository : RepositoryBase<Category>, ICategory_Repository
    {
        private Category SetClient(Category idata,bool is_new = false)
        {
            if (is_new)
            {
                idata.created_by = Client.CurrentUser.id;
                idata.created_ip = Client.Info.ipAddress;
            }
            else
            {
                idata.updated = DateTime.Now;
                idata.updated_by = Client.CurrentUser.id;
                idata.updated_ip = Client.Info.ipAddress;
            }
            return idata;
        }
        private Category_Tags SetClient_Tag(Category_Tags idata, bool is_new = false)
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
            return idata;
        }

        public async Task<Category> GetAsync(Expression<Func<Category, bool>> func)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(func);
            if (data != null)
            {
                var tags_context = this.storageContext.Set<Category_Tags>();
                var tags = await tags_context.Where(x=>x.category_id == data.id).Include(x=>x.Tags).Select(tag => new ItemUuidValue
                {
                    id = tag.id,
                    ref_id = tag.tag_id,
                    name = tag.Tags.name
                }).ToListAsync();
                data.Tags = tags;
            }
            return data;
        }

        public async Task<List<Category>> ListAsync(Expression<Func<Category, bool>> func, int limit = 20)
        {
            var data = dbSet.Where(func).OrderBy(o => o.name);
            if (limit > 0)
                data.Take(limit);
            return await data.ToListAsync();
        }

        public async Task<List<Category>> ListAsync_Contents(Expression<Func<Category, bool>> func, int limit = 20)
        {
            return await this.dbSet.Include(x => x.Contents).Where(func).OrderByDescending(o => o.updated).Take(limit).ToListAsync();
        }

        public async Task<Category> SaveChangeAsync(Category idata)
        {
            if (idata.id > 0)            
                return await UpdateAsync(idata);            
            else            
                return await CreateAsync(idata);            

        }
        public async Task<Category> CreateAsync(Category idata)
        {     
            idata = SetClient(idata,true);
            if(idata.Tags != null)
            {
                idata.Category_Tags = new List<Category_Tags>();
                foreach(var tag in idata.Tags)
                {
                    var new_tag = new Category_Tags()
                    {
                        tag_id = tag.ref_id
                    };
                    new_tag = SetClient_Tag(new_tag,true);
                    idata.Category_Tags.Add(new_tag);
                }
            }
            await this.dbSet.AddAsync(idata);
            await storageContext.SaveChangesAsync();
            return idata;
        }

        public async Task<Category> UpdateAsync(Category idata)
        {
            if (idata.id > 0)
            {
                var data = await this.dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.id == idata.id);
                if (data == null)
                    throw new Exception($"Categoty id:{idata.id} is not found");
                idata = SetClient(idata);
                data.name = idata.name;
                data.description = idata.description;
                
                var tags_context = this.storageContext.Set<Category_Tags>();
                var tags = await tags_context.Where(x => x.category_id == idata.id).AsNoTracking().ToListAsync();
                if (idata.Tags != null)
                {
                    if (data.Category_Tags == null)
                        data.Category_Tags = new List<Category_Tags>();
                    foreach (var tag in idata.Tags)
                    {
                        var tag_indb = tags.FirstOrDefault(x => x.id == tag.id);
                        if (tag_indb == null)
                        {
                            var new_tag = new Category_Tags()
                            {
                                category_id = idata.id,
                                tag_id = tag.ref_id,
                                Category = null,
                                Tags = null
                            };
                            new_tag = SetClient_Tag(new_tag, true);
                            await tags_context.AddAsync(new_tag);
                        }
                        else
                        {
                            tag_indb = SetClient_Tag(tag_indb);
                            tags_context.Update(tag_indb);
                        }
                    }
                    var ids = idata.Tags.Select(x => x.id).ToArray();
                    var remove_tags = tags.Where(x => !ids.Contains(x.id)).ToList();
                    if (remove_tags.Count() > 0)
                        tags_context.RemoveRange(remove_tags);
                }
                else if (tags.Count() > 0)
                {
                    tags_context.RemoveRange(tags);
                }
                idata = null;
                tags = null;
                data.Tags = null;
                data.Category_Tags = null;
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
            data = SetClient(data);
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

        public PagedList<Category> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var query = from data in this.dbSet
                        where data.is_active == true
                        orderby data.id descending
                        select new Category()
                        {
                            id = data.id,
                            name = data.name,
                            description = data.description,
                            created = data.created,
                            updated = data.updated
                        };
            if (condition != null)
            {
                if (!condition.txt.isNOEOW())
                {
                    query = query.Where(x => 
                    EF.Functions.Like(x.id.ToString(), $"%{condition.txt}%") || 
                    EF.Functions.Like(x.name, $"%{condition.txt}%") || 
                    EF.Functions.Like(x.description, $"%{condition.txt}%")
                    );
                }
            }
            return new PagedList<Category>(query, pagingParams.PageNumber, pagingParams.PageSize);
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
                    description = data.description
                };
            if (!txt.isNOEOW()) {
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

        public async Task<IEnumerable<Category>> ItemsAynce(string txt = null, int limit = 20)
        {
            var query = from data in this.dbSet
                        orderby data.name descending
                        select new Category()
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

        public async Task<IEnumerable<DropdownItem>> TagsDropdownAsync(int id,string txt = null,int limit = 20)
        {
            var tags_context = this.storageContext.Set<Tags>();
            var cate_tags_context = this.storageContext.Set<Category_Tags>();
            var query = from data in cate_tags_context
                        join tags in tags_context on data.tag_id equals tags.id     
                        where data.category_id == id
                        orderby tags.name ascending
                        select new DropdownItem()
                        {
                            value = data.id.ToString(),
                            label = tags.name,
                            uid = data.tag_id,
                            //ref_code = data.id.ToString(),
                            //id = data.category_id,
                            description = tags.description
                        };
            //if (id > 0)
            //    query = query.Where(x=>x.id == id);
            if (!txt.isNOEOW())
            {
                query = query.Where(x =>
                    //EF.Functions.Like(x.value.ToString(), $"%{txt}%") ||
                    EF.Functions.Like(x.label, $"%{txt}%") ||
                    EF.Functions.Like(x.description, $"%{txt}%")
                );
            }
            if (limit > 0)
                query = query.Take(limit);
            return await query.ToListAsync();
        }
    }

}
