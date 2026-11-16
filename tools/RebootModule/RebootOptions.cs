using System.Collections.Generic;
using System.Linq;

namespace AbsolutDashboard.Web.Services.Operations
{
    public sealed class RebootOptions
    {
        public List<OperationStep> Steps { get; set; } = new List<OperationStep>();

        public bool Configured => this.Steps.Count > 0 && this.Steps.All(s => s.IsValid);
    }
}
