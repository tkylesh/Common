using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace CommonAPICommon
{
    public static class Extensions
    {
        /// <summary>
        /// Decrypts a string of text using a Rijndael method with a pre-defined set of settings 
        /// </summary>
        /// <param name="encryptedString">String to be decrypted</param>
        /// <returns>string</returns>
        public static string Decrypt(this string encryptedString)
        {
            return RijndaelSimple.DoRijndael(encryptedString, EncrytionDirection.Decrypt);
        }

        public static string SeachXmlNodeRecursive(this XmlNodeList list, string search)
        {
            foreach (XmlNode node in list)
            {
                if (node.Name == search)
                {
                    return node.InnerText;
                }
            }
            foreach (XmlNode node in list)
            {
                if (node.HasChildNodes)
                {
                    return SeachXmlNodeRecursive(node.ChildNodes, search);
                }
            }
            return string.Empty;
        }

        public static DateTime? yyyyMMddToDateTime(this string value)
        {
            var parsedDate = DateTime.MinValue;
            if (DateTime.TryParseExact(value, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                return parsedDate;
            return null;
        }

        public static int? ToInt(this string value)
        {
            var asInt = 0;
            if (int.TryParse(value, out asInt))
                return asInt;
            return null;
        }

        public static DateTime? ToDateTime(this string value)
        {
            var parsedDate = DateTime.MinValue;
            if (DateTime.TryParse(value, out parsedDate))
                return parsedDate;
            return null;
        }

        public static string SafeTrim(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;
            return value.Trim();
        }

        public static string SafeTrimU(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;
            return value.Trim().ToUpper();
        }

        public static bool? YNToBool(this string value)
        {
            return !string.IsNullOrWhiteSpace(value) && value.Equals("Y", StringComparison.InvariantCultureIgnoreCase);
        }

        public static string IsReal(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;
            if (value.ToUpper().Contains("X"))
                return string.Empty;
            if (value.SafeTrim().Length != 9)
                return string.Empty;
            return value.SafeTrim();
        }

        public static string MaskedDL(this string value)
        {
            value = value.SafeTrim();

            if (value.Length >= 5)
            {
                var mask1 = value.Substring(0, value.Length - 5).PadRight(value.Length, '*');
                var maskedDLN = mask1.Substring(0, value.Length - 4).PadRight(value.Length, '0');
                return maskedDLN;
            }
            else
                return string.Empty;
        }

        public static string MaskedDOB(this DateTime value)
        {
            if (value == DateTime.MinValue)
                return string.Empty;
            else
                return string.Format("00/00/{0:yyyy}", value);
        }

        public static string MaskedSSN(this string value)
        {
            if (value.Length == 9 && !value.Contains("X"))
                return string.Format("XXX-XX-{0}", value.Substring(5, 4));
            else
                return string.Empty;
        }

        // This extension method is broken out so you can use a similar pattern with 
        // other MetaData elements in the future. This is your base method for each.
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(true);
            return attributes.Length > 0
              ? (T)attributes[0]
              : null;
        }

        // This method creates a specific call to the above method, requesting the
        // Description MetaData attribute.
        public static string GetDescription(this Enum value)
        {
            var attribute = value.GetAttribute<XmlElementAttribute>();
            return attribute == null ? value.ToString() : attribute.ElementName;
        }
    }
}
