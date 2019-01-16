using System.IO;
using PersonApi.Data.VO;

namespace PersonApi.Business.Implementations
{
    public class FileBusinessImpl : IFileBusiness
    {
        public byte[] GetPDFFile()
        {
            string path = Directory.GetCurrentDirectory();
            var fullPath = path + "\\Other\\Profile.pdf";
            return File.ReadAllBytes(fullPath);
        }
    }
}