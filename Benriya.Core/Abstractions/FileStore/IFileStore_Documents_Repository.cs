using Benriya.Share.Models.FileStore;
using ExtCore.Data.Abstractions;

namespace Benriya.Core.Abstractions.FileStore
{
    public interface IFileStore_Documents_Repository: IFileStore_Common<FileStore_Documents>,IRepository
    {
        
    }
}