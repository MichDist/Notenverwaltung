# Notenverwaltung
This application is the the result of a school assignement which was then extented with some features I thought were useful.

The inital task was to create a command line application in which the user can enter his grades for every subject. These grades can be changed later on as well. Furthermore there are options to review entered grades and calculate the overall average and the corresponding overall grade. 
In a later assignement the application was supposed to support a schedule. It was initalised with our current schedule, but the user can change it by selecting the day and hour. Finally the user can print the formatted schedule in a .txt file.
The aim of these school assignements was to learn about arrays and practice basic programming skills.

I decided to extend the application with some additional features in order to learn more about certain topics.
Therefore I started by creating a new table in a Postgresql database in which the entered grades including other relevant information such a the subject, date, etc. could be stored. There are two ways in order to achieve this: 
1. Create the SQL string directly in the application and then send it to the database.
2. Create a stored procedure in the database that can be called from the application. 

For longer and more complex queries the second option with the stored procedure seems better as you don't have that long string in your C# code. Plus it's better for long term maintainability to have your SQL all in the database and not blend in within hundreds of lines of code.

Currently there is no option to retrieve the data. That will be a task for the future.
