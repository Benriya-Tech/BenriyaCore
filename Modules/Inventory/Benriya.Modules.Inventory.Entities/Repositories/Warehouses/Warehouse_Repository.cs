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
using Benriya.Modules.Inventory.Share.Models.Warehouses;
using Benriya.Share.Models.FileStore;
using System.Data;
using Benriya.Modules.Inventory.Entities.Abstractions;

namespace Benriya.Modules.Inventory.Entities.Repositories
{
    public class Warehouse_Repository : RepositoryBase<Warehouse>, IWarehouse_Repository
    {
        private Warehouse SetClient(Warehouse idata, bool is_new = false)
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
            if (idata.is_main)
            {
                var whmain = dbSet.Where(x => x.is_main == true).ToList();
                whmain.ForEach(x => { x.is_main = false; });
                dbSet.UpdateRange(whmain);
            }
            return idata;
        }
        private FileStore_Images SetClient_ImageStore(FileStore_Images idata, bool is_new = false)
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
        public async Task<string> CheckUnique(Expression<Func<Warehouse,string>> field, string strSearch,string field_title = null)
        {
            var call = Expression.Call(field.Body, typeof(string).GetMethod("Contains"), new[] { Expression.Constant(strSearch) });
            Expression<Func<Warehouse, bool>> exp = Expression.Lambda<Func<Warehouse, bool>>(Expression.Equal(call, Expression.Constant(true)), field.Parameters);
            var data = await dbSet.Where(exp).Select(x=>new {id=x.id,name=x.name }).FirstOrDefaultAsync();
            if (data == null)
                return "ok";
            else
                return $"{strSearch}: already exist, please please enter new value";
        }
        private Warehouse_Area SetClient_Area(Warehouse_Area data, bool is_new = false,Warehouse_Area idata = null)
        {
            if (is_new)
            {
                data.created_by = Client.CurrentUser.id;
                data.created_ip = Client.Info.ipAddress;
            }
            else
            {
                data.updated = DateTime.Now;
                data.updated_by = Client.CurrentUser.id;
                data.updated_ip = Client.Info.ipAddress;
            }            
            if(idata != null)
            {
                data.name = idata.name;
                data.description = idata.description;
            }
            return data;
        }

        public async Task<Warehouse> GetAsync(Expression<Func<Warehouse, bool>> func)
        {
            var image_context = storageContext.Set<FileStore_Images>();
            var query = from data in this.dbSet                        
                        where data.is_active == true
                        orderby data.name ascending
                        select new Warehouse()
                        {
                            id = data.id,
                            name = data.name,
                            description = data.description,
                            created = data.created,
                            updated = data.updated,
                            Areas = data.Areas.Where(x => x.is_active == true).ToList()
                        };
            var xdata = await query.FirstOrDefaultAsync(func);
            xdata.Images = await image_context.Where(x => x.model_id == xdata.id && x.module == Config_FileStore_Modules.Warehouse_Image_Module && x.is_active == true).AsNoTracking().ToListAsync();
            return xdata;
        }

        public async Task<List<Warehouse>> ListAsync(Expression<Func<Warehouse, bool>> func, int limit = 20)
        {
            var data = dbSet.Where(func).OrderBy(o => o.name);
            if (limit > 0)
                data.Take(limit);
            return await data.ToListAsync();
        }

        public async Task<Warehouse> SaveChangeAsync(Warehouse idata)
        {
            if (idata.id > 0)
                return await UpdateAsync(idata);
            else
                return await CreateAsync(idata);

        }

        private async Task<bool> CheckData(Warehouse idata)
        {
            if (idata == null)
                throw new NullReferenceException("Warehouse data is required");
            var name = await dbSet.Select(x=>new { id=x.id,name=x.name}).FirstOrDefaultAsync(x=>x.id != idata.id && x.name == idata.name);
            if(name != null)
                throw new DuplicateNameException("[name]", new Exception($"Name [{idata.name}] is already used by existing objecte"));

            return true;
        }

        public async Task<Warehouse_Area> CheckArea(Warehouse_Area idata)
        {
            if (idata == null)
                throw new NullReferenceException("Area data is required");
            var area_context = storageContext.Set<Warehouse_Area>();
            var name = await area_context.Select(x => new { id = x.id, name = x.name }).FirstOrDefaultAsync(x => x.id != idata.id && x.name == idata.name);
            if (name != null)
                throw new DuplicateNameException("[name]", new Exception($"Area name [{idata.name}] is already used by existing object"));

            return idata;
        }

