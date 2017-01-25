using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosKernel4
{
    class File
    {
        List<String> data = new List<String>();
        string fn, ext;
        int fs = 0;


        public File(string fileName)
        {
            fn = fileName;
        }

        public void setFileExt(string extension)
        {
            ext = extension;
        }

        public void setFileSize(int fileSize)
        {
            fs = fileSize;
        }

        public void setFileData(List<string> fileData)
        {
            data = fileData;
        }

        public String getFileName()
        {
            return fn;
        }

        public string getSize()
        {

            return fs.ToString();
        }

        public String getExt()
        {
            return ext;
        }

        public List<String> getFileData()
        {
            return data;
        }
    }
}