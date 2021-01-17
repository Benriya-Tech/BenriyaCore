using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Modules.CMS.Share.Models;
using Benriya.Share.Models;
using Benriya.Share.Models.CoreTags;
using Benriya.Utils.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Benriya.Clients.Modules.CMS.Categories
{
    public class CategoryFormBase : FormBase<Category>
    {
        protected List<DropdownItem> SearchTagsData { get; set; }
        protected bool isLoadingTag { get; set; }
        //protected List<ItemUuidValue> TagsSelect { get; set; } = new List<ItemUuidValue>();
        public CategoryFormBase()
        {
            model = new Category();
            url = "cms/Category/";
        }
        protected void OnSearchTags(IEnumerable<DropdownItem> tagsData)
        {
            SearchTagsData = tagsData.ToList();
            CheckTagsData();
        }

        protected void AddTags(DropdownItem tag)
        {
            if (model.Tags == null)
                model.Tags = new Collection<ItemUuidValue>();
            //SearchTagsData.Remove(tag);
            model.Tags.Add(new ItemUuidValue() {ref_id = tag.uid,name=tag.label });
            CheckTagsData();
        }

        protected void RemoveTags(ItemUuidValue tag)
        {
            model.Tags.Remove(tag);
            CheckTagsData();
        }
        protected void OnLoadingTag(bool isLoading)
        {
            isLoadingTag = isLoading;
        }

        private void CheckTagsData()
        {
            if(SearchTagsData != null && model.Tags != null)
            {
                SearchTagsData.ForEach(x => {
                    x.is_active = model.Tags.FirstOrDefault(s=>s.ref_id == x.uid) == null;      
                }); 
            }
        }
        
    }
}
