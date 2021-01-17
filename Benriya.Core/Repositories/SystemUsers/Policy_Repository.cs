using Benriya.Share.Models.SystemUsers;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Benriya.Utils.Extensions;
using Benriya.Utils.Pagingation;
using Benriya.Utils.Models;
using Benriya.Core.Abstractions.SystemUsers;
using Benriya.Share.ViewModels;
using ExtCore.Infrastructure;

namespace Benriya.Core.Repositories.SystemUsers
{
    public class Policy_Repository : RepositoryBase<Policy_Roles>, IPolicy_Repository
    {

        private Policy_Roles SetClient(Policy_Roles idata, bool is_new = false)
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

        public async Task<Policy_Roles> GetAsync(Expression<Func<Policy_Roles, bool>> func)
        {
            return await this.dbSet.FirstOrDefaultAsync(func);
        }


        public async Task<List<Policy_Roles>> ListAsync(Expression<Func<Policy_Roles, bool>> func, int limit = 20)
        {
            var query = this.dbSet.Where(func);
            if (limit > 0)
                query = query.Take(limit);
            return await query.OrderBy(o => o.name).ToListAsync();
        }

        public async Task<Policy_Roles> CreateAsync(Policy_Roles idata)
        {
            if (idata.name.isNOEOW() || idata.code.isNOEOW())
                throw new ArgumentNullException($"Name and Code are required");
            var u = await dbSet.FirstOrDefaultAsync(x => x.code == idata.code);
            if (u != null)
                throw new ArgumentException($"Code {idata.code} already exists");
            idata = SetClient(idata, true);
            await this.dbSet.AddAsync(idata);
            await this.storageContext.SaveChangesAsync();
            return idata;
        }

        public async Task<Policy_Roles> UpdateAsync(Policy_Roles idata)
        {
            if (idata.id > 0)
            {
                var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == idata.id);
                if (data == null)
                    throw new Exception($"Permission id:{idata.id} is not found");
                data.name = idata.name;
                data.code = idata.code;
                data.is_active = idata.is_active;
                data = SetClient(data);
                this.dbSet.Update(data);
                await storageContext.SaveChangesAsync();
                return data;
            }
            return null;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            try
            {
                var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
                if (data == null)
                    throw new Exception($"Permission id:{id} is not found");
                data = SetClient(data);
                data.is_active = false;
                this.dbSet.Update(data);
                await storageContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
            if (data == null)
                throw new KeyNotFoundException($"Role id:{id} is not found");
            this.dbSet.Remove(data);
            return true;
        }

        public PagedList<Policy_Roles> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var query = from data in this.dbSet
                        where data.is_active == true
                        orderby data.id descending
                        select new Policy_Roles()
                        {
                            id = data.id,
                            name = data.name,
                            code = data.code,
                            description = data.description,
                            created = data.created,
                            updated = data.updated,
                            is_active = data.is_active,
                            created_by = data.created_by
                        };
            if (condition != null)
            {
                if (!condition.txt.isNOEOW())
                {
                    query = query.Where(x =>
                    EF.Functions.Like(x.id.ToString(), $"%{condition.txt}%") ||
                    EF.Functions.Like(x.name, $"%{condition.txt}%") ||
                    EF.Functions.Like(x.code, $"%{condition.txt}%") ||
                    EF.Functions.Like(x.description, $"%{condition.txt}%")
                    );
                }
            }
            return new PagedList<Policy_Roles>(query, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<IEnumerable<DropdownItem>> DropdownListAsync(Expression<Func<DropdownItem, bool>> func, int limit = 20)
        {
            return await dbSet.Select(x => new DropdownItem()
            {
                label = x.name,
                value = x.id.ToString(),
                description = x.description,
                ref_code = x.code
            }).Where(func).OrderBy(o => o.label).Take(limit).ToListAsync();
        }


        public async Task<bool> SetupMudules()
        {
            try
            {
                var perms = new ClaimPermission().GetPermissions();
                var extensions = new List<IExtension>();

                var all_policy_db = await dbSet.AsNoTracking().ToListAsync();
                var admin_role_db = this.storageContext.Set<User_Role>();
                var admin_codes = new string[] { "admin", "system", "systemadmin", "system_admin" };
                var admin_roles = await admin_role_db.AsNoTracking().Where(x => admin_codes.Contains(x.code.ToLower())).ToListAsync();
                if (admin_roles.Count() == 0)
                {
                    admin_roles = new List<User_Role>()
                    {
                        new User_Role(){code = "SYSTEM", name = "System",role_level = 30 },
                        new User_Role(){code = "SYSTEM_ADMIN", name = "System admin",role_level = 29 },
                        new User_Role(){code = "ADMIN", name = "Administrator",role_level = 28 },
                    };
                    await admin_role_db.AddRangeAsync(admin_roles);
                    await storageContext.SaveChangesAsync();
                }

                if (Client.ServerSettings.AssemblyCandidate)
                {
                    string[] starts = new string[] { "ExtCore" };
                    extensions = ExtensionManager.GetInstances<IExtension>().Where(x => !starts.Any(sl => x.Name.StartsWith(sl))).ToList();
                }
                else
                    extensions = ExtensionManager.GetInstances<IExtension>().ToList();

                var permission_context = this.storageContext.Set<Permission_Access>();
                var all_permission_db = await permission_context.AsNoTracking().ToListAsync();
                all_permission_db.ForEach(x => { x.is_active = false; });               

                if (all_permission_db.Count() > 0) return false;
         
                //var new_perms = new List<Permission_Access>();
                var new_pols = new List<Policy_Roles>();

                foreach (var perm in perms)
                {
                    var new_perm = new Permission_Access() { 
                        name = perm,
                        is_active = true,
                        code = perm,
                        description = perm
                    };             
                    await permission_context.AddAsync(new_perm);
                    await storageContext.SaveChangesAsync();

                    foreach (var ext in extensions)
                    {
                        var policy_code = $"{ext.Code}.{perm}";
                        foreach (var role in admin_roles)
                        {
                            new_pols.Add(new Policy_Roles()
                            {
                                role_id = role.id,
                                permission_id = new_perm.id,
                                code = policy_code,
                                module_name = ext.Name,
                                module_code = ext.Code,
                                User_Role = null,
                                Permission = null
                            });
                        }
                    }
                }
                await dbSet.AddRangeAsync(new_pols);
                await storageContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }

}
