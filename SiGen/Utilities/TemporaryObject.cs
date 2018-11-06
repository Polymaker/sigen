using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Utilities
{
	public class TemporaryObject : IDisposable
	{
		public bool IsDisposed { get; private set; }
		private Action OnDispose;

		public TemporaryObject(Action onDispose)
		{
			OnDispose = onDispose;
		}

		public TemporaryObject(Action onCreate, Action onDispose)
		{
			onCreate();
			OnDispose = onDispose;
		}

		public void Dispose()
		{
			if (!IsDisposed)
			{
				OnDispose();
				IsDisposed = true;
				GC.SuppressFinalize(this);
			}
		}

		~TemporaryObject()
		{
			if (!IsDisposed)
				Dispose();
		}
	}
}
