# Library Webpage

#### Two-Day Project for Epicodus, 07/20/2016

#### By Shradha Pulla & Stewart Cole

## Description

This program will serve as a catalog for a library, and allow patrons to check out books.

## Setup/Installation Requirements

This program can only be accessed on a PC with Windows 10, and with Git, Atom, and Sql Server Management Studio (SSMS) installed.

* Clone this repository
* Import the database and test database:
  * Open SSMS
  * Select the following buttons from the top nav bar to open the database scripts file: File>Open>File>"Desktop\Library\Sql Databases\library.sql"
  * Save the library.sql file
  * To create the database: click the "!Execute" button on the top nav bar
  * To create the database another way type the following into the top of the sql file:
    * CREATE DATABASE library
    * GO
  * Refresh SSMS
  * Repeat the above steps to import the test database
* Test the program:
  * Type following command into PowerShell > dnx test
  * All tests should be passing, if not run dnx test again
* View the web page:
  * Type following command into PowerShell > dnx kestrel
  * Open Chrome and type in the following address: localhost:5004

## Known Bugs

No known bugs.

## Specifications

The program should ... | Example Input | Example Output
----- | ----- | -----
Add a new book to the catalog | --- | ---
Find a book from the catalog by author or title | --- | ---
View a book from the catalog | --- | ---
Update a book entry in the catalog | --- | ---
Delete a book from the catalog | --- | ---
Find how many copies of a book there are | --- | ---
View all books in the catalog | --- | ---
Add an author to a book | --- | ---

## Future Features

HTML | CSS | C#
----- | ----- | -----
----- | ----- | -----

## Support and Contact Details

Contact Epicodus for support in running this program.

## Technologies Used

* HTML
* CSS
* Bootstrap
* C#

## License

*This software is licensed under the Microsoft ASP.NET license.*

Copyright (c) 2016 Shradha Pulla & Stewart Cole
