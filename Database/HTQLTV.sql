CREATE DATABASE HTQLTV;
USE HTQLTV;

-- Create Categories Table
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL
);

-- Insert Categories
INSERT INTO Categories (CategoryName) VALUES 
(N'Công nghệ thông tin'),
(N'Ngoại ngữ'),
(N'Truyện'),
(N'Văn học');

-- Create Books Table
CREATE TABLE Books (
    BookID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    Publisher NVARCHAR(255) NOT NULL,
    YearPublished INT NOT NULL,
    CategoryID INT,
    Quantity INT NOT NULL,
    Available INT NOT NULL,
    BookImage VARCHAR(255),
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Create Readers Table
CREATE TABLE Readers (
    ReaderID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(255) NOT NULL,
    ReaderAddress NVARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    DateOfBirth DATE NOT NULL
);

-- Create Staff Table
CREATE TABLE Staff (
    StaffID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(255) NOT NULL,
    Position NVARCHAR(100) NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    Email VARCHAR(255) NOT NULL
);



-- Create Borrow_Return Table
CREATE TABLE Borrow_Return (
    BorrowReturnID INT IDENTITY(1,1) PRIMARY KEY,
    ReaderID INT,
    BookID INT,
    BookNumber INT NOT NULL,
    BorrowDate DATE NOT NULL,
    DueDate DATE NOT NULL,
    ReturnDate DATE,
    StaffID INT,
	TotalBorrowed INT DEFAULT 0,
	TotalReturned INT DEFAULT 0,
    FOREIGN KEY (ReaderID) REFERENCES Readers(ReaderID),
    FOREIGN KEY (BookID) REFERENCES Books(BookID),
    FOREIGN KEY (StaffID) REFERENCES Staff(StaffID),
   
);



-- Create Users Table
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(50) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    RoleID INT NOT NULL,
);

--Ràng buộc ngày mượn phải lớn hơn hoặc bằng ngày hiện tại
ALTER TABLE Borrow_Return
ADD CONSTRAINT CHK_BorrowDate CHECK (BorrowDate >= CAST(GETDATE() AS DATE));

--Ràng buộc hạn trả phải lớn hơn ngày mượn
ALTER TABLE Borrow_Return
ADD CONSTRAINT CHK_DueDate CHECK (DueDate > BorrowDate);

--Ràng buộc ngày trả phải lớn hơn hoặc bằng hạn trả
ALTER TABLE Borrow_Return
ADD CONSTRAINT CHK_ReturnDate CHECK (ReturnDate IS NULL OR ReturnDate >= DueDate);

ALTER TABLE Borrow_Return
DROP CONSTRAINT CHK_DueDate_OneWeek;

-- Insert Books
INSERT INTO Books (Title, Author, Publisher, YearPublished, CategoryID, Quantity, Available, BookImage)
VALUES 
(N'Liệu IT đã hết thời', N'Nicholas G.Carr', N'Nhà xuất bản Trẻ', 2013, 1, 10, 10, N'book1.jpg'),
(N'Tiếng Nhật công nghệ thông tin trong ngành phần mềm', N'Nhóm tác giả', N'Nhà xuất bản Thế giới', 2018, 1, 10, 10, N'book2.jpg'),
(N'Data Structures and Algorithms: An Easy Introduction', N'Rudolph Russell', N'Nhà xuất bản Trẻ', 2018, 1, 10, 10, N'book3.jpg'),
(N'Kiến trúc máy tính', N'NGUYỄN ĐÌNH VIỆT', N'Đại học quốc gia Hà Nội', 2018, 1, 10, 8, N'book4.jpeg'),
(N'Code Dạo Kí Sự - Lập Trình Viên Đâu Phải Chỉ Biết Code', N'Phạm Huy Hoàng', N'Nhà xuất bản Thanh niên', 2017, 1, 10, 10, N'book5.jpg'),
(N'Tự Học Lập Trình C# Bằng Hình Ảnh', N'Phạm Quang Hiển', N'Nhà xuất bản Thanh niên', 2017, 1, 10, 10, N'book6.jpg'),
(N'Giáo trình lập trình Android', N'Lê Hoàng Sơn, Nguyễn Thọ Thông', N'Nhà xuất bản Xây dựng', 2017, 1, 10, 10, N'book7.jpg'),
(N'Nền Tảng Toán Học Trong Công Nghệ Thông Tin', N'Nhóm tác giả', N'Nhà xuất bản ĐHQG HCM', 2018, 1, 10, 10, N'book8.jpg'),
(N'Bài Giảng Quản Lý Dự Án Công Nghệ Thông Tin', N'ThS. Phạm Thảo', N'Nhà xuất bản ĐH Kinh tế quốc dân', 2019, 1, 10, 10, N'book9.jpeg'),
(N'Thiết kế mạng Intranet', N'Phạm Huy Hoàng', N'Nhà Xuất Bản Bách Khoa Hà Nội', 2019, 1, 10, 10, N'book10.jpg'),
(N'Tự học 2000 từ vựng tiếng Anh theo chủ đề', N'Đỗ Nhung', N'Nhà Xuất Bản Hồng Đức', 2020, 2, 10, 10, N'book11.png'),
(N'Học nhanh nhớ lâu 1500 từ vựng tiếng Trung', N'Gia Hân', N'Nhà Xuất Bản Hồng Đức', 2021, 2, 10, 10, N'book12.jpeg'),
(N'3000 từ vựng tiếng Anh thông dụng nhất', N'Huyền Windy', N'Nhà Xuất Bản Đại học quốc gia Hà Nội', 2021, 2, 10, 10, N'book13.jpeg'),
(N'Truyện ngắn Doraemon tập 2', N'Fujiko F Fujio', N'Nhà Xuất Bản Kim Đồng', 2008, 3, 10, 10, N'book14.jpeg'),
(N'Truyện Kiều', N'Nguyễn Du', N'Nhà Xuất Bản Trẻ', 2021, 4, 10, 10, N'book15.png'),
(N'Khói trời lộng lẫy', N'Nguyễn Ngọc Tư', N'Nhà Xuất Bản Trẻ', 2021, 4, 10, 10, N'book16.jpg'),
(N'Ngồi khóc trên cây', N'Nguyễn Nhật Ánh', N'Nhà Xuất Bản Trẻ', 2021, 4, 10, 10, N'book17.jpg');

