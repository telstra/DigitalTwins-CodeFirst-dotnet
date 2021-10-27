#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Telstra.Twins;
using Telstra.Twins.Attributes;
using Telstra.Twins.Models;

namespace $namespace$
{
    [Serializable]
    [DigitalTwin(Version = $version$, DisplayName = "$displayname$")]
    public class $name$ : $baseclass$
    {
        $content$

        public $name$() { }
        protected $name$(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext) { }
    }
}