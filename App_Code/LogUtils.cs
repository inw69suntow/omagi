using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

/// <summary>
/// Summary description for LogUtils
/// </summary>
public class LogUtils
{
	private LogUtils()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public readonly static ILog LOGGER = LogManager.GetLogger("log");
  

}