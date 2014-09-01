MyDbUp
======

An extension of DbUp's standard starter project that adds a few commonly used features.

DbUp, in general, is a command line tool for performing database migrations and deployments.

* Specify connection string via config file or command line parameters
* Use one copy of this executable and config file against several database targets
* Allow new scripts to be recorded (journaled) without being executed

## Usage ##

    MyDbUp ACTION [OPTIONS]
    MyDbUp ACTION [SCRIPT_FOLDER] [OPTIONS]

      --scriptFolder=VALUE
      --connectionString=VALUE
      --connectionName=VALUE
      -j, --journalOnly
      -?, --help

## Example ##

An example setup might use the following folder structure:

* `./Scripts/` - base folder for SQL script actions
* `./Scripts/Create/` - stands-up a copy of the DB schema from scratch
* `./Scripts/Update/` - migration scripts to update DB to latest version
* `./Scripts/TestDB/` - environment specific data scripts for QA testing

Your deploy process can be scripted to execute only the appropriate commands for that environment.

## References ##

1. http://dbup.github.io/

2. https://github.com/ericdc1/AliaSQL

3. http://www.ndesk.org/Options

## Contact ##

dustin potter, `ifatree`@`gmail.com`