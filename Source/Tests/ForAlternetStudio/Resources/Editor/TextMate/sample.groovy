// groovy -cp $HOME/.m2/repository/com/h2database/h2/1.3.166/h2-1.3.166.jar groovySql

// http://groovy.codehaus.org/Tutorial+6+-+Groovy+SQL

// Grab will not load the driver! but at least we can trigger it to download it and then use groovy -cp option to run this script.
// @Grab('com.h2database:h2:1.3.166')

import groovy.sql.Sql
sql = Sql.newInstance(
	'jdbc:h2:~/test', 
	'sa',
	'', 
	'org.h2.Driver')
sql.eachRow('select * from USER') { 
	println "${it.id}, ${it.firstName}"
}
/** Walk a dir that excludes all matching conditions, and then let fileHandler to process the files. */
def walk(File file, List excludeDirs, List excludeFileExts, Closure fileHandler) {
	if (file.isDirectory()) {
		normalizedPath = file.path.replaceAll("\\\\", "/")
		if (!excludeDirs.find{ dir -> normalizedPath =~ dir })
			file.eachFile{ subFile -> walk(subFile, excludeDirs, excludeFileExts, fileHandler) }
	} else if (file.isFile()) {	
		if (!excludeFileExts.find{ ext -> file.name.endsWith(ext) })
			fileHandler(file)
	}
}


// Main script
//   - Go into a directory and find all files that has 'Copyright (c)' text in header.
dir = args[0]
excludeDirs = ['\\./hg', '^\\./target']
excludeFileExts = ['.orig']
walk(new File(dir), excludeDirs, excludeFileExts) { file ->
	file.withReader() { reader ->
		while((line = reader.readLine()) != null) {
			matcher = (line =~ /Copyright \(c\)/)
			if (matcher) {
				println([matcher, file])
				break
			}
		}
	}
}