using Benriya.Utils.Models;
using Benriya.Utils.Pagingation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;

namespace Benriya.Utils
{
    [Serializable]
    public class AppSettings
    {
        public const string Settings = "AppSettings";
        public string Secret { get; set; }
        public string Hostname { get; set; }
        public bool AllowRegister { get; set; } = true;
        public bool ShowTraceId { get; set; } = true;
        public EmailSetting EmailSetting { get; set; }
        public string DocumentsPath { get; set; } = "";
        public string Assembly { get; set; }
        public bool AssemblyCandidate { get; set; }
        public int loginExpiryHrs { get; set; } = 4;
        public string PublicDirectory { get; set; }
        public string FileStoreFolder { get; set; }
        public List<FileSoreModules> FileSoreModules { get; set; }
    }

    public class FileSoreModules
    {
        public string name { get; set; }
        public string directory { get; set; }
    }
    public class EmailMessage
    {
        public string body { get; set; } // accepted HTML
        public string email { get; set; }
        public bool status { get; set; } = true;
    }
    public class EmailSetting
    {
        public Smtp Smtp { get; set; }
        public string[] cc { get; set; }
        public string SenderName { get; set; }
        public string Sender { get; set; }
    }

    public class Smtp
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; } = true;
        public bool DefaultCredentials { get; set; } = false;
    }

    [Serializable]
    public class EmailRequest
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string email { get; set; }
        public Guid id { get; set; }
        public FileStream Attachment { get; set; }
        public string file_name { get; set; }
    }

    [Serializable]
    public class ApiResultModel<T>
    {
        public int Status { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
        public ApiResultModel()
        {
            Status = 200;
            Title = "Successful";
            Message = "";
        }
        public void BadRequest(string message = null)
        {
            Status = 400;
            Title = "BadRequest";
            Message = String.IsNullOrEmpty(message) ? "The process has stopped" : message;
        }
        public void Notfound(string message = null)
        {
            Status = 404;
            Title = "NotFound";
            Message = String.IsNullOrEmpty(message) ? "Not found data" : message;
        }
        public void ServiceStopped(string message = null)
        {
            Status = 503;
            Title = "Unsuccessfully";
            Message = String.IsNullOrEmpty(message) ? "The process has stopped" : message;
        }

        public void Unauthorized(string message = null)
        {
            Status = 401;
            Title = "Unauthorized";
            Message = String.IsNullOrEmpty(message) ? "Unauthorized" : message;
        }
        public void Forbidden(string message = null)
        {
            Status = 403;
            Title = "Forbidden";
            Message = String.IsNullOrEmpty(message) ? "Forbidden" : message;
        }

    }

    [Serializable]
    public class ApiResultError : ApiResultModel<dynamic>
    {
        public ApiResultError(int code, string msg = null)
        {
            HttpStatusCode parsedCode = (HttpStatusCode)code;
            Status = code;
            Title = parsedCode.ToString();
            if (msg != null)
                Message = msg;
            else
                Message = parsedCode.ToString();
        }
    }

    [Serializable]
    public class ApiPagingtModel<T> : ApiResultModel<T>
    {
        public PagingHeader Paging { get; set; }
        //public List<T> DataList { get; set; }
    }

    [Serializable]
    public class ApiDropdownModel : ApiResultModel<IEnumerable<DropdownItem>>
    {
    }

    public class ErrorCheck
    {
        public string Message { get; set; }
        public bool is_error { get; set; } = false;
    }

    
    public class ErrorDetails : ApiResultModel<dynamic>
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }



    public class DropdownsGroup
    {
        public string label { get; set; }
        public string value { get; set; } = "0";
        public IEnumerable<DropdownItem> Options { get; set; }
        public string Class { get; set; }
        public string ClassName { get; set; }
        public bool Disabled { get; set; } = false;

    }

    public class ImagePath
    {
        public const string Original = "Original";
        public const string Thumbs = "Thumbs";
    }

    public interface IMongoSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

    public class MongoSettings : IMongoSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public static class SystemKeys
    {
        public const string JsonKey = "B3nRiY@#tEch";
    }
}
