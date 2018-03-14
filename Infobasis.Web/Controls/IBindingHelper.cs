using Infobasis.Web.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace Infobasis.Web.Controls
{
    //############################################################################
    internal interface IBindingHelper
    {
        object Eval(object container, string fieldName);
        object Eval(string format, object container, params string[] fieldNames);
        object BindKey(object container);
        object BindKey(string format, object container);
        bool IsLastRow(object container);
    }

    //############################################################################
    internal class BindingHelper
    {
        //=======================================================================
        public static object Eval(object container, string fieldName)
        {
            object dataItem = getDataItem(container);
            if (dataItem is DataRow)
            {
                DataRow row = (DataRow)dataItem;
                return row[fieldName];
            }
            else if (dataItem is DataRowView)
            {
                DataRowView row = (DataRowView)dataItem;
                return row[fieldName];
            }
            else if (dataItem is Hashtable)
            {
                Hashtable hashtable = (Hashtable)dataItem;
                return hashtable[fieldName];
            }
            else if (dataItem is XmlNode)
            {
                XmlNode node = (XmlNode)dataItem;
                XmlNodeList nodes = node.SelectNodes(fieldName); //fieldName=xpath
                if (nodes.Count == 1 && nodes[0].NodeType == XmlNodeType.Attribute)
                    return nodes[0].Value;
                else if (nodes.Count == 0)
                    return null;
                else
                    return nodes;
            }
            else
                throw new NotSupportedException("Binding to items of type '" +
                    dataItem.GetType() + "' not supported.");

        }

        //=======================================================================
        public static object Eval(string format, object container, params string[] fieldNames)
        {
            object dataItem = getDataItem(container);
            object[] values = new object[fieldNames.Length];

            if (dataItem is DataRowView)
            {
                // Copy field values into an array
                DataRowView row = (DataRowView)dataItem;
                for (int i = 0; i < fieldNames.Length; i++)
                    values[i] = row[fieldNames[i]];
            }
            else if (dataItem is XmlNode)
            {
                // Copy field values into an array
                XmlNode node = (XmlNode)dataItem;
                for (int i = 0; i < fieldNames.Length; i++)
                    values[i] = node.SelectSingleNode(fieldNames[i]).InnerText;
            }
            else
                throw new NotImplementedException("Formatted binding to items of type '" +
                    dataItem.GetType() + "' not yet implemented.");

            return string.Format(format, values);

        }

        //=======================================================================
        public static object BindKey(object container)
        {
            object dataItem = getDataItem(container);
            if (dataItem is DataRowView)
            {
                try
                {
                    DataRowView row = (DataRowView)dataItem;
                    int id = (int)row["ID"];
                    return new IbPrimaryKey(id);
                }
                catch (Exception caught)
                {
                    throw new ApplicationException("Error binding key with ID", caught);
                }
            }
            else if (dataItem is XmlNode)
            {
                try
                {
                    XmlNode node = (XmlNode)dataItem;
                    int id = Convert.ToInt32(node.SelectSingleNode("ID").Value);
                    return new IbPrimaryKey(id);
                }
                catch (Exception caught)
                {
                    throw new ApplicationException("Error binding XmlNode with keyPrefix ID.", caught);
                }
            }
            else
                throw new NotSupportedException("Binding to " + container.GetType().FullName + " not supported.");
        }

        //=======================================================================
        public static object BindKey(string format, object container)
        {
            return string.Format(format, BindKey(container));
        }

        //=======================================================================
        public static bool IsLastRow(object container)
        {
            DataRowView row = (DataRowView)getDataItem(container);
            DataRowView lastRow = row.DataView[row.DataView.Table.Rows.Count - 1];
            return object.Equals(row, lastRow);
        }

        //=======================================================================
        private static object getDataItem(object container)
        {
            object dataItem = null;
            if (container is RepeaterItem)
                dataItem = ((RepeaterItem)container).DataItem;
            else if (container is DataGridItem)
                dataItem = ((DataGridItem)container).DataItem;
            else if (container is DataListItem)
                dataItem = ((DataListItem)container).DataItem;
            else if (container is XmlNode)
                dataItem = container;
            else
                throw new ArgumentException("'" + container.GetType() +
                    "' not supported as valid container.", "container");

            return dataItem;

        }
    }
}