        public async Task<Warehouse> CreateAsync(Warehouse idata)
        {
            await CheckData(idata);
            try
            {
                await using var transaction = await storageContext.Database.BeginTransactionAsync();
                idata = SetClient(idata, true);
                List<FileStore_Images> imgs = idata.Images;
                idata.Images = null;
                await this.dbSet.AddAsync(idata);
                await storageContext.SaveChangesAsync();
                var image_context = storageContext.Set<FileStore_Images>();
                if (imgs != null && imgs.Count() > 0)
                {
                    // _mapper.Map<List<FileStore_Images>>(idata.Images.ToList());
                    imgs.ForEach(x =>
                    {
                        x = SetClient_ImageStore(x, true);
                        x.model_id = idata.id;
                        x.FileType = null;
                        x.module = Config_FileStore_Modules.Warehouse_Image_Module;
                        //x.url = Config_FileStore_Modules.Warehouse_Image_Module;
                    });
                }
                else
                {
                    imgs = await image_context.Where(x => x.model_id == idata.id && x.module == Config_FileStore_Modules.Warehouse_Image_Module).AsNoTracking().ToListAsync();
                    imgs.ForEach(x =>
                    {
                        x = SetClient_ImageStore(x, true);
                        x.model_id = idata.id;
                        x.FileType = null;
                        x.is_active = false;
                    });
                }

                image_context.UpdateRange(imgs);

                await storageContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return idata;
            }
            catch(Exception)
            {
                // var x = CheckHandleError(e);
                //throw e;
                return null;
            }
        }

        public async Task<Warehouse> UpdateAsync(Warehouse idata)
        {
            if (idata.id > 0)
            {
                var data = await this.dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.id == idata.id);
                if (data == null)
                    throw new Exception($"Categoty id:{idata.id} is not found");
                idata = SetClient(idata);
                data.name = idata.name;
                data.description = idata.description;

                var area_context = storageContext.Set<Warehouse_Area>();
                var area_list = await area_context.Where(x => x.warehouse_id == data.id).ToListAsync();
                if(idata.Areas != null && idata.Areas.Count() > 0)
                {
                    var area_ids = idata.Areas.Select(x => x.id).ToArray();
                    var area_update = area_list.Where(x => area_ids.Contains(x.id)).ToList();
                    var area_update_ids = area_update.Select(x => x.id).ToArray();
                    var area_remove = area_list.Where(x => !area_ids.Contains(x.id)).ToList();
                    var new_area_data = idata.Areas.Where(x => !area_update_ids.Contains(x.id)).ToList();

                    area_update.ForEach(x=> {
                        var area_idata = idata.Areas.FirstOrDefault(a=>a.id == x.id);
                        x.is_active = true;
                        x = SetClient_Area(x,false,area_idata);
                    });
                    area_remove.ForEach(x => {
                        x.is_active = false;
                        x = SetClient_Area(x);
                    });
                    if(area_remove.Count() > 0)
                    {
                        var update_data = area_update.Concat(area_remove).ToList();
                        area_context.UpdateRange(update_data);
                    }
                    if (new_area_data.Count() > 0)
                        area_context.AddRange(new_area_data);
                    
                }
                else if(area_list.Count() > 0)
                {
                    area_list.ForEach(x=> { x.is_active = false; });
                    area_context.UpdateRange(area_list);
                }
                data.Areas = null;

                var image_context = storageContext.Set<FileStore_Images>();
                var image_list = await image_context.Where(x => x.model_id == data.id && x.module == Config_FileStore_Modules.Warehouse_Image_Module ).ToListAsync();
                if (idata.Images != null && idata.Images.Count() > 0)
                {
                    var image_ids = idata.Images.Select(x => x.id).ToArray();
                    var image_update = image_list.Where(x => image_ids.Contains(x.id)).ToList();
                    var image_update_ids = image_update.Select(x => x.id).ToArray();
                    var image_remove = image_list.Where(x => !image_ids.Contains(x.id)).ToList();
                    var new_image_data = idata.Images.Where(x => !image_update_ids.Contains(x.id)).ToList();

                    image_update.ForEach(x =>
                    {
                        var area_idata = idata.Images.FirstOrDefault(a => a.id == x.id);
                        x.is_active = true;
                        x.updated_by = Client.CurrentUser.id;
                    });
                    image_remove.ForEach(x =>
                    {
                        x.is_active = false;
                        x.updated_by = Client.CurrentUser.id;
                    });
                    if (image_remove.Count() > 0)
                    {
                        var update_data = image_update.Concat(image_remove).ToList();
                        image_context.UpdateRange(update_data);
                    }
                    if (new_image_data.Count() > 0)
                    {
                        new_image_data.ForEach(x =>
                        {
                            x.created_by = Client.CurrentUser.id;
                        });
                        image_context.AddRange(new_image_data);
                    }

                }
                else if(image_list.Count() > 0)
                {
                    image_list.ForEach(x => { x.is_active = false; });
                    image_context.UpdateRange(image_list);
                }


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

        public PagedList<Warehouse> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var query = from data in this.dbSet
                        where data.is_active == true
                        orderby data.id descending
                        select new Warehouse()
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
            return new PagedList<Warehouse>(query, pagingParams.PageNumber, pagingParams.PageSize);
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

        public async Task<IEnumerable<Warehouse>> ItemsAynce(string txt = null, int limit = 20)
        {
            var query = from data in this.dbSet
                        orderby data.name descending
                        select new Warehouse()
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
