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
--0:商户;1:业务员 
Role int,
Password varchar(32),
Deleted bit default 0
)
alter table Account add Auth varchar(200)
alter table Account add LatestLoginTime datetime

create table [Transfer] (
TransferID int identity primary key,
AccountID int,
ShopID int,
PlateNumber varchar(20),
--0:过户  1:转籍
[Type] int,
--0:待补 1:完成
[Status] int,
CarType varchar(20),
Buyer varchar(20),
BuyerMobile varchar(40),
BuyerAddress varchar(400),
Seller varchar(20),
SellerMobile varchar(40),
SellerAddress varchar(400),
TransferRegion varchar(200),
Price decimal
)

alter table [Transfer] add IsUpload bit default 0
update [Transfer] set IsUpload=0
alter table [Transfer] add AddTime DateTime default GetDate()
update [Transfer] set AddTime=GetDate()


--2014/10/30
create table Photo(
PhotoID int identity primary key,
AccountID int,
TransferID int,
Photo varchar(255),
Description varchar(200),
AddTime datetime default GetDate()
)

