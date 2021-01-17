using Benriya.Clients.Wasm.Components.Services;
using Benriya.Share.Models.FileStore;
using Benriya.Utils;
using Benriya.Utils.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tewr.Blazor.FileReader;

namespace Benriya.Clients.Wasm.Components.Forms
{
    public class FormInsertImageBase : ComponentBase
    {
        [Inject]
        public IFileReaderService _fileReader { get; set; }
        [Inject]
        public IApiClientService<string> _api { get; set; }
        [Inject]
        private HttpClient _httpClient { get; set; }
        [Inject]
        private IJSRuntime _JSRuntime { get; set; }
        [Parameter]
        public EventCallback<string> InsertImage { get; set; }

        protected ElementReference inputFile;
        protected IFileInfo fileInfo { get; set; }
        protected bool isLoading { get; set; } = false;
        protected string image_url { get; set; }
        protected bool isHide { get; set; } = true;
        protected string Message;
        IFileReference file;
        public async Task InsertImageClick()
        {
            Message = null;
            if (file != null)
            {
                if (!ImageHelper.IsImageExtension(fileInfo.Type))
                {
                    Message = "File type not allowed, please choose an image";
                    return;
                }
                isLoading = true;
                using (var ms = await file.CreateMemoryStreamAsync(4 * 1024))
                {

                    var content = new MultipartFormDataContent();
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                    content.Add(new StreamContent(ms, Convert.ToInt32(ms.Length)), "file", fileInfo.Name);
                    var response = await _api.UploadFile<FileStore_Model>("FileStore/Image/CMS.Contents/true", content);
                    if (response != null)
                    {
                        if (response.Status == 200 && !response.Data.name.isNOEOW())
                        {
                            await InsertImage.InvokeAsync(response.Data.url);
                            await _JSRuntime.InvokeVoidAsync("Benriya.closeModal", "#FormInsertImage");
                        }
                        else
                            Message = response.Message;
                    }
                }
                isLoading = false;
            }
            else if (!image_url.isNOEOW())
            {
                await InsertImage.InvokeAsync(image_url);
                await _JSRuntime.InvokeVoidAsync("Benriya.closeModal", "#FormInsertImage");
            }
            else
                Message = "Please choose your an image";
        }        
        protected async Task ReadFileAsync()
        {
            file = (await _fileReader.CreateReference(inputFile).EnumerateFilesAsync()).FirstOrDefault();
            if (file == null)            
                return;
            fileInfo = await file.ReadFileInfoAsync();
            if (!ImageHelper.IsImageExtension(fileInfo.Type))            
                Message = "File type not allowed";                
        }
    }
}
