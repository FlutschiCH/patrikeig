using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AbsolutDashboard.Web.Services.Operations
{
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RebootOptions
	{
		public List<OperationStep> Steps { get; set; } = new List<OperationStep>();

		public bool Configured
		{
			get
			{
				if (this.Steps.Count > 0)
				{
					return this.Steps.All<OperationStep>((OperationStep s) => s.IsValid);
				}
				return false;
			}
		}
	}
}