select *from Books

-- Insert Readers
INSERT INTO Readers (FullName, ReaderAddress, PhoneNumber, Email, DateOfBirth)
VALUES 
(N'Nguyễn Văn A', N'Quy Nhơn', '0123456789', 'a@example.com', '2000-01-01'),
(N'Trần Thị B', N'Quy Nhơn', '0987654321', 'b@example.com', '2002-02-02'),
(N'Phạm Văn C', N'Quy Nhơn', '0912345678', 'c@example.com', '2005-03-03'),
(N'Lê Thị D', N'Quy Nhơn', '0901234567', 'd@example.com', '2003-04-04'),
(N'Hoàng Văn E', N'Quy Nhơn', '0898765432', 'e@example.com', '2005-05-05');



-- Insert Staff
INSERT INTO Staff (FullName, Position, PhoneNumber, Email)
VALUES 
(N'Nguyễn Thị X', N'Thủ thư', '0123456788', 'x@example.com'),
(N'Trần Văn Y', N'Thủ thư', '0987654320', 'y@example.com'),
(N'Phạm Thị Z', N'Thủ kho', '0912345679', 'z@example.com'),
(N'Lê Văn W', N'Quản lý', '0901234566', 'w@example.com'),
(N'Hoàng Thị V', N'Thủ thư', '0898765431', 'v@example.com');

select *from Staff



-- Insert Borrow_Return
INSERT INTO Borrow_Return (ReaderID, BookID, BookNumber, BorrowDate, DueDate, ReturnDate, StaffID)
VALUES (1, 1, 1, '2024-06-10', '2024-06-17', '2024-06-17', 1);

INSERT INTO Borrow_Return (ReaderID, BookID, BookNumber, BorrowDate, DueDate, ReturnDate, StaffID)
VALUES (2, 2, 1, '2024-06-10', '2024-06-17', '2024-06-17', 2);

INSERT INTO Borrow_Return (ReaderID, BookID, BookNumber, BorrowDate, DueDate, ReturnDate, StaffID)
VALUES (3, 3, 2, '2024-06-10', '2024-06-17', '2024-06-17', 1);

INSERT INTO Borrow_Return (ReaderID, BookID, BookNumber, BorrowDate, DueDate, ReturnDate, StaffID)
VALUES (4, 4, 2, '2024-06-10', '2024-06-17', '2024-06-17', 2);

INSERT INTO Borrow_Return (ReaderID, BookID, BookNumber, BorrowDate, DueDate, ReturnDate, StaffID)
VALUES (5, 5, 3, '2024-06-10', '2024-06-17', '2024-06-17', 5);

INSERT INTO Borrow_Return (ReaderID, BookID, BookNumber, BorrowDate, DueDate, ReturnDate, StaffID)
VALUES (1, 2, 3, '2024-06-10', '2024-06-17', NULL, 1);

-----------
-- Cập nhật trường TotalBorrowed và TotalReturned dựa trên ngày trả
UPDATE Borrow_Return
SET TotalBorrowed = BookNumber
WHERE ReturnDate IS NULL;

UPDATE Borrow_Return
SET TotalReturned = BookNumber
WHERE ReturnDate IS NOT NULL;


--------
select * FROM Borrow_Return


-- Insert Users
INSERT INTO Users (Username, Password, Email, RoleID) 
VALUES 
('staff', 'staff', 'staff1@example.com',  0),
('admin', 'admin', 'admin1@example.com', 1);

select *from Users

Use htqltv
drop table Users
drop table Borrow_Return
drop table Staff
drop table Books
drop table Readers
drop table Categories