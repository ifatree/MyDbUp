MyDbUp
======

An extension of DbUp's standard starter project that adds a few commonly used features.

DbUp, in general, is a command line tool for performing database migrations and deployments.

Out of the box, DbUp's starter project has no way to:

* Specify connection string via config file or command line parameters
* Use one copy of an executable and config file against several database targets
* Allow new scripts to be recorded (journaled) without being executed

I've also made a couple of simplifying and pain-reducing assumptions, I hope.

Feel free to tinker!

## Usage ##

    MyDbUp ACTION [OPTIONS]
    MyDbUp ACTION [SCRIPT_FOLDER] [OPTIONS]

      --scriptFolder=VALUE       a base script folder that holds action folders
      --connectionString=VALUE   the connection string to use (SqlClient)
      --connectionName=VALUE
      -j, --journalOnly
      -?, --help

## Example ##

An example setup might use the following folder structure:

* `./Scripts/` - base folder for SQL script actions
* `./Scripts/Create/` - stands-up a copy of the DB schema from scratch
* `./Scripts/{version}/Update/` - migration scripts to update DB to latest version
* `./Scripts/{version}/TestDB/` - environment specific data scripts for QA testing

Your deploy process can easily be scripted to execute only the appropriate commands for that environment:

* `$ MyDbUp.exe Update ./Scripts/2.0.7`

## References ##

1. http://dbup.github.io/

2. https://github.com/ericdc1/AliaSQL

3. http://www.ndesk.org/Options

## Contact ##

dustin potter, `ifatree`@`gmail.com`