using Benriya.Clients.Wasm.Components.Forms;
using Benriya.Share.Models.FileStore;
using Benriya.Share.Models.Menus;

namespace Benriya.Clients.Wasm.Backend.Pages.Filestore
{
    public partial class FileStoreType_FormBase : FormBase<FileStore_FileType>
    {
        public FileStoreType_FormBase()
        {
            model = new FileStore_FileType();
            url = "FileStore/FileType/";
        }
    }
}
