using System.Collections.Generic;

namespace Sitecore.Send.Util.Services
{
    public partial class MooSendSubmitAction
    {
        public class MooSendData
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
            public List<string> CustomFields { get; set; }
        }
    }
}