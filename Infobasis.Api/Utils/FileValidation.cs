using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Infobasis.Api.Utils
{
    public enum FileExtension
    {
        GIF = 7173,
        JPG = 255216,
        BMP = 6677,
        PNG = 13780,
        DOC = 208207,
        DOCX = 8075,
        XLSX = 8075,
        JS = 239187,
        XLS = 208207,
        SWF = 6787,
        MID = 7784,
        RAR = 8297,
        ZIP = 8075,
        XML = 6063,
        TXT = 7067,
        MP3 = 7368,
        WMA = 4838,
        PDF = 3780,
        CHM = 7384,
        HTML = 6033,
        ASPX = 239187,
        CS = 117115,
        SQL = 255254,

        // 239187 aspx
        // 117115 cs
        // 119105 js
        // 210187 txt
        //255254 sql 		
        // 7790 exe dll,
        // 8297 rar
        // 6063 xml
        // 6033 html
    }

    public class FileValidation
    {
        private static FileExtension[] imageFileExtension = { FileExtension.BMP,
                                                     FileExtension.GIF, FileExtension.JPG, FileExtension.PNG};

        public static bool IsImageExtension(HttpPostedFile httpPostedFile)
        {
            int fileLen = httpPostedFile.ContentLength;
            byte[] imgArray = new byte[fileLen];
            httpPostedFile.InputStream.Read(imgArray, 0, fileLen);
            MemoryStream ms = new MemoryStream(imgArray);
            System.IO.BinaryReader br = new System.IO.BinaryReader(ms);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = br.ReadByte();
                fileclass = buffer.ToString();
                buffer = br.ReadByte();
                fileclass += buffer.ToString();
            }
            catch
            {
            }
            br.Close();
            ms.Close();
            //注意将文件流指针还原
            httpPostedFile.InputStream.Position = 0;
            foreach (FileExtension fe in imageFileExtension)
            {
                if (Int32.Parse(fileclass) == (int)fe)
                    return true;
            }
            return false;
        }

        public static bool IsAllowedExtension(HttpPostedFile httpPostedFile, FileExtension[] fileEx)
        {
            int fileLen = httpPostedFile.ContentLength;
            byte[] imgArray = new byte[fileLen];
            httpPostedFile.InputStream.Read(imgArray, 0, fileLen);
            MemoryStream ms = new MemoryStream(imgArray);
            System.IO.BinaryReader br = new System.IO.BinaryReader(ms);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = br.ReadByte();
                fileclass = buffer.ToString();
                buffer = br.ReadByte();
                fileclass += buffer.ToString();
            }
            catch
            {
            }
            br.Close();
            ms.Close();
            //注意将文件流指针还原
            httpPostedFile.InputStream.Position = 0;
            foreach (FileExtension fe in fileEx)
            {
                if (Int32.Parse(fileclass) == (int)fe)
                    return true;
            }
            return false;
        }
    }
}