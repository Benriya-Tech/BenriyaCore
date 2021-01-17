using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Modules.CMS.Share.Models;
using Benriya.Share.Models;
using Benriya.Utils;
using Benriya.Utils.Extensions;
using Benriya.Utils.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Benriya.Clients.Modules.CMS.ContentManage
{
    public class ContentFormBase : FormBase<Contents>
    {
        protected List<DropdownItem> SearchTagsData { get; set; }
        protected bool isLoadingTag { get; set; }
        protected Category selectedCategory { get; set; } = new Category();
        protected int cate_id; 
        public ContentFormBase()
        {
            model = new Contents();
            url = "cms/Content/";
            TextEditorField = nameof(Contents.body);
            Redirect = "/cms/contents";
        }

        //protected override async Task OnParametersSetAsync()
        //{
        //    if (model.Category != null)
        //        Console.WriteLine($"----------> {model.Category.name}");
        //}

        protected async Task<IEnumerable<Category>> SearchCategory(string searchText)
        {
            var response = await _api.GetCustom<IEnumerable<Category>>($"cms/Category/Items?limit=0&q={searchText}");
            if (response.Status == 200 && response.Data != null)
                return response.Data;
            else
                return new List<Category>();
        }

        protected Category OnChangeCategory(object cates,object cate = null)
        {
            //if (cate == null) return cate;
            //model.Category = cate;
            //selectedCategory = cate;
            //cate_id = model.Category.id;
            //return cate;
            return new Category();
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
            model.Tags.Add(new ItemUuidValue() { ref_id = tag.uid, name = tag.label,description=tag.description });
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
            if (SearchTagsData != null && model.Tags != null)
            {
                SearchTagsData.ForEach(x =>
                {
                    x.is_active = model.Tags.FirstOrDefault(s => s.ref_id == x.uid) == null;
                });
            }
        }

    }
}
