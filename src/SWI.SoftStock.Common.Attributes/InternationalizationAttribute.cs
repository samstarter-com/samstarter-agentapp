using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Threading;

namespace SWI.SoftStock.Common.Attributes
{
    public class InternationalizationAttribute : Attribute, IOperationBehavior
    {
        private string locale;
        private string timeZone;

        public string Locale
        {
            get { return locale; }
            set
            {
                locale = value;
            }
        }

        public string TimeZone
        {
            get { return timeZone; }
            set { timeZone = value; }
        }

        #region IOperationBehavior Members

        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }


        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            if (String.IsNullOrEmpty(locale))
            {
                Locale = Thread.CurrentThread.CurrentCulture.Name;
            }
            clientOperation.Parent.MessageInspectors.Add(new InternationalizationMessageInspector(locale, timeZone));
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.Parent.MessageInspectors.Add(new InternationalizationMessageInspector());
        }

        public void Validate(OperationDescription operationDescription)
        {
        }
        #endregion

    }

    public class InternationalizationMessageInspector : IClientMessageInspector, IDispatchMessageInspector
    {
        private string locale;
        private string timeZone;

        public InternationalizationMessageInspector()
        {
        }

        public InternationalizationMessageInspector(string locale)
        {
            this.locale = locale;
        }

        public InternationalizationMessageInspector(string locale, string timeZone)
        {
            this.locale = locale;
            this.timeZone = timeZone;
        }

        #region IClientMessageInspector Members
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            International internationalHeader = new International();

            if (!String.IsNullOrEmpty(locale))
                internationalHeader.Locale = locale;

            if (!String.IsNullOrEmpty(timeZone))
                internationalHeader.Tz = timeZone;

            MessageHeader header = MessageHeader.CreateHeader(WSI18N.ElementNames.International, WSI18N.NamespaceURI, internationalHeader);
            request.Headers.Add(header);
            return null;
        }

        #endregion

        #region IDispatchMessageInspector Members
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            int index = request.Headers.FindHeader(WSI18N.ElementNames.International, WSI18N.NamespaceURI);
            request.Headers.UnderstoodHeaders.Add(request.Headers[index]);

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        #endregion
    }


}
