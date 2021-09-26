using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Tracer2.TracerAPI
{
	public class ThreadWatch
	{
		private static ThreadWatch instance;
		private ThreadWatch() { }

		public static ThreadWatch getInstance()
		{
			if (instance == null)
				instance = new ThreadWatch();
			return instance;
		}

		[DllImport("kernel32.dll")]
		private static extern long GetThreadTimes
			(IntPtr threadHandle, out long createionTime,
			 out long exitTime, out long kernelTime, out long userTime);

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetCurrentThread();
		public long GetThreadTimes()
		{
			IntPtr threadHandle = GetCurrentThread();

			long notIntersting;
			long kernelTime, userTime;

			long retcode = GetThreadTimes
				(threadHandle, out notIntersting,
				out notIntersting, out kernelTime, out userTime);

			bool success = Convert.ToBoolean(retcode);
			if (!success)
				throw new Exception(string.Format
				("failed to get timestamp. error code: {0}",
				retcode));
			long result = kernelTime + userTime;
			return result;
		}
	}
}
