using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Benriya.Utils.Pagingation
{
    public class PagingParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class SearchOptions
    {
        public string txt { get; set; }
        public string name { get; set; }
        public string key { get; set; }
        public Guid uuid { get; set; }
    }

    public class LinkInfo
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
    }

    [Serializable]
    public class PagingHeader
    {
        public int TotalItems { get; set; }
        public int PageNumber { get; set;}
        public int PageSize { get; set;}
        public int TotalPages { get; set;}
        public string ToJson() => JsonConvert.SerializeObject(this, new JsonSerializerSettings{ ContractResolver = new CamelCasePropertyNamesContractResolver() });

    }

    public class PagedList<T>
    {
        //private IQueryable<T> _source;
        public PagedList(IQueryable<T> source, int pageNumber, int pageSize)//,bool toList = false)
        {
            //try
            //{
                //_source = source;
                pageNumber = pageNumber < 1 ? 1 : pageNumber;
                this.PageNumber = pageNumber;                
                this.PageSize = pageSize;
                if (source != null)
                {
                    this.TotalItems = source.Count();
                    this.PageNumber = this.TotalItems <= this.PageSize ? 1 : this.PageNumber;
                    //if(toList)
                        this.List = source.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
                }
                else
                {
                    this.TotalItems = 0;
                    this.List = new List<T>();
                }
            //}catch(Exception ex)
            //{
            // throw;
            //}
        }

        public int TotalItems { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public List<T> List { get; }
        public int TotalPages => (int)Math.Ceiling(this.TotalItems / (double)this.PageSize);
        public bool HasPreviousPage => this.PageNumber > 1;
        public bool HasNextPage => this.PageNumber < this.TotalPages;
        public int NextPageNumber =>
               this.HasNextPage ? this.PageNumber + 1 : this.TotalPages;
        public int PreviousPageNumber =>
               this.HasPreviousPage ? this.PageNumber - 1 : 1;

        public PagingHeader GetHeader()
        {
            return new PagingHeader() {PageNumber = this.PageNumber,PageSize=this.PageSize,TotalItems=this.TotalItems,TotalPages=this.TotalPages };
        }
    }
}
