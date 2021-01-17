using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Share.Models.CoreTags;
using Benriya.Utils.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Backend.Pages.TagsContent
{
    public class TagsFormBase : FormBase<Tags>
    {
        public TagsFormBase()
        {
            url = "core/Tags/";
        }

        protected async Task<IEnumerable<DropdownItem>> SearchCategory(string searchText)
        {
            var response = await _api.GetCustom<IEnumerable<DropdownItem>>($"core/TagGroup/Dropdown?limit=0&q={searchText}");
            if (response.Status == 200 && response.Data != null)
                return response.Data;
            else
                return new List<DropdownItem>();
        }
    }
}
