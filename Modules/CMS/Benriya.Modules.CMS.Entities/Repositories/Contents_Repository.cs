using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Benriya.Utils.Extensions;
using Benriya.Modules.CMS.Entities.Abstractions;
using Benriya.Modules.CMS.Share.Models;
using Benriya.Modules.CMS.Share.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Benriya.Utils.Models;
using Benriya.Utils.Pagingation;
using Benriya.Utils;
using Benriya.Share.Models;

namespace Benriya.Modules.CMS.Entities.Repositories
{
    public class Contents_Repository : RepositoryBase<Contents>, IContents_Repository
    {

        private Contents SetIniData(Contents idata, bool is_new = false)
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

            if(idata.InCategory != null && !idata.InCategory.value.isNOEOW() && idata.InCategory.value.isNumberic())
            {              
                idata.category_id = idata.InCategory.value.ToInterger();
                idata.Category = null;
            }
            if (idata.Category != null && idata.Category.id > 0)
            {                
                idata.category_id = idata.Category.id;
                idata.Category = null;
            }

            if (idata.category_id < 1)
                throw new ArgumentException("Category is required");

            return idata;
        }
        private Content_Tags SetClient_Tag(Content_Tags idata, bool is_new = false)
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

        public async Task<Contents> GetAsync(Expression<Func<Contents, bool>> func)
        {
            var data =  await this.dbSet.Include(x=>x.Category).FirstOrDefaultAsync(func);
            if (data != null)
            {
                var tags_context = this.storageContext.Set<Content_Tags>();
                var tags = await tags_context.Where(x => x.content_id == data.id).Include(x => x.Tags).Select(tag => new ItemUuidValue
                {
                    id = tag.id,
                    ref_id = tag.tag_id,
                    name = tag.Tags.name
                }).ToListAsync();
                data.Tags = tags;
            }
            return data;
        }

        public async Task<List<Contents>> ListAsync(Expression<Func<Contents, bool>> func, int limit = 20)
        {
            var data = dbSet.Where(func).OrderBy(o => o.name);
            if (limit > 0)
                data.Take(limit);
            return await data.ToListAsync();
        }

        private async Task<string> GenPathAsync()
        {
            string code = RNG.RandomString();
            bool check = true;
            string data = null;
            do
            {
                data = await dbSet.Select(x=>x.path).FirstOrDefaultAsync(x => x == code);
                if (data == null)
                    check = false;
                else
                    code = RNG.RandomString();
            } while (check);
            return code;

        }

        private async Task<Contents> CheckData(Contents idata)
        {
            if (idata.path.isNOEOW())
                idata.path = await this.GenPathAsync();
            else
            {
                idata.path = idata.path.Trim();
                var chk = await dbSet.Select(x => new { path = x.path,id = x.id }).FirstOrDefaultAsync(x => x.path == idata.path);
                if (chk != null)
                {
                    if (idata.id != Guid.Empty && chk.id == idata.id)
                        return idata;
                    throw new DuplicateNameException($"Path: [{idata.path}] is exist");
                }
                chk = null;
            }
            return idata;
        }

        public async Task<Contents> SaveChangeAsync(Contents idata)
        {
            if (idata.id != Guid.Empty)            
                return await UpdateAsync(idata);            
            else            
                return await CreateAsync(idata);            

        }

        public async Task<Contents> CreateAsync(Contents idata)
        {
            //try
            //{
                idata = await this.CheckData(idata);
                idata = SetIniData(idata, true);
                idata.Category = null;
                if (idata.Tags != null)
                {
                    idata.Content_Tags = new List<Content_Tags>();
                    foreach (var tag in idata.Tags)
                    {
                        var new_tag = new Content_Tags()
                        {
                            tag_id = tag.ref_id,
                            Content = idata
                        };
                        new_tag = SetClient_Tag(new_tag, true);
                        idata.Content_Tags.Add(new_tag);
                    }
                }
                await this.dbSet.AddAsync(idata);
                await storageContext.SaveChangesAsync();
                return idata;
            //}catch(Exception)
            //{
            //    throw;
            //}

        }

        public async Task<Contents> UpdateAsync(Contents idata)
        {
            if (idata.id != Guid.Empty)
            {
                var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == idata.id);
                if (data == null)
                    throw new Exception($"Categoty id:{idata.id} is not found");
                idata = await this.CheckData(idata);
                data = SetIniData(data);
                data.category_id = idata.category_id;
                if(idata.Category.id > 0)
                    data.category_id = idata.Category.id;
                data.name = idata.name;
                data.path = idata.path;
                data.body = idata.body;
                data.description = idata.description;

                var tags_context = this.storageContext.Set<Content_Tags>();
                var tags = await tags_context.Where(x => x.content_id == idata.id).AsNoTracking().ToListAsync();
                if (idata.Tags != null)
                {
                    if (data.Content_Tags == null)
                        data.Content_Tags = new List<Content_Tags>();
                    foreach (var tag in idata.Tags)
                    {
                        var tag_indb = tags.FirstOrDefault(x => x.id == tag.id);
                        if (tag_indb == null)
                        {
                            var new_tag = new Content_Tags()
                            {
                                content_id = idata.id,
                                tag_id = tag.ref_id,
                                Content = null,
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

                this.dbSet.Update(data);
                await storageContext.SaveChangesAsync();
                data.Category = idata.Category;
                return data;
            }
            return null;

        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
            if (data == null)
                throw new Exception($"Categoty id:{id} is not found");
            data = SetIniData(data);
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

        public PagedList<Content_ViewModel> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var cate_db = this.storageContext.Set<Category>();
            var query = from data in this.dbSet
                     join cate in cate_db on data.category_id equals cate.id
                     where data.is_active == true && cate.is_active == true
                     select new Content_ViewModel
                     {
                         id = data.id,
                         name = data.name,
                         description = data.description,
                         category_id = cate.id,
                         category_name = cate.name,
                         likes = data.Content_Likes.Count(),
                         created = data.created,
                         updated = data.updated,
                         created_by = data.created_by,
                         updated_by = data.updated_by
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
            return new PagedList<Content_ViewModel>(query, pagingParams.PageNumber, pagingParams.PageSize);
        }

    }

}
