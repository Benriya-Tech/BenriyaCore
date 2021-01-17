using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Modules.Inventory.Share.Models.Products;

namespace Benriya.Clients.Modules.Inventory.Products
{
    public class Cagetgory_FormBase : FormBase<Goods_Category>
    {
        public Cagetgory_FormBase()
        {
            model = new Goods_Category();
            url = "inventory/GoodsCategory/";
        }
    }
}
