using System.Collections.Generic;
using System.Threading;
using Rat.Data;

namespace Rat.Core
{
	public interface IRatContextAccessor
	{
		RatContext RatContext { get; set; }
	}

	public class RatContextAccessor : IRatContextAccessor
	{
		private static readonly AsyncLocal<RatContextHolder> _currentContext = new();

		public RatContext RatContext
		{
			get
			{
				return _currentContext.Value?.Context;
			}
			set
			{
				var holder = _currentContext.Value;
				if (holder != null)
				{
					// Clear current RatContext trapped in the AsyncLocals, as its done.
					holder.Context = null;
				}

				if (value != null)
				{
					// Use an object indirection to hold the RatContext in the AsyncLocal,
					// so it can be cleared in all ExecutionContexts when its cleared.
					_currentContext.Value = new RatContextHolder { Context = value };
				}
			}
		}

		private class RatContextHolder
		{
			public RatContext Context;
		}
	}

	public sealed class RatContext
	{
		public ProcessingStatus Status { get; set; }

		public IDictionary<string, string> ValidationErrors { get; init; } = new Dictionary<string, string>();

		public string FailureReason { get; set; }
	}
}
