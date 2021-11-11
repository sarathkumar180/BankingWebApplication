
Insert into Roles values (1, 'Admin');
Go
Insert into Roles values (2, 'Teller');
Go
Insert into Roles values (3, 'Customer');
Go

Insert into [dbo].[Customer] values(1111,'admin','admin','Administrator','Admin','Admin@banking.com','Las Vegas, USA','+17023456789')
Go

Insert into [dbo].[UserRolesMapping] values (1, 1);
Go