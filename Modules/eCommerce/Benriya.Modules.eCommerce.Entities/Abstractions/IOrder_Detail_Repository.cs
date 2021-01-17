using Benriya.Modules.eCommerce.Share.Models.Orders;
using Benriya.Share.Abstractions;
using ExtCore.Data.Abstractions;
using System;


namespace Benriya.Modules.eCommerce.Entities.Abstractions
{
    public interface IOrder_Detail_Repository : ICommon_Repository<Order_Detail,Guid>, IRepository
    {

    }
}
