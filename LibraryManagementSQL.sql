Create Database LibraryManagement
create table Login 
(
  user_id int primary key,
  password varchar(100)
)
create table Book 
(
  book_id int identity(1,1) primary key,
  book_name varchar(100),
  author_name varchar(100),
  publication_name varchar(100),
  available_stock int  
)

create table Student
(
  student_id int identity(1,1) primary key,
  roll_number int,
  student_name varchar(100),
  department varchar(100),
  address varchar(200)
)
Drop table Student
create table Book_issue 
(
  issue_id int identity(1,1) primary key,
  student_id Int ,
  book_id Int ,  
  date_of_issue datetime,
  date_of_return datetime,
  Foreign key (book_id) references Book(book_id),
  Foreign key (student_id) references Student(student_id)
  
)
Insert into Login(user_id, password)
values (1, 'Revathi@04'),
       (2, 'Sweety@27');
select * from Login
select * from Book
select * from Student
select * from Book_issue