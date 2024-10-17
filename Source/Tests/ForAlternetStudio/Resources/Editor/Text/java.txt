package org.icemail.mail;

import java.awt.Dimension;

import java.io.File;
import java.io.IOException;
import java.io.FileOutputStream;
import java.io.PrintStream;

import java.text.DateFormat;

import java.util.Locale;
import java.util.Properties;
import java.util.TimeZone;

import javax.mail.Session;

import org.icemail.Package;

import org.icemail.smime.SMIMELibrary;
import org.icemail.smime.SMIMEManager;

import org.icemail.util.FormatResourceBundle;
import org.icemail.util.UserProperties;


/**
 * Class ICEMail
 */
public class ICEMail
{
	private static final int Debug_ = Package.DEBUG ? Package.getLevel("ICEMail") : 0;

	private static final String    VERSION_STR = "3.0.5";

	private static final String   PROP_PREFIX = "ICEMail.";
	private static final String   PROP_PACKAGE = "org.icemail.mail";
	private static final String   DEFAULTS_RSRC = "/org/icemail/mail/defaults.properties";

	// language resources
	private static Locale                 Locale_ = null;
	private static FormatResourceBundle   Bundle_ = null;
	private static DateFormat             DateFormat_ = null;

	// mail resources
	private static Session                Session_ = null;

	// managers
	private static TempFileManager        tempFileManager_ = null;
	private static SMIMELibrary           smimeLibrary_ = null;

	//............................................................
	// Accessors

	static public String
	getVersionString() {
		return ICEMail.VERSION_STR;
	}

	static public synchronized FormatResourceBundle
	getBundle() {
		if ( ICEMail.Bundle_ == null ) {
			FormatResourceBundle.addAlias("ice", "org.icemail.mail.ICEMail");
			ICEMail.Bundle_ = FormatResourceBundle.getFormatBundle("ice", ICEMail.getLocale());
			if ( Package.DEBUG && ICEMail.Debug_ > 0 ) {
				System.out.println("ResourceBundle set to: " + ICEMail.Bundle_);
			}
		}
		return ICEMail.Bundle_;
	}

	static public synchronized Locale
	getLocale() {
		if ( ICEMail.Locale_ == null ) {
			ICEMail.Locale_ = Locale.getDefault();
			//    ICEMail.Locale_ = Locale.JAPANESE;
			//    ICEMail.Locale_ = Locale.CHINESE;
			if ( Package.DEBUG && ICEMail.Debug_ > 0 ) {
				System.out.println("Locale set to: " + ICEMail.Locale_);
			}
		}
		return ICEMail.Locale_;
	}

	static public synchronized DateFormat
	getDateFormat() {
		if ( ICEMail.DateFormat_ == null ) {
			ICEMail.DateFormat_ = DateFormat.getDateTimeInstance(DateFormat.FULL, DateFormat.FULL,
				ICEMail.getLocale());
			if ( Package.DEBUG && ICEMail.Debug_ > 0 ) {
				System.out.println("DateFormat set to: " + ICEMail.DateFormat_);
			}
		}
		return ICEMail.DateFormat_;
	}

	static public synchronized Session
	getDefaultSession() {
		if ( ICEMail.Session_ == null ) {
			DialogAuthentication auth = new DialogAuthentication();

			ICEMail.Session_ = Session.getDefaultInstance(System.getProperties(), auth);
		}
		return ICEMail.Session_;
	}

	static public synchronized TempFileManager
	getTempFileManager() {
		if ( ICEMail.tempFileManager_ == null ) {
			// create the temporary file manager
			String xprefix = UserProperties.getProperty("tempFilePrefix", "icemail-temp");
			String xsuffix = UserProperties.getProperty("tempFileSuffix", ".dat");

			ICEMail.tempFileManager_ = new TempFileManager(xprefix, xsuffix);

			// establish the temporary directory if possible
			String xdirectory = UserProperties.getProperty("tempDirectory", null);

			if ( xdirectory == null ) {
				xdirectory = UserProperties.getProperty("user.dir", null);
			}
			if ( xdirectory == null ) {
				xdirectory = UserProperties.getProperty("user.home", null);
			}
			if ( xdirectory != null ) {
				ICEMail.tempFileManager_.setDirectory(xdirectory);
			}
			if ( Package.DEBUG && ICEMail.Debug_ > 0 ) {
				System.out.println("TempFileManager set to: " + ICEMail.tempFileManager_);
			}
		}
		return ICEMail.tempFileManager_;
	}

