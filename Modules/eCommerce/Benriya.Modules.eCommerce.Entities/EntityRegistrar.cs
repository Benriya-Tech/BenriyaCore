using Benriya.Modules.eCommerce.Share.Models.Orders;
using Benriya.Modules.eCommerce.Share.Models.Promotion;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;


namespace Benriya.Modules.eCommerce.Entities
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelbuilder)
        {

            modelbuilder.Entity<Order>();
            modelbuilder.Entity<Order_Detail>();
            modelbuilder.Entity<Pomotion_Master>();

        }
    }    
}
