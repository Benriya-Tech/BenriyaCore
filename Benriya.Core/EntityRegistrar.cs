using Benriya.Share.Models.Menus;
using Benriya.Share.Models.SystemUsers;
using Benriya.Share.Models.CoreTags;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Toolbelt.ComponentModel.DataAnnotations;
using Benriya.Share.Models.FileStore;
using Benriya.Share.Models.Colors;
using Benriya.Share.Models.Helpers;

namespace Benriya.Data
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelBuilder)
        {
            //------------------------ User
            modelBuilder.Entity<User_Role>(e =>
            {
                //e.HasMany(p => p.User).WithOne(x => x.User_Role).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
                e.HasIndex(o => o.id).IsUnique(true);
                e.HasIndex(o => o.code).IsUnique(true);
            });
            modelBuilder.Entity<Users>(e =>
            {
                e.HasOne(p => p.User_Role).WithMany(x => x.User).HasForeignKey(x => x.role_id).IsRequired(false).OnDelete(DeleteBehavior.Restrict); //HasConstraintName("FK_Users_with_Role_id").
                e.HasIndex(x => x.id).IsUnique(true);
                e.HasIndex(x => x.email).IsUnique(true);
            });

            modelBuilder.Entity<User_Credential>(e =>
            {                
                e.HasOne(p => p.User).WithOne(x => x.User_Credential).HasForeignKey<User_Credential>(x => x.user_id).IsRequired(true).OnDelete(DeleteBehavior.Restrict); //HasConstraintName("FK_Credentials_on_Users_id").
                e.HasIndex(x => x.id).IsUnique(true); 
                e.HasIndex(o => o.user_id).IsUnique(true);
            });
            modelBuilder.Entity<User_Token>(e =>
            {
                e.HasOne(p => p.User).WithOne(x => x.User_Token).HasForeignKey<User_Token>(x => x.user_id).IsRequired(true).OnDelete(DeleteBehavior.Restrict); //.HasConstraintName("FK_Token_on_Users_id")
                e.HasIndex(x => x.id).IsUnique(true);
                e.HasIndex(o => o.user_id).IsUnique(true);
            });
            modelBuilder.Entity<User_Directauth>(e =>
            {
                e.HasOne(p => p.User).WithMany(x => x.Direct_Auth).HasForeignKey(x => x.user_id).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
                e.HasIndex(o => o.id).IsUnique(true);
                e.HasIndex(o => o.user_id).IsUnique(false);
                e.HasIndex(o => o.key).IsUnique(true);
            });
            modelBuilder.Entity<User_Login>(e =>
           {
               e.HasOne(p => p.User).WithOne(x => x.User_Login).HasForeignKey<User_Login>(x => x.user_id).IsRequired(true).OnDelete(DeleteBehavior.Restrict); //HasConstraintName("FK_Login_on_Users_id").
               e.HasIndex(x => x.id).IsUnique(true);
               e.HasIndex(o => o.user_id).IsUnique(true);
           });

            //------------------------ Tags
            modelBuilder.Entity<Tags_Group>(e =>
            {                
                e.HasIndex(o => o.id).IsUnique(true);
                e.HasIndex(o => o.name).IsUnique(true);
            });
            modelBuilder.Entity<Tags>(e =>
            {
                e.HasOne(p => p.Group).WithMany(x => x.Tags).HasForeignKey(x => x.group_id).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
                e.HasIndex(o => o.id).IsUnique(true);
                e.HasIndex(o => o.group_id).IsUnique(false);
                e.HasIndex(o => o.name).IsUnique(false);
            });

            //------------------------ Permission
            modelBuilder.Entity<Permission_Access>(e =>
            {
                e.HasIndex(o => o.id).IsUnique(true);
            });

            modelBuilder.Entity<Policy_Roles>(e =>
            {
                e.HasOne(x => x.Permission).WithMany(x => x.Policy_Roles).HasForeignKey(x => x.permission_id).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.User_Role).WithMany(x => x.Policy_Roles).HasForeignKey(x => x.role_id).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
                e.HasIndex(o => o.id).IsUnique(true);
            });


            //------------------------ Menus
            modelBuilder.Entity<SystemMenu>(e =>
            {
                e.HasIndex(o => o.id).IsUnique(true);
            });
            modelBuilder.Entity<QuickMenu>(e =>
             {
                 e.HasIndex(o => o.id).IsUnique(true);
             });


            //------------------------ File store
            modelBuilder.Entity<FileStore_FileType>();
            modelBuilder.Entity<FileStore_Documents>(e =>
            {
                e.HasOne(p => p.FileType).WithMany(x => x.Documents).HasForeignKey(x => x.file_type_id);//.HasConstraintName("FK_Filestore_Docs_FileType_id");//.IsRequired(true).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<FileStore_Images>(e =>
            {
                e.HasOne(p => p.FileType).WithMany(x => x.Images).HasForeignKey(x => x.file_type_id);//.HasConstraintName("FK_Filestore_Images_FileType_id");//.IsRequired(true).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<FileStore_Files>(e =>
            {
                e.HasOne(p => p.FileType).WithMany(x => x.Files).HasForeignKey(x => x.file_type_id);//.HasConstraintName("FK_Filestore_Files_FileType_id");//.IsRequired(true).OnDelete(DeleteBehavior.Restrict);
            });



            modelBuilder.Entity<Color_Store>();
            modelBuilder.Entity<Weight_Type>();
            
        }
    }
}
