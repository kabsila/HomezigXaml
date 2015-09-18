using System;

namespace Homezig
{
	public enum NodeDeviceType
	{
		InWallSwitch,
		Camera,
		GeneralPurposeDetector,
		RemoteControl,
		UnknowDeviceType
	};

	public class EnumtoString
	{
		public EnumtoString()
		{

		}

		public static string EnumString(Enum deviceType){

			string eString = string.Empty;
			if (deviceType.Equals (NodeDeviceType.InWallSwitch)) {
				eString = "In Wall Swtich";
			} else if (deviceType.Equals (NodeDeviceType.Camera)) {
				eString = "Camera";
			} else if (deviceType.Equals (NodeDeviceType.GeneralPurposeDetector)) {
				eString = "General Purpose Detector";
			} else if (deviceType.Equals (NodeDeviceType.RemoteControl)) {
				eString = "Remote Control";
			} else {
				eString = "UnknowDeviceType";
			}

			return eString;
		}
	}
}

