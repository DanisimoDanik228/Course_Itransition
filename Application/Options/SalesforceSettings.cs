using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Options
{
    public class SalesforceSettings
    {
        public string EndpointUrl { get;  set; }
        public string ClientId { get;  set; }
        public string ConsumerSecret{ get;  set; }
        public string SObjectName { get;  set; }
        public string ApiVersion { get;  set; }
    }
}
