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
