using System;
using System.Linq;
using System.IO;
using SWI.SoftStock.Client.Common.Helpers;

namespace SWI.SoftStock.Client.Common
{
    //public static class UniqueIdentifier
    //{
    //    private static string GetFullFileName(Type typeParameterType)
    //    {
    //        return FileHelper.GetFullFileName(typeParameterType.Name.ToLower());
    //    }

    //    public static void WriteUniqueIdentifier<T>(Guid uniqueId)
    //    {
    //        var fullFileName = GetFullFileName(typeof(T));
    //        File.WriteAllText(fullFileName,uniqueId.ToString());
    //    }

    //    public static Guid ReadUniqueIdentifier<T>()
    //    {
    //        var fullFileName = GetFullFileName(typeof(T));
    //        var uId=File.ReadAllLines(fullFileName).FirstOrDefault();
    //        Guid result;
    //        Guid.TryParse(uId, out result);
    //        return result;            
    //    }
    //}
}
