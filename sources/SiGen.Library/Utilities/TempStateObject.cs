using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Utilities
{
	public class TempStateObject : IDisposable
	{
		//private Action InitState;
		private Action EndState;

		public bool IsDisposed { get; private set; }

		public TempStateObject(Action initState, Action endState)
		{
			//InitState = initState;
			EndState = endState;

			initState();
		}

		public void Dispose()
		{
			if (!IsDisposed)
			{
				EndState();
				IsDisposed = true;
			}
		}
	}
}
