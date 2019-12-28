using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Commands
{
    public class SubmitOrder
    {
        public Guid UserId { get; set; }
        public Guid Movie { get; set; }
        public Guid Theater { get; set; }
        public string Time { get; set; }
        public int NumberOfTickets { get; set; }
    }
}
