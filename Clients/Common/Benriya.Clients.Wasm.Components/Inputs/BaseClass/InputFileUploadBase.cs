using Benriya.Clients.Wasm.Components.Classes;
using Benriya.Clients.Wasm.Components.Services;
using Benriya.Share.Models.FileStore;
using Benriya.Utils;
using Benriya.Utils.Extensions;
using Benriya.Utils.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Tewr.Blazor.FileReader;

namespace Benriya.Clients.Wasm.Components.Inputs
{
    public class InputFileUploadBase<T> : ComponentBase
    {
        [Inject]
        public IFileReaderService _fileReader { get; set; }
        [Inject]
        public IApiClientService<T> _api { get; set; }
        [Inject]
        private HttpClient _httpClient { get; set; }

        [Parameter]
        public EventCallback<T> InsertImage { get; set; }
        [Parameter]
        [Required]
        public string Module { get; set; }
        [Parameter]       
        [Range(0,4000)]
        public bool IsPublic { get; set; }
        [Parameter]
        [Range(0,4000)]
        public int Width { get; set; } = 0;
        [Parameter]
        public int Height { get; set; } = 0;

        protected ElementReference inputFile;
        protected IFileInfo fileInfo { get; set; }
        public bool isLoading { get; set; }
        protected bool isHide { get; set; } = true;
        protected string Message;
        IFileReference file;
        public async Task InsertImageClick()
        {
            Message = null;
            if (file != null)
            {
                var block = new string[]{"download","script","exe","command","bat","com","db","js","ja","xa","ii","is","ha","bin","cmd"};
                if (block.Any(fileInfo.Type.Contains))
                {
                    Message = "File type not allowed, please choose a correct file.";
                    return;
                }
                using (var ms = await file.CreateMemoryStreamAsync(4 * 1024))
                {

                    var content = new MultipartFormDataContent();
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                    content.Add(new StreamContent(ms, Convert.ToInt32(ms.Length)), "file", fileInfo.Name);
                    var response = await _api.UploadFile<T>($"FileStore/Image/{Module}/{IsPublic}?width={Width}&height={Height}", content);
                    if (response != null)
                    {
                        if (response.Status == 200 && response.Data != null)
                        {
                            //T xdata = response.Data;
                            //Type model = xdata.GetType();
                            //PropertyInfo uri_prop = model.GetProperty("url");
                            //Console.WriteLine(uri_prop.GetValue(xdata).ToString());
                            //var file_content = await _api.GetFile(uri_prop.GetValue(xdata).ToString());
                            //PropertyInfo content_prop = model.GetProperty("file_content");
                            //content_prop.SetValue(xdata,file_content,null);
                            //await InsertImage.InvokeAsync(xdata);
                            await InsertImage.InvokeAsync(response.Data);
                        }
                        else
                            Message = response.Message;
                    }
                }                
            }
            else
                Message = "Please choose your an image";
        }        
        protected async Task ReadFileAsync()
        {
            Message = null;
            file = (await _fileReader.CreateReference(inputFile).EnumerateFilesAsync()).FirstOrDefault();
            if (file == null)
            {
                Message = "Cannot read file data";
                return;
            }
            isLoading = true;            
            fileInfo = await file.ReadFileInfoAsync();
            StateHasChanged();
            await Task.Delay(300);
            await InsertImageClick();      
            isLoading = false;
        }
    }
}
