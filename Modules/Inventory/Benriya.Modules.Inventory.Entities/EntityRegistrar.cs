using Benriya.Modules.Inventory.Share.Models.Products;
using Benriya.Modules.Inventory.Share.Models.Warehouses;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;


namespace Benriya.Modules.Inventory.Entities
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelbuilder)
        {

            modelbuilder.Entity<Warehouse>();
            modelbuilder.Entity<Warehouse_Area>();
            modelbuilder.Entity<Warehouse_Store>();

            modelbuilder.Entity<Goods>();
            modelbuilder.Entity<Goods_Category>();
            modelbuilder.Entity<Goods_Color>();
            modelbuilder.Entity<Goods_Image>();
            modelbuilder.Entity<Goods_Tags>();
            modelbuilder.Entity<Goods_Unit>();
        }
    }    
}
