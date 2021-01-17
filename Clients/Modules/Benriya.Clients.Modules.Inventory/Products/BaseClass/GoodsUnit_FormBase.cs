using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Modules.Inventory.Share.Models.Products;


namespace Benriya.Clients.Modules.Inventory.Products
{
    public class GoodsUnit_FormBase : FormBase<Goods_Unit>
    {
        public GoodsUnit_FormBase()
        {
            model = new Goods_Unit();
            url = "inventory/GoodsUnit/";
        }
    }
}
