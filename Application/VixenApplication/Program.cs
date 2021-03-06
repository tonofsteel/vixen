﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Common.Controls;

namespace VixenApplication
{
	internal static class Program
	{
		private static NLog.Logger Logging = NLog.LogManager.GetCurrentClassLogger();
		private const string ErrorMsg = "An application error occurred. Please contact the Vixen Dev Team " +
									"with the following information:\n\n";
		private static VixenApplication _app;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			try
			{
				AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
				Application.ThreadException += Application_ThreadException;
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				_app = new VixenApplication();
				Application.Run(_app);
			}
			catch (Exception ex)
			{
				LogMessageAndExit(ex);
			}
		}

		static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
		{
			LogMessageAndExit(e.Exception);
			
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
		{ 
			var e = (Exception)args.ExceptionObject;
			LogMessageAndExit(e);
		}

		private static void LogMessageAndExit(Exception ex)
		{
			// Since we can't prevent the app from terminating, log this to the event log. 
			Logging.Fatal(ErrorMsg, ex);
			if (_app != null)
			{
				_app.RemoveLockFile();
			}
			Environment.Exit(1);
		}

	}
}