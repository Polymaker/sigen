using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
	public class FretPosition : LayoutComponent
	{
		private double _PositionRatio;
		private double _Cents;
		private FretManager _Owner;
		//private 

		public SIString String => _Owner.String;

		public bool UsedForComputation { get; set; }

		public double PositionRatio
		{
			get { return _PositionRatio; }
			set
			{
				SetPropertyValue(ref _PositionRatio, value);
			}
		}

		public double Cents
		{
			get { return _Cents; }
			set
			{
				SetPropertyValue(ref _Cents, value);
			}
		}

		public FretPosition(FretManager manager) : base(manager.Layout)
		{
			_Owner = manager;
		}

		public static FretPosition FromCents(double cents)
		{
			return new FretPosition(null) { _Cents = cents };
		}
	}
}
