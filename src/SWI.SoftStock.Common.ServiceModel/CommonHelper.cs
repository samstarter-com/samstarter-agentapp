using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;
using System.Globalization;
using System.Reflection;

namespace SWI.SoftStock.Common
{
    /// <summary>
    /// Helper class for common simple tasks and extensions
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// Smart clones <see cref="ICloneable"/> item.
        /// If return failed, throws catched exception or returns default value for <see cref="T"/>, depending on <see cref="throwException"/> flag.
        /// By default, returns default value for <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="throwException"></param>
        /// <returns></returns>
        public static T SmartClone<T>(this T item, bool throwException = false)
            where T : ICloneable
        {
            if (ReferenceEquals(null, item))
                throw new ArgumentNullException("item");

            try
            {
                return (T)item.Clone();
            }
            catch
            {
                if (throwException)
                    throw;

                return default(T);
            }
        }

        /// <summary>
        /// Creates string representation of specified <see cref="list"/>, using either default <see cref="string.Format(string,object)"/> or specified <see cref="toStringOverride"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <param name="format"></param>
        /// <param name="toStringOverride"></param>
        /// <returns></returns>
        public static string ListToString<T>(this IEnumerable<T> list, string separator = "; ", string format = null, Func<T, string> toStringOverride = null)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            var sb = new System.Text.StringBuilder();

            var toStringFunc = toStringOverride ?? new Func<T, string>(item => String.Format(format ?? "{0}", item));

            foreach (var item in list)
            {
                sb.AppendFormat("{0}{1}", toStringFunc(item), separator);
            }

            return sb.ToString();
        }        

        /// <summary>
        /// Converts secure string to string
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public static string SecureStringToString(this SecureString value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            var bstr = Marshal.SecureStringToBSTR(value);
            try
            {
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }

        /// <summary>
        /// Converts string to secure string
        /// </summary>
        /// <param name="current">
        /// The current.
        /// </param>
        /// <returns>
        /// </returns>
        public static SecureString ToSecure(this string current)
        {
            var secure = new SecureString();
            foreach (var c in current.ToCharArray()) secure.AppendChar(c);
            return secure;
        }
    }
}
