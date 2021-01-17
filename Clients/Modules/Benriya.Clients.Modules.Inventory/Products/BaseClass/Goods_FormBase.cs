using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Modules.Inventory.Share.Models.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benriya.Clients.Modules.Inventory.Products
{
    public class Goods_FormBase : FormBase<Goods>
    {
        public Goods_FormBase()
        {
            model = new Goods();
            url = "inventory/Goods/";
        }

        protected async Task<IEnumerable<Goods_Category>> SearchCategory(string searchText)
        {
            var response = await _api.GetCustom<IEnumerable<Goods_Category>>($"inventory/GoodsCategory/Items?limit=0&q={searchText}");
            if (response.Status == 200 && response.Data != null)
            {
                //foreach(var x in response.Data)
                //{
                //    var xx = x;
                //    xx.name = $"<span class=\"bg-dark\">zzz</span>"+xx.name;
                //}

                return response.Data;
            }
            else
                return new List<Goods_Category>();
        }

        protected async Task<IEnumerable<Goods_Unit>> SearchUnit(string searchText)
        {
            var response = await _api.GetCustom<IEnumerable<Goods_Unit>>($"inventory/GoodsUnit/Items?limit=0&q={searchText}");
            if (response.Status == 200 && response.Data != null)
                return response.Data;
            else
                return new List<Goods_Unit>();
        }
    }
}
