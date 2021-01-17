using Benriya.Modules.eCommerce.Share.Models.Orders;
using Benriya.Share.Abstractions;
using ExtCore.Data.Abstractions;
using System;

namespace Benriya.Modules.eCommerce.Entities.Abstractions
{
    public interface IOrder_Repository: ICommon_Repository<Order,Guid>, IRepository
    {

    }
}