	static public synchronized SMIMELibrary
	getSMIMELibrary() {
		if ( ICEMail.smimeLibrary_ == null ) {
			try {
				ICEMail.smimeLibrary_ =
					SMIMEManager.getSMIMELibrary("org.icemail.mail.smime.SMimeUtilities");
			} catch ( Exception xex ) {
				System.err.println("WARNING: SMIME Library is not available [" + xex + "]");
				ICEMail.smimeLibrary_ = null;
			}
		}
		return ICEMail.smimeLibrary_;
	}

	//............................................................

	static public void
	main(String[] argv) {

		UserProperties.setPropertyPrefix(ICEMail.PROP_PREFIX);

		UserProperties.setDefaultsResource(ICEMail.DEFAULTS_RSRC);

		UserProperties.printContext(System.err);

		// NOTE
		// These properties modify the values used in Message-ID and boundary
		// ids. The normal behavior will put the user's account id in those
		// fields, which is considered to lessen security.
		Properties props = System.getProperties();
		props.put("mail.user", "ICEMail"); // only affects message ids
		props.put("user.name", "ICEMail"); // only affects boundries

		// REGISTER OUR MAIN DYNAMIC PROPERTIES
		String propFilename = (File.separatorChar == '/') ? ".icemailrc" : "icemailrc.txt";

		String dynamicPath = Configuration.getInstance().getHomeDirectory().getPath() +
			File.separator + propFilename;

		UserProperties.registerDynamicProperties(Configuration.DP_MAIN, dynamicPath, null);

		// PROCESS UserProperties OPTIONS
		String[] myArgs = UserProperties.processOptions(argv);

		// LOAD PROPERTIES 
		UserProperties.loadProperties(ICEMail.PROP_PACKAGE, null);

		ICEMail.processOptions(myArgs);

		TimeZone.setDefault(TimeZone.getDefault());

		// CONFIGURATION
		Configuration config = Configuration.getInstance();
		config.loadMailCap();
		config.loadMimeTypes();
		config.checkEssentialProperties();

		// CREATE MAIN WINDOW 
		MainFrame frame = MainFrame.getInstance();

		frame.show();

		MailEventThread.getInstance().start();
	}

	static public void
	shutDownICEMail() {
		System.out.println("ICEMail shutdown.");

		ComposeMgr.getInstance().closeAllFrames();

		if ( !ComposeMgr.getInstance().hasOpenWindows() ) {
			Configuration config = Configuration.getInstance();

			// save all essential properties
			MainFrame.getInstance().saveLayoutProperties();
			HotJavaMailBrowser.saveLayoutProperties();
			config.saveProperties();

			getTempFileManager().deleteAllFiles();

			System.exit(0);
		}
	}

	//............................................................

	static private void
	processOptions(String[] args) {
		int iArg = 0;

		for ( ; iArg < args.length ; ++iArg ) {
			if ( !args[iArg].startsWith("-") )
				break;

			if ( args[iArg].equals("-?") ||
				args[iArg].equals("-help") ||
			args[iArg].equals("-usage") ) {
				ICEMail.printUsage();
			} else if ( args[iArg].equals("-v") || args[iArg].equals("-version") ) {
				System.err.println("ICEMail Release: " + ICEMail.VERSION_STR);
			} else if ( args[iArg].equals("-mimeFile") ) {
				Configuration.getInstance().setMimeFileName(args[++iArg]);
			} else if ( args[iArg].equals("-mailcapFile") ) {
				Configuration.getInstance().setMailcapFileName(args[++iArg]);
			} else if ( args[iArg].equals("-err") ) {
				try {
					System.setErr(new PrintStream(new FileOutputStream(args[++iArg])));
				} catch ( IOException ex ) {
					ex.printStackTrace(System.err);
				}
			} else if ( args[iArg].equals("-out") ) {
				try {
					System.setOut(new PrintStream(new FileOutputStream(args[++iArg])));
				} catch ( IOException ex ) {
					ex.printStackTrace(System.err);
				}
			} else if ( args[iArg].equals("-errout") ) {
				try {
					PrintStream s = new PrintStream(new FileOutputStream(args[++iArg]));

					System.setOut(s);
					System.setErr(s);
				} catch ( IOException ex ) {
					ex.printStackTrace(System.err);
				}
			} else {
				System.err.println("ignoring option '" + args[iArg]);
			}
		}
	}

	static private void
	printUsage() {
		System.err.println("usage: org.icemail.mail.ICEMail [options...]");

		System.err.println("ICEMail options:");
		System.err.println("    -mailcapFile file, use file for mailcap");
		System.err.println("    -mimeFile file, use file for mimetypes");
		System.err.println("    -err file, redirect System.err to file");
		System.err.println("    -out file, redirect System.out to file");
		System.err.println("    -errout file, redirect System.out and System.err to file");
		System.err.println("    -v, -versiobn, display version info");
		System.err.println("    -?, -help, -usage, display usage info");

		UserProperties.printUsage(System.err);
	}

}
