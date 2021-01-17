using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Benriya.Core;
using Benriya.Share.Abstractions;
using System.Threading.Tasks;
using Benriya.Utils;
using Benriya.Core.Filters;
using ExtCore.FileStorage.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Options;
using Benriya.Core.Classes;
using ExtCore.Data.Abstractions;
using Benriya.Share.Models.FileStore;
using Benriya.Utils.Extensions;
using Benriya.Core.Abstractions.FileStore;
using Microsoft.AspNetCore.Authorization;

namespace Benriya.Modules.FileStore.Controllers
{
    [Route("api/FileStore/[controller]")]
    [ApiController]    
    public class ImageController : CommonController
    {
        private readonly ILogger<ImageController> _logger;
        private readonly IFileStorage _fileStorage;
        private readonly IStorage _storage;
        private readonly AppSettings _appSetting;
        private readonly FileSystemManager _fileManager;
        public ImageController(ILogger<ImageController> logger,FileSystemManager fileManager, IRequestServices request, IOptions<AppSettings> appSetting,IStorage storage, IFileStorage fileStorage) : base(request)
        {
            _logger = logger;
            _fileStorage = fileStorage;
            _appSetting = appSetting.Value;
            _storage = storage;
            _fileManager = fileManager;
        }

        [HttpGet("{type}/{file}/{key}")]
        public async Task<ActionResult> Index(string type,string file,string key)
        {
            if(type.isNOEOW() || file.isNOEOW() || key.isNOEOW())
            {
                return BadRequest(new ApiResultError(400,"Type and File and Key is required"));
            }    
            var data = await _storage.GetRepository<IFileStore_Images_Repository>().GetAsync(x=>x.is_active == true && x.name.Equals(file) && x.check_sum.Equals(key));
            if(data == null)
            {
                return BadRequest(new ApiResultError(400,"Data not found"));
            }
            string path;
            if (data.is_public)
                path = $@"{_appSetting.PublicDirectory}\{_appSetting.FileStoreFolder}\{data.module}\{type}\";
            else
                path = $@"{AppContext.BaseDirectory}{_appSetting.FileStoreFolder}\{data.module}\{type}\";
            var image = System.IO.File.OpenRead(path+data.name);
            return File(image,MimeTypes.GetContentType(data.name));
        }

        [ApiAuthurize]
        [HttpPost("{module}/{isPublic}")]
        public async Task<ActionResult> Upload(string module,bool isPublic = false,int width = 0, int height = 0)
        {
            var result = new ApiResultModel<FileStore_Images>(); 
            if(width > 3000 || height > 3000)
            {
                result.BadRequest("Width and Height must be less than 3000");
                return BadRequest(result);
            }
            try
            {
                var file = Request.Form.Files[0];
                result = await _fileManager.SaveImage(module, file, isPublic,width,height);

                switch (result.Status)
                {
                    case 200: return Ok(result);
                    case 404: return NotFound(result);
                    default: return BadRequest(result);
                }
            }catch(Exception e)
            {
                result.BadRequest(e.Message);
                return BadRequest(result);
            }            
        }
    }
}
