using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBR.Web
{
    public class JsonHandler
    {

        public static JsonMessage CreateMessage(int ptype,string pmessage,string pvalue)
        {
            JsonMessage json = new JsonMessage()
            {
                type = ptype,
                message = pmessage,
                value = pvalue
            };
            return json;
        }
        public static JsonMessage CreateMessage(int ptype, string pmessage)
        {
            JsonMessage json = new JsonMessage()
            {
                type = ptype,
                message = pmessage,
            };
            return json;
        }

        internal static object CreateMessage(int v, object insertSucceed)
        {
            throw new NotImplementedException();
        }
    }

    public class JsonMessage
    {
        public int type{get;set;}
        public string message{get;set;}
        public string value{get;set;}
    }
}
