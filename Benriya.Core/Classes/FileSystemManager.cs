using Benriya.Core.Abstractions.FileStore;
using Benriya.Share.Abstractions;
using Benriya.Share.Models.FileStore;
using Benriya.Utils;
using Benriya.Utils.Extensions;
using ExtCore.Data.Abstractions;
using ExtCore.FileStorage.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Core.Classes
{
    public class FileSystemManager
    {
        private readonly ILogger<FileSystemManager> _logger;
        private readonly IFileStorage _fileStorage;
        private readonly IStorage _storage;
        private readonly AppSettings _appSetting;
        public string _module { get; set; }
        public FileSystemManager(ILogger<FileSystemManager> logger, IOptions<AppSettings> appSetting, IStorage storage, IFileStorage fileStorage)
        {
            _logger = logger;
            _fileStorage = fileStorage;
            _appSetting = appSetting.Value;
            _storage = storage;
        }

        public async Task<ApiResultModel<FileStore_Images>> SaveImage(string mudule,IFormFile file,bool isPublic = false,int width = 0, int height = 0)
        {            
            var result = new ApiResultModel<FileStore_Images>();
            if (mudule.isNOEOW())
            {
                result.BadRequest("Module name is required");
                return result;
            }
            _module = mudule;
            try
            {
                if (file.Length > 0)
                {
                    var file_extension = Path.GetExtension(file.FileName);
                    var ext_data = await _storage.GetRepository<IFileStore_Type_Repository>().ListAsync(x => x.file_type == File_Types.Image && x.is_active == true);
                    var check_type = ext_data.Where(x => x.file_extension == file_extension).FirstOrDefault();
                    if(check_type == null)
                    {
                        result.BadRequest($"File type not allowed, file must be ({(string.Join(",", ext_data.Select(x=>x.file_extension).ToArray()))})");
                        return result;
                    }

                    string filename = Guid.NewGuid().ToString() + file_extension;
                    string url_img = null;
                    string path_img = null;
                    string path = @"\";
                    IFileProxy fileProxy;
                    if (isPublic && _appSetting.PublicDirectory != null)
                    {
                        path = $@"{_appSetting.PublicDirectory}\{_appSetting.FileStoreFolder}\{_module}\{ImagePath.Original}\";
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        _fileStorage.CreateDirectoryProxy(path);
                        fileProxy = _fileStorage.CreateFileProxy(path, filename);
                        path_img = $@"{_appSetting.FileStoreFolder}/{_module}/";
                        url_img =  $@"{path_img}{ImagePath.Thumbs}/{filename}";
                    }
                    else
                    {
                        path = $@"{AppContext.BaseDirectory}{_appSetting.FileStoreFolder}\{_module}\{ImagePath.Original}\";
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        fileProxy = _fileStorage.CreateFileProxy(path, filename);
                        path_img = $@"FileStore/Image/";
                        url_img = $@"{path_img}{ImagePath.Thumbs}/{filename}";
                    }
                    await fileProxy.WriteStreamAsync(file.OpenReadStream());
                    new ImageResize(path + filename).ToThumnail(width, height);

                    if (url_img != null)
                    {

                        var data = await _storage.GetRepository<IFileStore_Images_Repository>().CreateAsync(new FileStore_Images()
                        {
                            file_type_id = check_type.id,
                            module = _module,
                            title = Path.GetFileNameWithoutExtension(file.FileName),
                            is_public = isPublic,
                            name = filename,
                            url = url_img,
                            check_sum = CryptographyCore.CreateCheckSUm_Stream(file.OpenReadStream())
                        });
                        result.Data = data;
                        return result;
                    }
                    else
                    {
                        result.BadRequest();
                        return result;
                    }
                }
                else
                {
                    result.BadRequest("File is required");
                    return result;
                }
            }
            catch (Exception e)
            {
                result.BadRequest(e.Message);
                return result;
            }
        }
    }
}
