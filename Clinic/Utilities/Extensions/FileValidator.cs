﻿using Clinic.Utilities.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Runtime.CompilerServices;

namespace Clinic.Utilities.Extensions
{
    public static class FileValidator
    {
        public static bool ValidateType(this IFormFile file, string type)
        {
            if (file.ContentType.Contains(type))
            {
                return true;
            }
            return false;
        }
        public static bool ValidateSize(this IFormFile file, FileSize fileSize, int size)
        {
            switch (fileSize)
            {
                case FileSize.KB:
                    return file.Length <= size * 1024;
                case FileSize.MB:
                    return file.Length <= size * 1024 * 1024;
                case FileSize.GB:
                    return file.Length <= size * 1024 * (1024 * 1024);
            }
            return false;
        }
        public static string GetPath(this string fileName, params string[] roots)
        {
            string path = string.Empty; 
            for (int i = 0; i < roots.Length; i++)
            {
                Path.Combine(path, roots[i]);
            }
            return Path.Combine(path, fileName);
        } 
        public static string GetFileExtension(this IFormFile file)
        {
            int lastDotIndex = file.Name.LastIndexOf('.');
            if (lastDotIndex != -1)
            {
                string.Concat(Guid.NewGuid().ToString(), file.Name.Substring(lastDotIndex));
            }
            return file.Name;
        }
        public async static Task<string> CreateFileAsync(this IFormFile file, params string[] roots)
        {
            string fileName = string.Concat(Guid.NewGuid().ToString(), GetFileExtension(file));
            string path = GetPath(fileName, roots);
            using (FileStream fileStream = new(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }
        public static void DeleteFile(this string fileName, params string[] roots)
        {
            string path = GetPath(fileName, roots);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
