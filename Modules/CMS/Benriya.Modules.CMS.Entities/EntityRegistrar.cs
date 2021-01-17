using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Benriya.Modules.CMS.Share.Models;

namespace Benriya.Modules.CMS.Entities
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelbuilder)
        {

            modelbuilder.Entity<Category>(e =>
            {
                e.HasIndex(x => x.id).IsUnique(true);
                e.HasIndex(x => x.name).IsUnique(false);
            });

            modelbuilder.Entity<Category_Tags>(e =>
            {
                e.HasOne(p => p.Category).WithMany(x => x.Category_Tags).HasForeignKey(x => x.category_id).IsRequired(true).OnDelete(DeleteBehavior.Restrict); //.HasConstraintName("FK_Tags_on_Category")
                e.HasIndex(x => x.id).IsUnique(true);
                e.HasIndex(x => x.category_id).IsUnique(false);
                e.HasIndex(x => x.tag_id).IsUnique(false);
            });

            modelbuilder.Entity<Contents>(e =>
            {
                e.HasOne(p => p.Category).WithMany(x => x.Contents).HasForeignKey(x => x.category_id).OnDelete(DeleteBehavior.Restrict); //.HasConstraintName("FK_Contents_on_Category")
                e.HasIndex(x => x.id).IsUnique(true);
                e.HasIndex(x => x.path).IsUnique(true);
                e.HasIndex(x => x.category_id).IsUnique(false);
                e.HasIndex(x => x.name).IsUnique(false);                
            });

            modelbuilder.Entity<Content_Tags>(e =>
            {
                e.HasOne(p => p.Content).WithMany(x => x.Content_Tags).HasForeignKey(x => x.content_id).IsRequired(true).OnDelete(DeleteBehavior.Restrict); //.HasConstraintName("FK_Tags_on_Contents")
                e.HasIndex(x => x.id).IsUnique(true);
                e.HasIndex(x => x.content_id).IsUnique(false);
                e.HasIndex(x => x.tag_id).IsUnique(false);
            });

            modelbuilder.Entity<Content_Likes>(e =>
            {
                e.HasOne(p => p.Content).WithMany(x => x.Content_Likes).HasForeignKey(x => x.content_id).IsRequired(true).OnDelete(DeleteBehavior.Restrict); //.HasConstraintName("FK_Likes_on_Contents")
                e.HasIndex(x => x.id).IsUnique(true);
                e.HasIndex(x => x.content_id).IsUnique(false);
            });


            modelbuilder.Entity<Comments>(e =>
            {
                e.HasOne(p => p.Contents).WithMany(x => x.Comments).HasForeignKey(x => x.content_id).IsRequired(true).OnDelete(DeleteBehavior.Restrict); //.HasConstraintName("FK_Comments_on_Contents")
                e.HasIndex(x => x.id).IsUnique(true);
            });

            modelbuilder.Entity<Comment_Users>(e =>
            {
                e.HasOne(p => p.Comments).WithMany(x => x.Comment_Users).HasForeignKey(x => x.comment_id).IsRequired(true).OnDelete(DeleteBehavior.Restrict); //.HasConstraintName("FK_Comment_Users_on_Comments")
                e.HasIndex(x => x.id).IsUnique(true);
                e.HasIndex(x => x.firstname).IsUnique(false);
                e.HasIndex(x => x.lastname).IsUnique(false);
                e.HasIndex(x => x.email).IsUnique(false);
                e.HasIndex(x => x.alias_name).IsUnique(false);
            });

            modelbuilder.Entity<Comment_Likes>(e =>
            {
                e.HasOne(p => p.Comment).WithMany(x => x.Comment_Likes).HasForeignKey(x => x.comment_id).IsRequired(true).OnDelete(DeleteBehavior.Restrict); //.HasConstraintName("FK_Likes_on_Comments")
                e.HasIndex(x => x.id).IsUnique(true);
                e.HasIndex(x => x.comment_id).IsUnique(false);
            });

        }
    }
    
}
