using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xwcs.indesign
{
    namespace json
    {
        public enum DataKind
        {
            Unknown = 0,
            Event = 1
        }


        public class Message
        {
            public int id { get; set; }
            public BaseData data { get; set; }
        }

        [JsonConverter(typeof(core.json.JsonSubtypes), "DataKindType")]
        [core.json.JsonSubtypes.KnownSubType(typeof(JsUnknown), DataKind.Unknown)]
        [core.json.JsonSubtypes.KnownSubType(typeof(JsEvent), DataKind.Event)]
        public class BaseData
        {
            public DataKind DataKindType { get; set; }
        }

        public class JsUnknown : BaseData
        {

        }
        /*
            bubbles : evt.bubbles,
            cancelable : evt.cancelable,
            defaultPrevented: evt.defaultPrevented,
            eventType: evt.eventType,
            id: evt.id,
            index: evt.index,
            isValid: evt.isValid,
            propagationStopped: evt.propagationStopped,
            timeStamp: evt.timeStamp
            currentTargetID: evt.currentTarget.id,
            parentID: evt.parent.id,
            targetID: evt.target.id
         */
        public class JsEvent : BaseData
        {
            public bool bubbles { get; set; }
            public bool cancelable { get; set; }
            public bool defaultPrevented { get; set; }
            public string eventType { get; set; }
            public int id { get; set; }
            public int index { get; set; }
            public bool isValid { get; set; }
            public bool propagationStopped { get; set; }
            public DateTime timeStamp { get; set; }
            public int currentTargetID { get; set; }
            public int parentID { get; set; }
            public int targetID { get; set; }
        }
    }
}





