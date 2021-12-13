using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;

namespace SWI.SoftStock.Common.Attributes
{
    [DataContract(Name = WSI18N.ElementNames.International, Namespace = WSI18N.NamespaceURI)]
    public class International
    {
        private string locale;
        private string tz;
        private List<Preferences> preferences;

        public International()
        {
        }

        [DataMember(Name = WSI18N.ElementNames.Locale)]
        public string Locale
        {
            get { return locale; }
            set { locale = value; }
        }

        [DataMember(Name = WSI18N.ElementNames.TZ)]
        public string Tz
        {
            get { return tz; }
            set { tz = value; }
        }

        [DataMember(Name = WSI18N.ElementNames.Preferences)]
        public List<Preferences> Preferences
        {
            get { return preferences; }
            set { preferences = value; }
        }

        public static International GetHeaderFromIncomeMessage()
        {          
            MessageHeaders headers = OperationContext.Current.IncomingMessageHeaders;

            foreach (MessageHeaderInfo uheader in headers.UnderstoodHeaders)
            {
                if (uheader.Name == WSI18N.ElementNames.International && uheader.Namespace == WSI18N.NamespaceURI)
                {
                    International internationalHeader = headers.GetHeader<International>(uheader.Name, uheader.Namespace);
                    return internationalHeader;
                }
            }

            return null;
        }
    }

    internal static class WSI18N
    {
        public const string Prefix = "i18n";
        public const string NamespaceURI = "http://www.w3.org/2005/09/ws-i18n";

        public static class ElementNames
        {
            public const string International = "International";
            public const string Locale = "Locale";
            public const string TZ = "TZ";
            public const string Preferences = "Preferences";
        }
    }

    public class Preferences : IXmlSerializable
    {
        private XmlNode anyElement;

        public XmlNode AnyElement
        {
            get { return anyElement; }
            set { anyElement = value; }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            XmlDocument document = new XmlDocument();
            anyElement = document.ReadNode(reader);
        }

        public void WriteXml(XmlWriter writer)
        {
            anyElement.WriteTo(writer);
        }
    }
}
