﻿using Business.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Extension
{
    public class Helper
    {
        public static string SaveFile(string rootPath,string folder,IFormFile file)
        {
            if(file.ContentType !="image/png" && file.ContentType !="image/jpeg")
                throw new ImageContentTypeException("Image content type sehvdir");

            if (file.Length > 2097152)
                throw new ImageSizeException("Image size max 2mb ola biler!");

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            string path = rootPath + $@"\{folder}\"+ fileName;

            using (FileStream fileStream = new FileStream(path,FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return fileName;
        }

        public static void DeleteFile(string rootPath,string folder, string fileName)
        {
           string path = rootPath + $@"\{folder}\" + fileName;

            if (!File.Exists(path))
                throw new Exceptions.FileNotFoundException("File tapilmadi");
            File.Delete(path);
        }
    }
}