using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Infobasis.Data.DataAccess
{
    [Serializable]
    public class SerializableXmlDocument : XmlDocument, ISerializable
    {
        /// <summary>
        /// An implementation of XmlDocument that is serializable and can therefore be stored in 
        /// ASP.NET Session when in StateServer or SQLServer mode.
        /// </summary>
        public SerializableXmlDocument()
        {
        }

        protected SerializableXmlDocument(SerializationInfo info, StreamingContext context)
        {
            this.InnerXml = info.GetString("XML");
        }


        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("XML", this.InnerXml);
        }
    }	
}
