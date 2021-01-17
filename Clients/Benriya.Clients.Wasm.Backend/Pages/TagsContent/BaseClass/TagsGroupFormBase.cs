using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Share.Models.CoreTags;


namespace Benriya.Clients.Wasm.Backend.Pages.TagsContent
{
    public class TagsGroupFormBase : FormBase<Tags_Group>
    {
        public TagsGroupFormBase()
        {
            url = "core/TagGroup/";
        }    
    }
}
