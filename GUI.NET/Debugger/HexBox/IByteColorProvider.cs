using System;
using System.Drawing;

namespace Be.Windows.Forms
{
	public interface IByteColorProvider
	{
		void Prepare(long firstByteIndex, long lastByteIndex);
		Color GetByteColor(long firstByteIndex, long byteIndex, out Color bgColor);
	}
}
