using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Commands
{
    public class SubmitOrder
    {
        public int Movie { get; set; }
        public int Theater { get; set; }
        public string Time { get; set; }
        public int NumberOfTickets { get; set; }
    }
}
