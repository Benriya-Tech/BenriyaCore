using Benriya.Share.Models.SystemUsers;
using Benriya.Utils.Models;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Benriya.Utils.Extensions;
using Benriya.Utils;
using Microsoft.AspNetCore.Components;
using Benriya.Utils.Pagingation;
using Benriya.Core.Abstractions.SystemUsers;

using Benriya.Share.ViewModels;
//using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace Benriya.Core.Repositories.SystemUsers
{
    //[ScopedService]
    public class Users_Repository : RepositoryBase<Users>, IUsers_Repository
    {

        public async Task<Users> GetAsync(Expression<Func<Users, bool>> func)
        {
            return await this.dbSet.FirstOrDefaultAsync(func);
        }


        public async Task<List<Users>> ListAsync(Expression<Func<Users, bool>> func, int limit = 20)
        {
            return await this.dbSet.Where(func).OrderBy(o => o.firstname).Take(limit).ToListAsync();
        }

        public async Task<Users> CreateAsync(Users idata, string password)
        {
            if (idata.email.isNOEOW() || password.isNOEOW())
                throw new ArgumentNullException($"Username or password is required");
            if (idata.role_id < 1)
            {
                var role_context = this.storageContext.Set<User_Role>();
                var role = await role_context.FirstOrDefaultAsync(x => x.role_level == 0);
                if (role != null)
                    idata.role_id = role.id;
            }
            var u = await dbSet.FirstOrDefaultAsync(x => x.email == idata.email);
            if (u != null)
                throw new ArgumentNullException($"Email {idata.email} already exists");
            //var r = await dbSet.FirstOrDefaultAsync(x => x.code == ClaimPermission.General);

            await this.dbSet.AddAsync(idata);
            await this.SetPassword(idata, password);
            await storageContext.SaveChangesAsync();
            return idata;

        }

        private async Task SetPassword(Users idata, string password)
        {

            byte[] passwordHash, passwordSalt;
            PasswordUtils.CreatePasswordHash(idata.email, password, out passwordHash, out passwordSalt);
            var cred = new User_Credential()
            {
                user_id = idata.id,
                password_hash = passwordHash,
                password_salt = passwordSalt
            };
            var cred_context = this.storageContext.Set<User_Credential>();
            var cred_data = await cred_context.FirstOrDefaultAsync(x => x.user_id == idata.id);
            if (cred_data != null)
                cred_context.Update(cred);
            else
                await cred_context.AddAsync(cred);
        }

        public async Task<Users> UpdateAsync(Users idata, string password = null)
        {
            if (idata.id != Guid.Empty)
            {
                var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == idata.id);
                if (data == null)
                    throw new Exception($"Categoty id:{idata.id} is not found");
                data.firstname = idata.firstname;
                data.lastname = idata.lastname;
                data.alias_name = idata.alias_name;
                data.avatar = idata.avatar;
                data.email = idata.email;
                data.updated = DateTime.Now;
                data.updated_by = Client.CurrentUser.id;
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
                throw new Exception($"User id:{id} is not found");

            data.updated = DateTime.Now;
            data.is_active = false;
            data.updated_by = Client.CurrentUser.id;
            this.dbSet.Update(data);
            await storageContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var data = await this.dbSet.FirstOrDefaultAsync(x => x.id == id);
            if (data == null)
                throw new Exception($"User id:{id} is not found");
            this.dbSet.Remove(data);
            return true;
        }

        public PagedList<Users> GetList(PagingParams pagingParams, SearchOptions condition = null)
        {
            var query = from data in this.dbSet
                        where data.is_active == true
                        orderby data.id descending
                        select new Users()
                        {
                            id = data.id,
                            firstname = data.firstname,
                            lastname = data.lastname,
                            created = data.created,
                            updated = data.updated,
                            email = data.email,
                            is_active = data.is_active,
                            created_by = data.created_by
                        };
            if (condition != null)
            {
                if (!condition.txt.isNOEOW())
                {
                    query = query.Where(x =>
                    EF.Functions.Like(x.id.ToString(), $"%{condition.txt}%") ||
                    EF.Functions.Like(x.firstname, $"%{condition.txt}%") ||
                    EF.Functions.Like(x.lastname, $"%{condition.txt}%") ||
                    EF.Functions.Like(x.email, $"%{condition.txt}%")
                    );
                }
            }
            return new PagedList<Users>(query, pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<IEnumerable<DropdownItem>> DropdownListAsync(Expression<Func<DropdownItem, bool>> func, int limit = 20)
        {
            return await dbSet.Select(x => new DropdownItem()
            {
                label = $"{x.firstname} {x.lastname}",
                value = x.id.ToString(),
                description = x.alias_name
            }).Where(func).OrderBy(o => o.label).Take(limit).ToListAsync();
        }

        public async Task<Users> Authen_Validate(string username, string password)
        {
            if (username.isNOEOW() || password.isNOEOW())
                throw new ArgumentNullException($"Username or password is required");
            try
            {
                var user = await dbSet.Include(x => x.User_Credential).Include(x => x.User_Role).FirstOrDefaultAsync(x => x.email.Equals(username) && x.is_active == true && x.User_Role.is_active == true);// && x.User_Role.role_level > 1);
                if (user != null)
                {
                    var policy_db = this.storageContext.Set<Policy_Roles>();
                    user.User_Role.Policy_Roles = await policy_db.Where(x => x.role_id == user.role_id && x.is_active == true).ToListAsync();
                    bool check = PasswordUtils.VerifyPasswordHash(user.email, password, user.User_Credential.password_hash, user.User_Credential.password_salt);
                    if (check)
                    {
                        user.User_Role.User = null;
                        user.User_Credential = null;
                        return user;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
