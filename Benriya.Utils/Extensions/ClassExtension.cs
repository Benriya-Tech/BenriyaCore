using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Benriya.Utils.Extensions
{
    public class ClassExtension
    {
        public static string GetMemberName<TClass>(Expression<Func<TClass, object>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;
            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }
            return body.Member.Name;
        }

    }

    public static class FormFileExtensions
    {
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }

    public sealed class WriteOnce<T>
    {
        private T value;
        private bool hasValue;
        public override string ToString()
        {
            return hasValue ? Convert.ToString(value) : "";
        }
        public T Value
        {
            get
            {
                if (!hasValue) throw new InvalidOperationException("Value not set");
                return value;
            }
            set
            {
                if (hasValue) throw new InvalidOperationException("Value already set");
                this.value = value;
                this.hasValue = true;
            }
        }
        public T ValueOrDefault { get { return value; } }

        public static implicit operator T(WriteOnce<T> value) { return value.Value; }
    }
}
