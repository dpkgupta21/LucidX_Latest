using System;
using System.Collections.Generic;
using System.Text;

namespace LucidX.ResponseModels
{
	public class ResultDoc
	{
		public bool isAuthenticate { get; set; }
		public object[] UserProperties { get; set; }
	}
}
