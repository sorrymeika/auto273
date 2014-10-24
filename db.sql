create table Admin(
AdminID int identity primary key,
AdminName varchar(100),
Password varchar(32)
)

insert into Admin (AdminName,Password) values ('admin','E10ADC3949BA59ABBE56E057F20F883E')

create table Shop (
ShopID int identity primary key,
ShopName varchar(20),
Sort int default 0,
Deleted bit default 0
)

create table Account (
AccountID int identity primary key,
AccountName varchar(20),
ShopID int,
Name varchar(20),
Role int,
Password varchar(32),
Deleted bit default 0
)

create table [Transfer] (
TransferID int identity primary key,
AccountID int,
ShopID int,
PlateNumber varchar(20),
[Type] int,
[Status] int,
Buyer varchar(20),
BuyerMobile varchar(40),
BuyerAddress varchar(400),
Seller varchar(20),
SellerMobile varchar(40),
SellerAddress varchar(400),
TransferRegion varchar(200),
Price decimal
)